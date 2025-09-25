using UnityEngine;

public class ShopManageFish : MonoBehaviour
{
    public static ShopManageFish instance;

    public ShopFishController theShopFishController;

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
