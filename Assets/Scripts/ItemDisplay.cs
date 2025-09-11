using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDisplay : MonoBehaviour
{
    public string itemName;
    public Image itemImage;
    public TMP_Text itemAmount;

    public void UpdateDisplay()
    {
        if (ItemController.instance != null)
        {
            ItemInfo info = ItemController.instance.GetItemInfo(itemName);
            if (info != null)
            {
                itemImage.sprite = info.itemSprite;
                itemAmount.text = "x" + info.itemAmount;
            }
        }
    }
}