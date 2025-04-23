using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Singleton;

    [SerializeField] List<Transform> spawnPositions;
    [SerializeField] List<GameObject> enemies;

    public Transform weaponPool;
    public PlayerController player;
    public List<WeaponSO> weaponList = new();

    int totalEnemies = 0;
    float spawnCD = 20;

    void Awake()
    {
        Singleton = this;
        weaponList = Resources.LoadAll<WeaponSO>("ScriptableObjects/").OrderBy(x => x.id).ToList();
    }

    void Start()
    {
        StartCoroutine(NextSpawn());
    }

    public void GameEnd()
    { 
        
    }

    public void OpenShop()
    { 
        
    }

    public void SpawnEnemy()
    {
        if (totalEnemies >= 20) return;
        totalEnemies++;
        Instantiate(enemies[Random.Range(0, enemies.Count)], spawnPositions[Random.Range(0, spawnPositions.Count)].position, Quaternion.identity);
    }

    public void EnemyKilled()
    { 
        totalEnemies--;
    }

    IEnumerator NextSpawn()
    {
        while (true)
        { 
            SpawnEnemy();
            spawnCD -= .5f;
            if(spawnCD <= 2) spawnCD = 2;
            yield return new WaitForSeconds(spawnCD);
        }
    }
}