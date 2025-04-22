using Cinemachine;
using UnityEngine;

public class PlayerController : MonoBehaviour, IPawn
{
    [SerializeField] FixedJoystick joystickMovement;
    [SerializeField] FixedJoystick joystickAim;
    [SerializeField] CinemachineFreeLook CMFreeLook;
    [SerializeField] Rigidbody rb;
    [SerializeField] Animator animator;
    [SerializeField] Transform weaponPos;

    GameObject weaponInHand;
    PlayerData data;
    WeaponSO currentWeapon;
    Vector3 moveDirection;

    bool canAttack = true;

    private void Start()
    {
        data = new PlayerData();
        ChangeWeapon(1);
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
        weaponInHand.SetActive(false); // Visually hide weapon
        var weapon = Instantiate(currentWeapon.prefab);
        weapon.transform.position = weaponPos.position;
        weapon.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward * currentWeapon.projectileForce + Camera.main.transform.up * 5f);
        var projectile = weapon.AddComponent<Projectile>();
        projectile.weaponSO = currentWeapon;
        projectile.rb.AddForce(Camera.main.transform.forward * currentWeapon.projectileForce + Camera.main.transform.up * 5f, ForceMode.Impulse);
        projectile.GetComponent<Collider>().enabled = true;
    }

    public void ChangeWeapon(int id)
    {
        Destroy(weaponInHand);
        currentWeapon = Weapons.Singleton.List.Find(x => x.id == id);
        weaponInHand = Instantiate(currentWeapon.prefab, weaponPos);
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