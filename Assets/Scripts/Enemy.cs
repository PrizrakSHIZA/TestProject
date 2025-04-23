using UnityEngine;

public class Enemy : MonoBehaviour, IPawn
{
    [SerializeField] Rigidbody rb;
    [SerializeField] Animator animator;
    [SerializeField] Transform weaponPos;

    GameObject weaponInHand;
    PawnData data;
    WeaponSO currentWeapon;
    Vector3 moveDirection;

    bool canAttack = true;

    private void Start()
    {
        data = new PawnData(50, 3, 5, 0);
        ChangeWeapon(5);
    }

    public void TakeDamage(int damage)
    {
        Debug.Log($"Take damage {damage}");
    }

    public void Attack()
    {
        if (canAttack)
        {
            canAttack = false;
            animator.SetTrigger("Attack");
            Invoke(nameof(ResetAttack), currentWeapon.attackCD);
        }
    }

    void ResetAttack()
    {
        canAttack = true;
        weaponInHand.SetActive(true);
    }

    public void LaunchProjectile()
    {
        Vector3 attackVector = transform.forward * currentWeapon.projectileForce + transform.up * 8.5f - transform.right * 0.5f;
        weaponInHand.SetActive(false); // Visually hide weapon
        var weapon = Instantiate(currentWeapon.prefab);
        weapon.transform.position = weaponPos.position;
        weapon.transform.rotation = Quaternion.LookRotation(attackVector);
        var projectile = weapon.AddComponent<Projectile>();
        projectile.weaponSO = currentWeapon;
        projectile.ignore = gameObject;
        projectile.rb.AddForce(attackVector, ForceMode.Impulse);
        projectile.GetComponent<Collider>().enabled = true;
    }

    public void ChangeWeapon(int id)
    {
        Destroy(weaponInHand);
        currentWeapon = GameController.Singleton.weaponList.Find(x => x.id == id);
        weaponInHand = Instantiate(currentWeapon.prefab, weaponPos);
        // Easy search because of simple architecture. For more complex architecture should be changed(Trails should be stored in Projectile)
        for (int i = 0; i < weaponInHand.transform.childCount; i++)
        {
            weaponInHand.transform.GetChild(i).gameObject.SetActive(false);
        }
        weaponInHand.transform.localRotation = currentWeapon.rotationOffset;
        weaponInHand.transform.localPosition = currentWeapon.positionOffset;
        weaponInHand.GetComponent<Rigidbody>().isKinematic = true;
    }

    public void Move(Vector3 direction)
    {
        rb.AddForce(direction * data.moveSpeed * 10f);
        // Speed limit
        if (rb.velocity.magnitude > data.maxSpeed)
            rb.velocity = rb.velocity.normalized * data.maxSpeed;
    }

    public void HandleAim()
    {
        transform.LookAt(GameController.Singleton.player.transform);
        Vector3 rot = transform.rotation.eulerAngles;
        rot.x = 0;
        transform.rotation = Quaternion.Euler(rot);
    }

    public void HandleMovement()
    {
        Vector3 direction = Vector3.zero;

        if ((transform.position - GameController.Singleton.player.transform.position).magnitude > 10)
            direction = (GameController.Singleton.player.transform.position - transform.position).normalized;
        else if ((transform.position - GameController.Singleton.player.transform.position).magnitude < 8)
            direction = -(GameController.Singleton.player.transform.position - transform.position).normalized;

        rb.MovePosition(transform.position + direction * Time.fixedDeltaTime * data.moveSpeed);
    }

    private void Update()
    {
        HandleAim();
        if (Vector3.Distance(GameController.Singleton.player.transform.position, transform.position) <= 10)
            Attack();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }
}
