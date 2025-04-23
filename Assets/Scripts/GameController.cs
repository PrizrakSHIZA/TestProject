using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Singleton;

    [SerializeField] List<Transform> spawnPositions;
    [SerializeField] List<GameObject> enemies;
    [SerializeField] OverlayUI overlayUI;

    public Transform weaponPool;
    public PlayerController player;
    public List<WeaponSO> weaponList = new();

    int totalEnemies = 0;
    int money = 0;
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

    public void PurchaseItem(int id)
    {
        if (id == 0) // health buy
        {
            if (money >= 100)
            {
                money -= 100;
                overlayUI.UpdateCoins(money);
                player.SetHealth(100);
                overlayUI.UpdateHealth(100);
            }
        }
        else
        {
            if (money >= weaponList[id].price)
            {
                money -= weaponList[id].price;
                player.ChangeWeapon(id);
                overlayUI.UpdateCoins(money);
            }
        }
    }

    public void GameEnd()
    {
        overlayUI.OpenGameEndMenu();
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
        money += Random.Range(5, 16);
        overlayUI.UpdateCoins(money);
    }

    public void PlayerHealthChanged(int newValue)
    {
        overlayUI.UpdateHealth(newValue);
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