using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    [Header("Rotation speed")]
    public float x;
    public float y;
    public float z;

    void Update()
    {
        gameObject.transform.Rotate(new Vector3(x, y, z));
    }
}
