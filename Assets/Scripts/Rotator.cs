using UnityEngine;

public class Rotator : MonoBehaviour
{
    public Vector3 rotation;
    Collider collider;

    private void Start()
    {
        collider = GetComponent<Collider>();
    }

    void Update()
    {
        if(collider.enabled)
            transform.Rotate(rotation);
    }
}
