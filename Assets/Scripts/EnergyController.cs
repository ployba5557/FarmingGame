using UnityEngine;
using UnityEngine.UI;

public class EnergyController : MonoBehaviour
{
    public static EnergyController instance;

    public float maxEnergy = 200f;
    public float currentEnergy;
    public Image energyBarFill;

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

    void Start()
    {
        currentEnergy = maxEnergy;
        UpdateUI();
        TimeController.OnNewDayStarted += RefillEnergy;
    }

    public void UseEnergy(float amount)
    {
        currentEnergy -= amount;
        currentEnergy = Mathf.Clamp(currentEnergy, 0f, maxEnergy);
        UpdateUI();
    }

    public bool HasEnoughEnergy(float amount) // ✅ ต้องมีอันนี้
    {
        return currentEnergy >= amount;
    }

    public void RefillEnergy()
    {
        currentEnergy = maxEnergy;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (energyBarFill != null)
        {
            energyBarFill.fillAmount = currentEnergy / maxEnergy;
        }
    }

    private void OnDestroy()
    {
        TimeController.OnNewDayStarted -= RefillEnergy;
    }
}
