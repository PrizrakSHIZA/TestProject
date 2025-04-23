using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;

public class OverlayUI : MonoBehaviour
{
    [SerializeField] CanvasGroup gameEndMenu;
    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] TextMeshProUGUI heatlhText;


    public void OpenGameEndMenu()
    {
        gameEndMenu.gameObject.SetActive(true);
        gameEndMenu.DOFade(1f, 1f);
    }

    public void UpdateHealth(int value)
    {
        if(value <= 0) value = 0;
        heatlhText.text = value.ToString();
    }

    public void UpdateCoins(int value)
    {
        coinText.text = value.ToString();
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(0);
    }
}