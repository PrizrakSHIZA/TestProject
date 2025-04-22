using UnityEngine;

public class Prjectile : MonoBehaviour
{
    Rigidbody rb;

    public WeaponSO weaponSO;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Enemy")
        {
            //other.GetComponent<PlayerView>().TakeDamage(weaponSO.damage);
        }
        else
        {
            rb.drag = 1f;
        }
    }

    private void Update()
    {
        if (rb.velocity.magnitude < 0.01f)
            rb.isKinematic = true;
    }
}