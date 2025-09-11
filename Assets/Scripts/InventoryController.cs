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
        if(UIController.instance.theShop.gameObject.activeSelf == false)
        {
           if (gameObject.activeSelf == false)
           {
                gameObject.SetActive(true);

                UpdateDisplay();
           }
           else
           {
                gameObject.SetActive(false); 
           } 
        
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
