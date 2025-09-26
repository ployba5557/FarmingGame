using UnityEngine;

public class ShopController : MonoBehaviour
{
    public ShopSeedDisplay[] seeds;
    public ShopCropDisplay[] crops;

    public ShopMushroomDisplay[] items;

    public GameObject shopPanel;

    public void OpenClose()
    {
        // เดิม: if (UIController.instance != null && !UIController.instance.theIC.gameObject.activeSelf)
        // ปรับเป็น: ถ้าไม่มี UIController ให้ถือว่าผ่าน, ถ้ามีก็ต้องให้ inventory ปิดอยู่
        bool canOpen = true;
        if (UIController.instance != null)
        {
            canOpen = UIController.instance.theIC != null
                      ? !UIController.instance.theIC.gameObject.activeSelf
                      : true;
        }

           // 🚩 ย้ายการประกาศและการกำหนดค่า 'target' ขึ้นมาก่อน
        GameObject target = shopPanel != null ? shopPanel : gameObject;

        // 🚩 ลำดับที่ 1: ตรวจสอบว่าสามารถเปิดได้หรือไม่
        if (!canOpen) return;
        
        // 🚩 ลำดับที่ 2: สลับสถานะของ Shop Panel (เปิด/ปิด)
        target.SetActive(!target.activeSelf);
        
        // 🚩 ลำดับที่ 3: ถ้า Panel ถูกเปิดใช้งาน (activeSelf เป็น true) ให้อัปเดตข้อมูล UI
        if (target.activeSelf)
        {
            if (seeds != null)
            {
                foreach (var seed in seeds)
                {
                    if (seed != null) seed.UpdateDisplay();
                }
            }

            if (crops != null)
            {
                foreach (var crop in crops)
                {
                    if (crop != null) crop.UpdateDisplay();
                }
            }
            
            // อัปเดตการแสดงผลสำหรับไอเท็มทั่วไป (เห็ด)
            if (items != null)
            {
                foreach (var item in items)
                {
                    if (item != null) item.UpdateDisplay();
                }
            }
        }
    }

    public void OpenShop(string shopType)
    {
        // ถ้าต้องการประเภทของร้านเพิ่มเติม ให้ประยุกต์จาก shopType ที่นี่
        if (shopPanel != null) shopPanel.SetActive(true);
        else gameObject.SetActive(true);
    }

    public void CloseShop()
    {
        if (shopPanel != null) shopPanel.SetActive(false);
        else gameObject.SetActive(false);
    }
}
