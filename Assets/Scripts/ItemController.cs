using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public static ItemController instance;

    public List<ItemInfo> allItems = new List<ItemInfo>();

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
        }
    }

    public ItemInfo GetItemInfo(string itemName)
    {
        return allItems.Find(i => i.itemName == itemName);
    }

    public bool SellItem(string itemName, int amountToSell)
    {
        // ค้นหา ItemInfo จาก allItems
        ItemInfo item = allItems.Find(i => i.itemName == itemName);

        if (item != null)
        {
            // 1. ตรวจสอบว่ามีไอเท็มเพียงพอ
            if (item.itemAmount >= amountToSell)
            {
                // 2. คำนวณรายได้ (ใช้ itemPrice)
                float sellPrice = item.itemPrice * amountToSell;

                // 3. หักไอเท็ม
                item.itemAmount -= amountToSell;

                // 4. เพิ่มเงิน (ต้องมี CurrencyController)
                if (CurrencyController.instance != null)
                {
                    CurrencyController.instance.AddMoney(sellPrice);
                }

                // 5. อัปเดต UI Inventory
                if (UIController.instance != null && UIController.instance.theIC != null)
                {
                    UIController.instance.theIC.UpdateDisplay();
                }

                return true;
            }
            else
            {
                Debug.LogWarning($"Cannot sell {itemName}. Not enough items.");
                return false;
            }
        }

        Debug.LogWarning($"Item {itemName} not found.");
        return false;
    }

}