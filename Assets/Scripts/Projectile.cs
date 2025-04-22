using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Rigidbody rb;
    public WeaponSO weaponSO;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();    
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (rb.isKinematic) return;

        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<IPawn>().TakeDamage(weaponSO.damage);
            transform.SetParent(collision.gameObject.transform);
        }
        rb.isKinematic = true;
    }

    private void FixedUpdate()
    {
        if (weaponSO.LookAtVelocity && !rb.isKinematic && rb.velocity.magnitude > 0)
        {
            transform.rotation = Quaternion.LookRotation(rb.velocity);
        }
    }
}