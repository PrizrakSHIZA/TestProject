using Cinemachine;
using UnityEngine;

public class PlayerView : MonoBehaviour, IPlayerView
{
    [SerializeField] FixedJoystick joystickMovement;
    [SerializeField] FixedJoystick joystickAim;
    [SerializeField] CinemachineFreeLook CMFreeLook;
    [SerializeField] Rigidbody rb;
    [SerializeField] Animator animator;

    PlayerPresenter presenter;

    Vector3 moveDirection;

    private void Start()
    {
        presenter = new PlayerPresenter(this, rb);
    }

    public void Attack()
    {
        presenter.Attack();
    }

    public void TakeDamage()
    {
        throw new System.NotImplementedException();
    }

    public void HandleAim()
    {
        // Redirecting inputs to cinamchine, cuz virtual joystick doersn` work with Input system(which CM is using by default)
        CMFreeLook.m_XAxis.Value += joystickAim.Horizontal * CMFreeLook.m_XAxis.m_MaxSpeed * Time.deltaTime;
        CMFreeLook.m_YAxis.Value -= joystickAim.Vertical * CMFreeLook.m_YAxis.m_MaxSpeed * Time.deltaTime;

        presenter.Aim(joystickAim.Horizontal, joystickAim.Vertical);
    }

    public void HandleMovement()
    {
        moveDirection = transform.forward * joystickMovement.Vertical + transform.right * joystickMovement.Horizontal;
        animator.SetFloat("MoveInput", Mathf.Abs(joystickMovement.Horizontal) + Mathf.Abs(joystickMovement.Vertical));
        presenter.Move(moveDirection);
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