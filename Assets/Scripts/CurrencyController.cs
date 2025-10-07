using UnityEngine;

public class CurrencyController : MonoBehaviour
{
    public static CurrencyController instance;

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

    public float currencyMoney;

    private void Start()
    {
        UIController.instance.UpdateMoneyText(currencyMoney);
    }

    public void SpendMoney(float amountToSpend)
    {
        currencyMoney -= amountToSpend;

        UIController.instance.UpdateMoneyText(currencyMoney);

    }

    public void AddMoney(float amountToAdd)
    {
        currencyMoney += amountToAdd;

        UIController.instance.UpdateMoneyText(currencyMoney);
    }

    public bool CheckMoney(float amount)
    {
        if(currencyMoney >= amount)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
