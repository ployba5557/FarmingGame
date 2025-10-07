using UnityEngine;
using System.Collections.Generic;

// 🚩 เปลี่ยนชื่อคลาสเป็น ShopMeatController
public class ShopMeatController : MonoBehaviour
{
    // 🚩 เปลี่ยนชนิด List
    public List<ShopMeatDisplay> allMeatDisplay = new List<ShopMeatDisplay>();

    public void OpenClose()
    {
        gameObject.SetActive(!gameObject.activeSelf);

        if (gameObject.activeSelf)
        {
            // 🎯 สั่งอัปเดต UI เมื่อเปิดร้าน
            UpdateAllMeatDisplay();
        }
    }

    // เมธอดใหม่สำหรับอัปเดต UI ของไอเท็มทั้งหมดในร้านเนื้อ
    public void UpdateAllMeatDisplay()
    {
        // 🚩 เปลี่ยนชนิดตัวแปรใน foreach
        foreach (ShopMeatDisplay meatDisplay in allMeatDisplay)
        {
            if (meatDisplay != null)
            {
                meatDisplay.UpdateDisplay();
            }
        }
    }
}