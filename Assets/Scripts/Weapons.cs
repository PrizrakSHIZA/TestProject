using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Weapons : MonoBehaviour
{
    public static Weapons Singleton;

    public List<WeaponSO> List = new();

    void Awake()
    {
        Singleton = this;
        List = Resources.LoadAll<WeaponSO>("ScriptableObjects/").OrderBy(x => x.id).ToList();
    }
}