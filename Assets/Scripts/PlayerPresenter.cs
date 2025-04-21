using UnityEngine;

public class PlayerPresenter : IPlayerPresenter
{
    IPlayerView view;
    PlayerModel model;
    Rigidbody rb;

    public PlayerPresenter(IPlayerView view, Rigidbody rb)
    { 
        model = new PlayerModel();
        this.view = view;
        this.rb = rb;
    }

    public void Attack()
    {
        throw new System.NotImplementedException();
    }

    public void ChangeWeapon(int index)
    {
        throw new System.NotImplementedException();
    }

    public void Move(Vector3 direction)
    {
        rb.AddForce(direction.normalized * model.moveSpeed * 10f);
        // Speed limit
        if (rb.velocity.magnitude > model.moveSpeed)
            rb.velocity = rb.velocity.normalized * model.moveSpeed;
    }
}