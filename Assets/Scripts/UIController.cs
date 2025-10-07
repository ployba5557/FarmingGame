using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class UIController : MonoBehaviour
{

    public static UIController instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            print("UIController destroy");
            Destroy(gameObject);

        }
        else
        {
            print("UIController instance");
            instance = this;
            DontDestroyOnLoad(gameObject);

        }


    }


    public GameObject[] toolbarActivatorIcons;

    public TMP_Text timeText;
    public TMP_Text dateText;

    public InventoryController theIC;
    public ShopController theShop;
    public ShopFishController theShopFish;
    public ShopToolController theShopTool;

    public Image seedImage;

    public TMP_Text moneyText;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.iKey.wasPressedThisFrame)
        {
            theIC.OpenClose();
        }
        //#if UNITY_EDITOR

        //        if (Keyboard.current.tKey.wasPressedThisFrame)
        //        {
        //            theShop.OpenClose();
        //        }
        //#endif 
    }


    public void OpenShop(string shopType)
    {
        // ตรวจสอบว่า Inventory เปิดอยู่หรือไม่ ถ้าเปิดให้ปิด
        if (theIC.gameObject.activeSelf)
        {
            theIC.OpenClose();
        }
        // สั่งให้ ShopController เปิดร้านค้าที่ถูกต้อง
        theShop.OpenShop(shopType);
    }


    public void SwitchTool(int selected)
    {
        foreach(GameObject icon in toolbarActivatorIcons)
        {
           icon.SetActive(false); 
        }

        toolbarActivatorIcons[selected].SetActive(true);   
    }

    public void UpdateTimeText(float currentTime)
    {
        int hour = Mathf.FloorToInt(currentTime % 24); // แปลงให้ไม่เกิน 24 ชม.
        int minute = Mathf.FloorToInt((currentTime - Mathf.Floor(currentTime)) * 60);

        string timeFormatted = string.Format("{0:00}:{1:00}", hour, minute);
        timeText.text = timeFormatted;

        // แสดงวันที่
        if (TimeController.instance != null)
        {
            System.DateTime date = TimeController.instance.currentDate;
            string shortDay = GetThaiDayName(date.DayOfWeek);
            dateText.text = $"{shortDay}. {TimeController.instance.currentDay}";
        }
    }

    private string GetThaiDayName(System.DayOfWeek dayOfWeek)
    {
        switch (dayOfWeek)
        {
            case System.DayOfWeek.Sunday: return "Sun";
            case System.DayOfWeek.Monday: return "Mon";
            case System.DayOfWeek.Tuesday: return "Tue";
            case System.DayOfWeek.Wednesday: return "Wed";
            case System.DayOfWeek.Thursday: return "Thu";
            case System.DayOfWeek.Friday: return "Fri";
            case System.DayOfWeek.Saturday: return "Sat";
            default: return "";
        }
    }


    public void SwitchSeed(CropController.CropType crop)
    {
       seedImage.sprite = CropController.instance.GetCropInfo(crop).seedType;

        AudioManager.Instance.PlaySFXPitchAdjusted(5);
    }

    public void UpdateMoneyText(float currentMoney)
    {
        moneyText.text = "$" + currentMoney;
    }

    

   


}
