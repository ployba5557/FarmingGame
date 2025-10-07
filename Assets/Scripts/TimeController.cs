using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TimeController : MonoBehaviour
{

    public static TimeController instance;
    public static event System.Action OnNewDayStarted;
    public System.DateTime currentDate;



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

    public float CurrentTime;

    public float dayStart;

    public float timeSpeed = 20.0f;
    private bool timeActive;
    public int currentDay = 1;

    public string dayEndScene;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CurrentTime = dayStart;
        timeActive = true;

        currentDate = new System.DateTime(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (timeActive == true)
        {
            CurrentTime += Time.deltaTime * timeSpeed;
            //Debug.Log("Current Time: " + CurrentTime);

            if (CurrentTime >= 24.0f) // ตรวจสอบเวลาที่ 24:00 หรือ 00:00
            {
                CurrentTime = 0.0f; // รีเซ็ตเวลา
                EndDay();
            }

            if (UIController.instance != null)
            {
                UIController.instance.UpdateTimeText(CurrentTime);
            }
        }
    }
    public void EndDay()
    {
        timeActive = false;
        currentDay++;
        GridInfo.instance.GrowCrop();

        PlayerPrefs.SetString("Transition", "Wake Up");

        //StartDay();

        SceneManager.LoadScene(dayEndScene);

        currentDate = currentDate.AddDays(1);



    }

    public void StartDay()
    {
        timeActive = true;
        CurrentTime = dayStart;

        AudioManager.Instance.PlaySFX(6);

        if (ObjectSaveManager.instance != null)
        {
            ObjectSaveManager.instance.ResetDailyObjects();
        }

        if (OnNewDayStarted != null)
            OnNewDayStarted.Invoke(); // ⬅ ต้องมีบรรทัดนี้

        
    }

}
