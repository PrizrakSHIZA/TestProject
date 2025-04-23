using UnityEngine;

public class EventReciever : MonoBehaviour
{
    IPawn pawn;

    private void Start()
    {
        pawn = transform.parent.gameObject.GetComponent<IPawn>();
    }

    void ThrowFrame()
    {
        pawn.LaunchProjectile();
    }

    void StopAttack()
    { 
        pawn.StopAttack();
    }
}