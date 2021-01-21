using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Coin : MonoBehaviour
{
    TextMeshProUGUI coinslabel;
    Scene2 script;

    private void Start()
    {
        script = GameObject.Find("LevelSettings").GetComponent<Scene2>();
        coinslabel = GameObject.Find("CoinsLabel").GetComponent<TextMeshProUGUI>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            script.coins++;
            coinslabel.text = "Coins: " + script.coins;
            Destroy(gameObject);
        }
    }
}
