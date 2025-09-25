using UnityEngine;
using System.Collections.Generic;

public class ShopFishController : MonoBehaviour
{
    public List<ShopFishDisplay> allFishDisplay = new List<ShopFishDisplay>();

    public void OpenClose()
    {
        // สั่งให้ GameObject ที่สคริปต์นี้ติดอยู่เปิด-ปิด
        gameObject.SetActive(!gameObject.activeSelf);

        // ถ้าเปิดแล้ว ให้อัปเดต UI ของปลาทุกตัว
        if (gameObject.activeSelf)
        {
            UpdateAllFishDisplay();
        }
    }

    public void UpdateAllFishDisplay()
    {
        foreach (ShopFishDisplay fishDisplay in allFishDisplay)
        {
            if (fishDisplay != null)
            {
                fishDisplay.UpdateDisplay();
            }
        }
    }
}