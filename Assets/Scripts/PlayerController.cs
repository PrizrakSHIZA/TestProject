using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class PlayerController : MonoBehaviour, IPawn
{
    [SerializeField] FixedJoystick joystickMovement;
    [SerializeField] FixedJoystick joystickAim;
    [SerializeField] CinemachineFreeLook CMFreeLook;
    [SerializeField] Rigidbody rb;
    [SerializeField] Animator animator;
    [SerializeField] Transform weaponPos;
    [SerializeField] SkinnedMeshRenderer bodyRenderer;
    [SerializeField] MeshRenderer headRenderer;

    GameObject weaponInHand;
    PawnData data;
    WeaponSO currentWeapon;
    Material headMaterial, bodyMaterial;
    Vector3 moveDirection;

    bool canAttack = true;

    private void Start()
    {
        data = new PawnData();
        headMaterial = headRenderer.material;
        bodyMaterial = bodyRenderer.material;
        ChangeWeapon(1);
    }

    public void SetHealth(int amount)
    {
        data.hp = amount;
    }

    public void TakeDamage(int damage)
    {
        //Animation 
        headMaterial.DORewind();
        bodyMaterial.DORewind();
        headMaterial.DOColor(Color.red, .2f).SetEase(Ease.Flash).SetLoops(2, LoopType.Yoyo);
        bodyMaterial.DOColor(Color.red, .2f).SetEase(Ease.Flash).SetLoops(2, LoopType.Yoyo);

        // Damage calc
        data.hp -= damage;
        GameController.Singleton.PlayerHealthChanged(data.hp);
        if (data.hp <= 0)
        {
            animator.SetTrigger("Death");
            rb.isKinematic = true;
            this.enabled = false;
            GameController.Singleton.GameEnd();
        }
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

    public void StopAttack()
    {
        // Not needed - not melee attack on player
    }

    void ResetAttack()
    {
        canAttack = true;
        weaponInHand.SetActive(true);
    }

    public void LaunchProjectile()
    {
        Vector3 attackVector = Camera.main.transform.forward * currentWeapon.projectileForce + Camera.main.transform.up * 5f;
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

    public void Aim(float horizontalInput, float verticalInput)
    {
        Vector3 viewDir = transform.position - new Vector3(Camera.main.transform.position.x, transform.position.y, Camera.main.transform.position.z);
        transform.forward = viewDir.normalized;

        Vector3 inputDir = transform.forward * verticalInput + transform.right * horizontalInput;

        if (inputDir != Vector3.zero)
            transform.forward = Vector3.Slerp(transform.forward, inputDir.normalized, Time.deltaTime * data.rotationSpeed);
    }

    public void HandleAim()
    {
        // Redirecting inputs to cinamchine, cuz virtual joystick doersn` work with Input system(which CM is using by default)
        CMFreeLook.m_XAxis.Value += joystickAim.Horizontal * CMFreeLook.m_XAxis.m_MaxSpeed * Time.deltaTime;
        CMFreeLook.m_YAxis.Value -= joystickAim.Vertical * CMFreeLook.m_YAxis.m_MaxSpeed * Time.deltaTime;

        Aim(joystickAim.Horizontal, joystickAim.Vertical);
    }

    public void HandleMovement()
    {
        moveDirection = transform.forward * joystickMovement.Vertical + transform.right * joystickMovement.Horizontal;
        animator.SetFloat("MoveInput", Mathf.Abs(joystickMovement.Horizontal) + Mathf.Abs(joystickMovement.Vertical));
        Move(moveDirection);
    }

    private void Update()
    {
        HandleAim();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }
}