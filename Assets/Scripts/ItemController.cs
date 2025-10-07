using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public static ItemController instance;

    public List<ItemInfo> allItems = new List<ItemInfo>();

    public InventoryController inventoryUIController; 

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);

        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void AddItem(string itemName, int amount)
    {
        ItemInfo item = allItems.Find(i => i.itemName == itemName);
        if (item != null)
        {
            item.itemAmount += amount;
            Debug.Log("เพิ่ม " + itemName + " เข้า Inventory. จำนวนปัจจุบัน: " + item.itemAmount);
            UpdateInventoryUI();
        }
    }
    
     public void UpdateInventoryUI()
    {
        if (inventoryUIController != null && inventoryUIController.gameObject.activeSelf)
        {
            // ถ้าหน้าต่าง Inventory หลักเปิดอยู่ ให้อัปเดตช่องแสดงผลทั้งหมด
            inventoryUIController.UpdateDisplay();
        }
        
        // **สำคัญ:** เนื่องจากเห็ดอยู่ในหน้า "OTHERS" 
        // คุณต้องแน่ใจว่าได้อัปเดตหน้าอื่น ๆ ด้วย (ถ้ามันเปิดอยู่)
        // อาจต้องเพิ่ม Logic เพื่ออัปเดตหน้า 'otherPanel' ด้วย
        if (inventoryUIController != null && inventoryUIController.otherPanel.activeSelf)
        {
            // ต้องเข้าถึง InventoryController ของหน้า 'OTHERS' เพื่อสั่งอัปเดต
            InventoryController otherIC = inventoryUIController.otherPanel.GetComponent<InventoryController>();
            if (otherIC != null)
            {
                otherIC.UpdateDisplay();
            }
        }
    }

    public ItemInfo GetItemInfo(string itemName)
    {
        return allItems.Find(i => i.itemName == itemName);
    }

}