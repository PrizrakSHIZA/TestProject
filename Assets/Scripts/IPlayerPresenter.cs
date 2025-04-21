using UnityEngine;

public interface IPlayerPresenter
{
    void Move(Vector3 direction);
    void Attack();
    void ChangeWeapon(int index);
}