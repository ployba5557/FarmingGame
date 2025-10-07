using UnityEngine;
using UnityEngine.SceneManagement;

// ใช้ DontDestroyOnLoad เพื่อเก็บข้อมูลการเลือกตัวละครไว้
public class CharacterSelectManager : MonoBehaviour
{

    private void Start()
    {
        AudioManager.Instance.PlayTitle();
    }

    public static CharacterSelectManager Instance;

    // ตัวแปรสำหรับเก็บการเลือก: True = ผู้หญิง, False = ผู้ชาย
    public bool isMale = false; 

    // ตั้งค่าชื่อ Scene เกมหลัก
    public string gameSceneName = "GameScene";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // เมธอดสำหรับปุ่ม "ผู้ชาย"
    public void SelectMale()
    {
        isMale = false;
        Debug.Log("Character selected: Male. Loading Game Scene...");
        StartGame();
    }

    // เมธอดสำหรับปุ่ม "ผู้หญิง"
    public void SelectFemale()
    {
        isMale = true;
        Debug.Log("Character selected: Female. Loading Game Scene...");
        StartGame();
    }

    private void StartGame()
    {
        // โหลด Scene เกมหลัก
        SceneManager.LoadScene(gameSceneName);

        AudioManager.Instance.PlayNextBGM();

        AudioManager.Instance.PlaySFXPitchAdjusted(5);
    }
}