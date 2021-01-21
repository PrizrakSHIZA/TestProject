using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene2 : MonoBehaviour
{
    public int coins = 0;

    public void ButtonMenu()
    {
        Settings.CurrentLevel = "MainMenu";
        SceneManager.LoadScene("LoadingScreen");
    }
}
