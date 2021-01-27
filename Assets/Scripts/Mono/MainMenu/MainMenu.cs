using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    Animation anim;
    GameObject eventsystem;
    GameObject NotificationWindow;

    private void Start()
    {
        anim = gameObject.GetComponent<Animation>();
        eventsystem = GameObject.Find("EventSystem");
    }

    public void ToSettings()
    {
        eventsystem.SetActive(false);
        anim.Play("ToSettings");
    }

    public void ToMenu()
    {
        eventsystem.SetActive(false);
        anim.Play("ToMenu");
    }

    public void EnableButtons()
    {
        eventsystem.SetActive(true);
    }

    public void ButtonPlay()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            anim.Play("Notification");
            return;
        }
        eventsystem.SetActive(false);
        anim.Play("FadeOut");
    }

    public void LevelLoaded()
    {
        Settings.CurrentLevel = "Scene2";
        SceneManager.LoadScene("LoadingScreen");
    }
}
