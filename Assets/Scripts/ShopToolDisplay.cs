using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopToolDisplay : MonoBehaviour
{
    public string itemName;

    public Image itemImage;
    public TMP_Text amountText, priceText;

    // เมธอดสำหรับอัปเดต UI
    public void UpdateDisplay()
    {
        ItemInfo info = ItemController.instance.GetItemInfo(itemName);

        if (info != null)
        {
            itemImage.sprite = info.itemSprite;
            amountText.text = "x" + info.itemAmount;
            priceText.text = "$" + info.itemPrice;
        }
        else
        {
            // ถ้าไม่พบไอเท็ม ให้แสดงค่าเริ่มต้น
            Debug.LogError("ไม่พบไอเท็มชื่อ: " + itemName);
            itemImage.sprite = null;
            amountText.text = "x0";
            priceText.text = "$0";
        }
    }

    // เมธอดสำหรับขายไอเท็ม
    public void SellItem()
    {
        ItemInfo info = ItemController.instance.GetItemInfo(itemName);

        if (info != null && info.itemAmount > 0)
        {
            CurrencyController.instance.AddMoney(info.itemPrice);
            ItemController.instance.AddItem(itemName, -1);
            UpdateDisplay();
        }
        else
        {
            Debug.Log("ไม่มี " + itemName + " ที่จะขาย");
        }
    }
}