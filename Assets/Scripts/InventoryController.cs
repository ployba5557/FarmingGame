using UnityEngine;
using System.Collections.Generic;

public class InventoryController : MonoBehaviour
{
    public static InventoryController instance;
    public Dictionary<string, int> otherItems = new Dictionary<string, int>(); 


      private void Awake()
    {
        // ตรวจสอบและกำหนด Singleton
        if (instance == null)
        {
            instance = this;
            // ตัวเลือก: เก็บ Inventory ไว้ไม่ให้ถูกทำลายเมื่อเปลี่ยนฉาก
            // DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public SeedDisplay[] seeds;
    public CropDisplay[] crops;
    public ItemDisplay[] others;
    public GameObject inventoryScreen; 
    public GameObject inventory2;
    public GameObject otherPanel;
    
     public void AddItem(string itemID, int amount)
    {
        if (otherItems.ContainsKey(itemID))
        {
            otherItems[itemID] += amount;
        }
        else
        {
            otherItems.Add(itemID, amount);
        }

        // อัปเดต UI เมื่อมีการเปลี่ยนแปลงข้อมูล
        UpdateDisplay();
    }


    public void OpenClose()
    {
        // ตรวจสอบว่าหน้าต่างร้านค้า (ร้านผัก) หรือร้านปลาเปิดอยู่หรือไม่
        bool shopOpen = UIController.instance.theShop != null && UIController.instance.theShop.gameObject.activeSelf;
        bool fishShopOpen = UIController.instance.theShopFish != null && UIController.instance.theShopFish.gameObject.activeSelf;

        // ถ้าหน้าต่างร้านค้าใดๆ เปิดอยู่ จะไม่อนุญาตให้เปิด Inventory
        if (shopOpen || fishShopOpen)
        {
            return;
        }

        // สลับสถานะของ GameObject Inventory
        gameObject.SetActive(!gameObject.activeSelf);

        // ถ้าเปิดขึ้นมาแล้ว ให้เรียก UpdateDisplay
        if (gameObject.activeSelf)
        {
            UpdateDisplay();
        }
    }
    public void UpdateDisplay()
{
    // 1. อัปเดต SeedDisplay
    foreach(SeedDisplay seed in seeds)
    {
        seed.UpdateDisplay();
    }
    
    // 2. อัปเดต CropDisplay
    foreach (CropDisplay crop in crops)
    {
        crop.UpdateDisplay();
    }
    
    // 3. อัปเดต ItemDisplay โดยใช้ข้อมูลจาก Dictionary (เหลือลูปนี้ลูปเดียว)
     foreach (ItemDisplay item in others)
    {
        item.UpdateDisplay();
    }
    
     // 🚩 โค้ดที่ถูกต้อง: เพียงแค่สั่งให้ ItemDisplay ทุกตัวอัปเดตตัวเอง
    //    ItemDisplay จะไปดึงจำนวนจาก ItemController เอง
    foreach (ItemDisplay item in others)
    {
        item.UpdateDisplay();
    }
    
    // 🚩 ลบโค้ดซ้ำซ้อนและผิดพลาดที่เคยแทรกเข้าไป
    /*
    foreach (ItemDisplay itemDisplay in others)
    {
        if (otherItems.ContainsKey(itemDisplay.itemID)) 
        {
            itemDisplay.amount = otherItems[itemDisplay.itemID]; 
        }
        else
        {
            itemDisplay.amount = 0;
        }
        itemDisplay.UpdateDisplay();
    }
    */


    }

    public void SwitchScreens()
    {
        
        gameObject.SetActive(false);

        
        otherPanel.SetActive(true);

       
        InventoryController otherIC = otherPanel.GetComponent<InventoryController>();
        if (otherIC != null)
        {
            otherIC.UpdateDisplay();
        }
    }
}
