using UnityEngine;
using UnityEngine.SceneManagement;

// ใช้ DontDestroyOnLoad เพื่อเก็บข้อมูลการเลือกตัวละครไว้
public class CharacterSelectManager : MonoBehaviour
{
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
        isMale = true;
        Debug.Log("Character selected: Male. Loading Game Scene...");
        StartGame();
    }

    // เมธอดสำหรับปุ่ม "ผู้หญิง"
    public void SelectFemale()
    {
        isMale = false;
        Debug.Log("Character selected: Female. Loading Game Scene...");
        StartGame();
    }

    private void StartGame()
    {
        // โหลด Scene เกมหลัก
        SceneManager.LoadScene(gameSceneName);
    }
}