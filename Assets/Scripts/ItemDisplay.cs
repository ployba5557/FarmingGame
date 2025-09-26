using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDisplay : MonoBehaviour
{
    // 🚩 เปลี่ยนชื่อตัวแปรนี้เพื่อความชัดเจนในการผูกกับ ItemInfo.itemName
    public string itemID; 
    
    public Image itemImage;
    public TMP_Text itemAmountText; // 🚩 เปลี่ยนชื่อตัวแปรให้ตรงตาม Type (ถ้าต้องการ)

    public void UpdateDisplay()
    {
        if (ItemController.instance != null)
        {
            // 🚩 ใช้ itemID ในการดึงข้อมูล
            ItemInfo info = ItemController.instance.GetItemInfo(itemID);
            
            if (info != null)
            {
                // ถ้ามีไอเท็มนี้ในระบบ
                itemImage.sprite = info.itemSprite;
                
                // 🚩 แสดงจำนวนเป็น "x[จำนวน]" หรือซ่อนถ้าจำนวนเป็น 0
                if (info.itemAmount > 0)
                {
                    itemAmountText.text = "x" + info.itemAmount.ToString();
                    itemImage.gameObject.SetActive(true); // แสดงไอคอนถ้ามีของ
                }
                else
                {
                    itemAmountText.text = "";
                    itemImage.gameObject.SetActive(false); // ซ่อนไอคอนถ้าไม่มีของ
                }
            }
            // (คุณอาจต้องกำหนดค่าเริ่มต้นของ itemImage.sprite และ itemAmount.text ให้เป็นค่าว่างใน Start() ด้วย)
        }
    }
}