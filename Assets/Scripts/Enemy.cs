using UnityEngine;

public class Enemy : MonoBehaviour, IPawn
{
    [SerializeField] bool isMelee = false;
    [SerializeField] Rigidbody rb;
    [SerializeField] Animator animator;
    [SerializeField] Transform weaponPos;
    [SerializeField] Transform meleePos;
    [SerializeField] Material material;


    GameObject weaponInHand;
    PawnData data;
    WeaponSO currentWeapon;
    Vector3 moveDirection;
    Vector3 simulatedInput = Vector3.zero;

    bool canAttack = true;
    bool canMove = true;

    private void Start()
    {
        data = new PawnData(50, 3, 5, 0);
        ChangeWeapon(isMelee ? Random.Range(0, 7) : Random.Range(1, 7));
    }

    public void TakeDamage(int damage)
    {
        //material
    }

    public void Attack()
    {
        if (canAttack)
        {
            canAttack = false;
            if (isMelee)
            {
                animator.SetTrigger("MeleeAttack");
                Invoke(nameof(ResetAttack), 3f);
                canMove = false;
            }
            else
            { 
                animator.SetTrigger("Attack");
                Invoke(nameof(ResetAttack), currentWeapon.attackCD);
            }
        }
    }

    public void StopAttack()
    {
        canMove = true;
    }

    void ResetAttack()
    {
        canAttack = true;
        weaponInHand.SetActive(true);
    }

    public void LaunchProjectile()
    {
        if (isMelee)
        {
            var colliders = Physics.OverlapSphere(meleePos.position, 1f);
            foreach (var collider in colliders)
            {
                if (collider.tag == "Player")
                    collider.GetComponent<IPawn>().TakeDamage(currentWeapon.damage);
            }
        }
        else
        { 
            Vector3 attackVector = transform.forward * currentWeapon.projectileForce + transform.up * currentWeapon.AIAimCorrection.y - transform.right * currentWeapon.AIAimCorrection.x;
            weaponInHand.SetActive(false); // Visually hide weapon
            var weapon = Instantiate(currentWeapon.prefab);
            weapon.transform.parent = GameController.Singleton.weaponPool;
            weapon.transform.position = weaponPos.position;
            weapon.transform.rotation = Quaternion.LookRotation(attackVector);
            var projectile = weapon.AddComponent<Projectile>();
            projectile.weaponSO = currentWeapon;
            projectile.ignore = gameObject;
            projectile.rb.AddForce(attackVector, ForceMode.Impulse);
            projectile.GetComponent<Collider>().enabled = true;
        }
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

        if (isMelee && canMove)
        {
            if ((transform.position - GameController.Singleton.player.transform.position).magnitude > 1.5f)
                direction = (GameController.Singleton.player.transform.position - transform.position).normalized;
        }
        else if (!isMelee)
        { 
            if ((transform.position - GameController.Singleton.player.transform.position).magnitude > 10)
                direction = (GameController.Singleton.player.transform.position - transform.position).normalized;
            else if ((transform.position - GameController.Singleton.player.transform.position).magnitude < 8)
                direction = -(GameController.Singleton.player.transform.position - transform.position).normalized;
        }

        simulatedInput = Vector3.Slerp(simulatedInput, direction, Time.deltaTime * 5f);
        animator.SetFloat("MoveInput", simulatedInput.magnitude);
        rb.MovePosition(transform.position + direction * Time.fixedDeltaTime * data.moveSpeed);
    }

    private void Update()
    {
        if (isMelee)
        {
            if(canMove)
                HandleAim();
            if (Vector3.Distance(GameController.Singleton.player.transform.position, transform.position) <= 1.5)
                Attack();
        }
        else
        {
            HandleAim();
            if (Vector3.Distance(GameController.Singleton.player.transform.position, transform.position) <= 10)
                Attack();
        }
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }
}
