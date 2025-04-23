using TMPro;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI buttonText;
    [SerializeField] Animator anim;
    [SerializeField] Transform itemParent;
    [SerializeField] Item itemPrefab;

    bool opened = false;

    private void Start()
    {
        LoadItems();
    }

    public void LoadItems()
    {
        for (int i = 1; i < GameController.Singleton.weaponList.Count; i++)
        {
            Item item = Instantiate(itemPrefab, itemParent);
            item.Init(GameController.Singleton.weaponList[i].icon, GameController.Singleton.weaponList[i].price.ToString(), i);
        }
    }

    public void ButtonPressed()
    {
        if (opened)
        {
            anim.Play("ShopMenuClose");
        }
        else
        {
            Time.timeScale = 0f;
            buttonText.text = "Close";
            anim.Play("ShopMenuOpen");
            opened = true;
        }
    }

    void Closed()
    {
        Time.timeScale = 1f;
        buttonText.text = "SHOP";
        opened = false;
    }
}