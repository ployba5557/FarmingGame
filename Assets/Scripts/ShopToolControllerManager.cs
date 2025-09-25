using UnityEngine;

public class ShopToolControllerManager : MonoBehaviour
{
    public static ShopToolControllerManager instance;

    public ShopToolController theShopToolController;

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