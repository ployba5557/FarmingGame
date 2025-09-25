using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public SeedDisplay[] seeds;
    public CropDisplay[] crops;
    public ItemDisplay[] others;
    public GameObject inventoryScreen; 
    public GameObject inventory2;      
    public GameObject otherPanel;


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
        foreach(SeedDisplay seed in seeds)
        {
            seed.UpdateDisplay();
        }
        foreach (CropDisplay crop in crops)
        {
            crop.UpdateDisplay();
        }
        foreach (ItemDisplay item in others)
        {
            item.UpdateDisplay();
        }

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
