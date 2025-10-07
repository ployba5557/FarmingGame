using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Linq; 
using System.Collections.Generic; // 🚩 เพิ่ม: สำหรับ FindObjectsByType

public class DayEndController : MonoBehaviour
{
    public TMP_Text dayText;
    public string wakeUpScene;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(TimeController.instance != null)
        {
            // 1. อัปเดตข้อความวัน
            dayText.text = "- Day " + TimeController.instance.currentDay + " -";
        }
        
        // 2. ประมวลผลเห็ดที่ผู้เล่นพลาดไปในวันนั้น
        //    (สมมติว่า DayEndController ทำงานอยู่ในฉากที่เห็ดอยู่)
        ProcessMissedMushrooms(); 

        AudioManager.Instance.PauseMusic();
        AudioManager.Instance.PlaySFX(1);
    }

    private void Update()
    {
        // รอการกดปุ่มใดๆ เพื่อเริ่มวันใหม่
        if(Keyboard.current.anyKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame)
        {
            // 1. แจ้ง TimeController ให้เพิ่มวัน
            TimeController.instance.StartDay();

            AudioManager.Instance.ResumeMusic();
            // 2. โหลดฉากตื่นนอน/ฉากหลัก
            SceneManager.LoadScene(wakeUpScene);
        }
    }
    
    /// <summary>
    /// ค้นหาเห็ดทั้งหมดที่ยัง Active อยู่ในฉากเมื่อสิ้นสุดวัน
    /// และทำเครื่องหมายว่าถูก "พลาด" (Missed) เพื่อบังคับให้เข้าสู่ Respawn Queue แบบสุ่ม
    /// </summary>
    private void ProcessMissedMushrooms()
    {
        // 🚩 แก้ไข: ใช้ FindObjectsByType ตามที่ Unity แนะนำเพื่อลบ Warning CS0618
        GatherableMushroom[] allMushrooms = FindObjectsByType<GatherableMushroom>(FindObjectsSortMode.None);

        foreach (GatherableMushroom mushroom in allMushrooms)
        {
            // ตรวจสอบว่าเห็ดกำลังแสดงผลอยู่ใน Hierarchy (activeInHierarchy)
            if (mushroom != null && mushroom.gameObject.activeInHierarchy)
            {
                // ถ้าเห็ดกำลังแสดงผลอยู่ แสดงว่าผู้เล่น "พลาด" ที่จะเก็บมันไป
                // MarkAsMissed() จะซ่อนเห็ดและบันทึกวันที่ถูกเก็บลงใน ObjectSaveManager
                mushroom.MarkAsMissed();
            }
        }
        Debug.Log("Finished processing missed mushrooms for the day.");
    }
}