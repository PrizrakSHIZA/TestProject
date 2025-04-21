using UnityEngine;

public interface IPlayerPresenter
{
    void Move(Vector3 direction);
    void Aim(float horizontalInput, float verticalInput);
    void Attack();
    void ChangeWeapon(int index);
}