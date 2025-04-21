using Cinemachine;
using UnityEngine;

public class PlayerView : MonoBehaviour, IPlayerView
{
    [SerializeField] FixedJoystick joystickMovement;
    [SerializeField] FixedJoystick joystickAim;
    [SerializeField] CinemachineFreeLook CMFreeLook;
    [SerializeField] Rigidbody rb;

    PlayerPresenter presenter;

    Vector3 moveDirection;

    private void Start()
    {
        presenter = new PlayerPresenter(this, rb);
    }

    public void Attack()
    {
        throw new System.NotImplementedException();
    }

    public void TakeDamage()
    {
        throw new System.NotImplementedException();
    }

    public void HandleAim()
    {
        if (joystickAim.Vertical != 0 && joystickAim.Horizontal != 0)
        { 
            CMFreeLook.m_XAxis.Value += joystickAim.Horizontal * CMFreeLook.m_XAxis.m_MaxSpeed * Time.deltaTime;
            CMFreeLook.m_YAxis.Value -= joystickAim.Vertical * CMFreeLook.m_YAxis.m_MaxSpeed * Time.deltaTime;
        }
    }

    public void HandleMovement()
    {
        // Handle movement
        if (joystickMovement.Horizontal != 0 && joystickMovement.Vertical != 0)
        {
            moveDirection = transform.forward * joystickMovement.Vertical + transform.right * joystickMovement.Horizontal;
            presenter.Move(moveDirection);
        }
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