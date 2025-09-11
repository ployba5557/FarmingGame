using UnityEngine;

public class ShopController : MonoBehaviour
{
    public ShopSeedDisplay[] seeds;
    public ShopCropDisplay[] crops;

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

        if (!canOpen) return;

        GameObject target = shopPanel != null ? shopPanel : gameObject;
        target.SetActive(!target.activeSelf);

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
