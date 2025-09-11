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
}