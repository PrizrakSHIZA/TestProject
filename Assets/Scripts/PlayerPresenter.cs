using UnityEngine;

public class PlayerPresenter : IPlayerPresenter
{
    IPlayerView view;
    PlayerModel model;
    Rigidbody rb;

    bool canAttack = true;

    public PlayerPresenter(IPlayerView view, Rigidbody rb)
    { 
        model = new PlayerModel();
        this.view = view;
        this.rb = rb;
    }

    public void Attack()
    {
        if (canAttack)
        {
            
        }
    }

    public void ChangeWeapon(int index)
    {
        throw new System.NotImplementedException();
    }

    public void Move(Vector3 direction)
    {
        rb.AddForce(direction * model.moveSpeed * 10f);
        // Speed limit
        if (rb.velocity.magnitude > model.maxSpeed)
            rb.velocity = rb.velocity.normalized * model.maxSpeed;
    }

    public void Aim(float horizontalInput, float verticalInput)
    {
        Vector3 viewDir = view.transform.position - new Vector3(Camera.main.transform.position.x, view.transform.position.y, Camera.main.transform.position.z);
        view.transform.forward = viewDir.normalized;

        Vector3 inputDir = view.transform.forward * verticalInput + view.transform.right * horizontalInput;

        if (inputDir != Vector3.zero)
            view.transform.forward = Vector3.Slerp(view.transform.forward, inputDir.normalized, Time.deltaTime * model.rotationSpeed);
    }
}