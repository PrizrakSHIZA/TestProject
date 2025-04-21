using UnityEngine;

public interface IPlayerView
{
    Transform transform { get; }

    void HandleMovement();
    void HandleAim();
    void Attack();
    void TakeDamage();
}