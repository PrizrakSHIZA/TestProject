using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField]
    public GameObject Coin;
    [SerializeField]
    float LifeTime = 5f;
    GameObject temp;
    
    private void Start()
    {
        InvokeRepeating("SpawnCoin", 0.5f, 0.5f);
    }

    void SpawnCoin()
    {
        temp = Instantiate(Coin, new Vector3(Random.Range(-10, 10),20, Random.Range(-10,10)), Quaternion.identity);
        Destroy(temp, LifeTime);
    }
}
