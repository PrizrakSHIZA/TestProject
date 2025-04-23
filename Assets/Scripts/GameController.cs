using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Singleton;

    public Transform weaponPool;
    public PlayerController player;
    public List<WeaponSO> weaponList = new();

    void Awake()
    {
        Singleton = this;
        weaponList = Resources.LoadAll<WeaponSO>("ScriptableObjects/").OrderBy(x => x.id).ToList();
    }
}