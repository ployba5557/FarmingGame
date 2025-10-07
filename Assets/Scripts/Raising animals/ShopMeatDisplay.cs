using UnityEngine;
using TMPro;
using UnityEngine.UI;

// 🚩 เปลี่ยนชื่อคลาสเป็น ShopMeatDisplay
public class ShopMeatDisplay : MonoBehaviour
{
    public string itemName;

    public Image itemImage;
    public TMP_Text amountText, priceText;

    // เมธอดสำหรับอัปเดต UI
    public void UpdateDisplay()
    {
        // 🎯 ดึงข้อมูลไอเท็ม
        ItemInfo info = ItemController.instance.GetItemInfo(itemName);

        if (info != null)
        {
            itemImage.sprite = info.itemSprite;
            // 💡 แสดงจำนวนไอเท็มที่มีใน Inventory
            amountText.text = "x" + info.itemAmount;
            // 💡 แสดงราคาขายของไอเท็ม (ใช้ราคาที่กำหนดใน ItemController)
            priceText.text = "$" + info.itemPrice; 
        }
        else
        {
            Debug.LogError("ไม่พบไอเท็มชื่อ: " + itemName);
            itemImage.sprite = null;
            amountText.text = "x0";
            priceText.text = "$0";
        }
    }

    // เมธอดสำหรับขายไอเท็ม (ผู้เล่นขายให้ NPC)
    public void SellItem()
    {
        ItemInfo info = ItemController.instance.GetItemInfo(itemName);

        if (info != null && info.itemAmount > 0)
        {
            // 1. เพิ่มเงิน: ได้รับเงินตามราคาขายต่อชิ้น
            CurrencyController.instance.AddMoney(info.itemPrice);
            // 2. ลบไอเท็ม: ลบออกจาก Inventory 1 ชิ้น
            ItemController.instance.AddItem(itemName, -1);
            // 3. อัปเดต UI ทันที
            UpdateDisplay();
            Debug.Log("ขาย " + itemName + " ได้เงิน $" + info.itemPrice);
        }
        else
        {
            Debug.Log("ไม่มี " + itemName + " ที่จะขาย");
        }
    }
}