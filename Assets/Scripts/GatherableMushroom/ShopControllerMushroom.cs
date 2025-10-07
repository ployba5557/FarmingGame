using UnityEngine;
using System.Collections.Generic;

// 🚩 เปลี่ยนชื่อคลาสเป็น ShopControllerMushroom
public class ShopControllerMushroom : MonoBehaviour
{
    // 🚩 เปลี่ยน List ให้ใช้คลาส ShopMushroomDisplay
    public List<ShopMushroomDisplay> allMushroomDisplay = new List<ShopMushroomDisplay>();

    public void OpenClose()
    {
        // สั่งให้ GameObject ที่สคริปต์นี้ติดอยู่เปิด-ปิด (คือ Shop Panel)
        gameObject.SetActive(!gameObject.activeSelf);

        // ถ้าเปิดแล้ว ให้อัปเดต UI ของรายการเห็ดทุกตัว
        if (gameObject.activeSelf)
        {
            UpdateAllMushroomDisplay();
        }
    }

    // 🚩 เปลี่ยนชื่อเมธอด
    public void UpdateAllMushroomDisplay()
    {
        // 🚩 วนลูปผ่านรายการ ShopMushroomDisplay
        foreach (ShopMushroomDisplay mushroomDisplay in allMushroomDisplay)
        {
            if (mushroomDisplay != null)
            {
                mushroomDisplay.UpdateDisplay();
            }
        }
    }
}