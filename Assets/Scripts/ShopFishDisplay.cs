using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopFishDisplay : MonoBehaviour
{
    public string itemName; // เปลี่ยนจาก CropType เป็น string เพื่อระบุชื่อไอเท็ม

    public Image itemImage; // เปลี่ยนชื่อเป็น itemImage
    public TMP_Text amountText, priceText;

    public void UpdateDisplay()
    {
        // 1. ดึงข้อมูลไอเท็มจาก ItemController
        ItemInfo info = ItemController.instance.GetItemInfo(itemName);

        // 2. เช็คว่าหาไอเท็มเจอหรือไม่
        if (info != null)
        {
            // 3. อัปเดต UI ให้แสดงผล
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

    public void SellItem()
    {
        // 1. ดึงข้อมูลไอเท็ม
        ItemInfo info = ItemController.instance.GetItemInfo(itemName);

        // 2. ตรวจสอบว่ามีไอเท็มในคลังหรือไม่
        if (info != null && info.itemAmount > 0)
        {
            // 3. เพิ่มเงินให้ผู้เล่น
            CurrencyController.instance.AddMoney(info.itemPrice);

            // 4. ลดจำนวนไอเท็มในคลังลง 1
            ItemController.instance.AddItem(itemName, -1);

            // 5. อัปเดตหน้าจอแสดงผล
            UpdateDisplay();
        }
        else
        {
            Debug.Log("ไม่มี " + itemName + " ที่จะขาย");
        }
    }
}