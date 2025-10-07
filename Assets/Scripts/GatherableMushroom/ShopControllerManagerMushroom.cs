using UnityEngine;

// Manager สำหรับดูแล ShopControllerMushroom
public class ShopControllerManagerMushroom : MonoBehaviour
{
    public static ShopControllerManagerMushroom instance;

    // 🚩 ผูกตัวแปรนี้กับ ShopControllerMushroom ใน Inspector
    public ShopControllerMushroom theShopMushroomController;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // ตัวเลือก: ถ้าต้องการให้ Manager อยู่ตลอด
        }
    }
}