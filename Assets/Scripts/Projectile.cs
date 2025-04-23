using UnityEngine;

public class Projectile : MonoBehaviour
{
    Collider collider;

    public Rigidbody rb;
    public WeaponSO weaponSO;
    public GameObject ignore;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (rb.isKinematic) return;
        if (collision.gameObject == ignore) return;

        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<IPawn>().TakeDamage(weaponSO.damage);
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "Weapon") Destroy(gameObject);

        collider.enabled = false;
        rb.isKinematic = true;
        Invoke(nameof(DestroyMe), 20);
    }

    private void FixedUpdate()
    {
        if (weaponSO.LookAtVelocity && !rb.isKinematic && rb.velocity.magnitude > 0)
        {
            transform.rotation = Quaternion.LookRotation(rb.velocity);
        }
    }

    void DestroyMe()
    { 
        Destroy(gameObject);
    }
}