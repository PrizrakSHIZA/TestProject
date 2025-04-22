using UnityEngine;

public class EventReciever : MonoBehaviour
{
    [SerializeField] PlayerController controller;

    void ThrowFrame()
    {
        controller.LaunchProjectile();
    }
}