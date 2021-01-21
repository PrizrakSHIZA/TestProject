using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public Slider slider;

    Animation anim;
    AsyncOperation gameLevel;

    void Start()
    {
        anim = gameObject.GetComponent<Animation>();
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        gameLevel = SceneManager.LoadSceneAsync(Settings.CurrentLevel);
        gameLevel.allowSceneActivation = false;
        while (gameLevel.progress < 1)
        {
            slider.value = gameLevel.progress;
            yield return new WaitForEndOfFrame();

            if (gameLevel.progress >= 0.9f)
            {
                anim.Play("FadeOut");
            }
        }
    }

    public void SceneLoaded()
    {
        gameLevel.allowSceneActivation = true;
    }
}
