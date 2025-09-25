using UnityEngine;
using System.Collections.Generic;

public class ShopToolController : MonoBehaviour
{
    public List<ShopToolDisplay> allToolsDisplay = new List<ShopToolDisplay>();

    public void OpenClose()
    {
        gameObject.SetActive(!gameObject.activeSelf);

        if (gameObject.activeSelf)
        {
            UpdateAllToolsDisplay();
        }
    }

    // เมธอดใหม่สำหรับอัปเดต UI ของไอเท็มทั้งหมดในร้าน
    public void UpdateAllToolsDisplay()
    {
        foreach (ShopToolDisplay toolDisplay in allToolsDisplay)
        {
            if (toolDisplay != null)
            {
                toolDisplay.UpdateDisplay();
            }
        }
    }
}