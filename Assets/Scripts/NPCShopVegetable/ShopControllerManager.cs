using UnityEngine;

public class ShopControllerManager : MonoBehaviour
{
    public static ShopControllerManager instance;

    public ShopController theShopController;

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
