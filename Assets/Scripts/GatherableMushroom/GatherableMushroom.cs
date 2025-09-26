using UnityEngine;

public class GatherableMushroom : MonoBehaviour
{
    // กำหนด ID ของไอเทมเห็ด (ต้องตรงกับ ItemInfo.itemName)
    public string itemID = "Wild_Mushroom"; 
    public int itemAmount = 1;
    public float disappearTime = 0.1f; 

    public void Gather()
    {
        // 1. มอบไอเทมเข้า Inventory โดยใช้ ItemController
        if (ItemController.instance != null)
        {
            // 🚩 เรียกใช้ ItemController ที่ถูกต้อง
            ItemController.instance.AddItem(itemID, itemAmount); 
            Debug.Log($"Collected {itemAmount} of {itemID} using the basket and added to ItemController.");
        }
        else
        {
            Debug.LogError("ItemController.instance is missing! Item was not collected.");
        }

        // 2. ซ่อนหรือทำลายเห็ด
        gameObject.SetActive(false); 
        // Destroy(gameObject, disappearTime); 
    }
}