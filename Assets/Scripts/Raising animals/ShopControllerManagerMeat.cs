using UnityEngine;

// 🚩 เปลี่ยนชื่อคลาสเป็น ShopMeatControllerManager
public class ShopMeatControllerManager : MonoBehaviour
{
    // 🚩 เปลี่ยนชื่อตัวแปร Instance
    public static ShopMeatControllerManager instance;

    // 🚩 เปลี่ยนตัวแปรอ้างอิง
    public ShopMeatController theShopMeatController;

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
}