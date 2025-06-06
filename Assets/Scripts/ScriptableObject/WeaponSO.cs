using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObject/CreateWeapon")]
public class WeaponSO : ScriptableObject
{
    public int id;
    public int damage;
    public float projectileForce;
    public float attackCD;
    public bool LookAtVelocity;
    public int price;
    public Sprite icon;
    public GameObject prefab;
    public Quaternion rotationOffset;
    public Vector3 positionOffset;
    public Vector2 AIAimCorrection;
}