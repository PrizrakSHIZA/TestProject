using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI priceText;
    int id;

    public void Init(Sprite sprite, string price, int id)
    {
        image.sprite = sprite;
        priceText.text = price;
        this.id = id;
    }

    public void OnButtonPressed()
    {
        GameController.Singleton.PurchaseItem(id);
    }
}