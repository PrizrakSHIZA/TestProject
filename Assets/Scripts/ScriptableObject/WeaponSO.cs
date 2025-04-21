using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObject/CreateWeapon")]
public class WeaponSO : ScriptableObject
{
    public int damage;
    public float projectileSpeed;
    public int attackCD;
    public GameObject prefab;
    public Quaternion rotationOffset;
}