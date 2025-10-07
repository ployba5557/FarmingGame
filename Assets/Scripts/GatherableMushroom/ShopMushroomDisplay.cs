using UnityEngine;
using TMPro;
using UnityEngine.UI;

// 🚩 เปลี่ยนชื่อคลาสเป็น ShopMushroomDisplay
public class ShopMushroomDisplay : MonoBehaviour
{
    // ใช้ itemName ผูกกับชื่อเห็ดใน ItemInfo
    public string itemName; 

    public Image itemImage;
    public TMP_Text amountText, priceText;
    
    // ❌ ลบ 'public int sellAmount = 1;' ออกไป

    public void UpdateDisplay()
    {
        // 1. ดึงข้อมูลไอเท็มจาก ItemController
        ItemInfo info = ItemController.instance.GetItemInfo(itemName);

        // 2. เช็คว่าหาไอเท็มเจอหรือไม่
        if (info != null)
        {
            // 3. อัปเดต UI ให้แสดงผล
            gameObject.SetActive(true); 
            itemImage.sprite = info.itemSprite;
            
            // แสดงราคาต่อ 1 ชิ้น
            priceText.text = "$" + info.itemPrice; 
            // แสดงจำนวนไอเท็มที่มีใน Inventory ของผู้เล่น
            amountText.text = "x" + info.itemAmount;
        }
        else
        {
            // ถ้าไม่พบไอเท็ม ให้ซ่อน UI ช่องขายนี้
            gameObject.SetActive(false);
            Debug.LogWarning("ไม่พบไอเท็มชื่อ: " + itemName + " ใน ItemController system.");
        }
    }

    public void SellItem()
    {
        // 1. ดึงข้อมูลไอเท็ม
        ItemInfo info = ItemController.instance.GetItemInfo(itemName);

        // 2. ตรวจสอบว่ามีไอเท็มในคลังหรือไม่ (และต้องมีอย่างน้อย 1 ชิ้น)
        if (info != null && info.itemAmount > 0)
        {
            // 3. เพิ่มเงินให้ผู้เล่น (ใช้ราคาต่อ 1 ชิ้น)
            CurrencyController.instance.AddMoney(info.itemPrice);

            // 4. ลดจำนวนไอเท็มในคลังลง 1 ชิ้น
            // ใช้ AddItem(itemName, -1) เพื่อลดจำนวน
            ItemController.instance.AddItem(itemName, -1);

            // 5. อัปเดตหน้าจอแสดงผลในร้านค้าและ Inventory (ถ้ามี UIController)
            UpdateDisplay();
            
            // 🚩 (Optional) ถ้าคุณมี UIController.instance.theIC.UpdateDisplay() ใน ItemController.SellItem() 
            //   คุณไม่จำเป็นต้องเรียกเพิ่ม แต่ถ้า ItemController ของคุณใช้ logic แบบ AddItem(-1)
            //   คุณอาจต้องเรียก Inventory Controller เพื่ออัปเดต UI ด้วย
            // if (UIController.instance != null && UIController.instance.theIC != null) 
            // {
            //     UIController.instance.theIC.UpdateDisplay();
            // }

        }
        else
        {
            // ถ้าขายไม่สำเร็จ (มีไม่พอ)
            Debug.Log("ไม่มี " + itemName + " ที่จะขาย");
        }
    }
}