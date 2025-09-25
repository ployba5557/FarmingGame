// ไฟล์: PlayerInput.cs
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    // อ้างอิงถึง UIManager
    public UIManager uiManager;

    public QuestManager questManager; 

    void Update()
    {
        // ตรวจสอบว่าผู้เล่นกดปุ่ม 'Q' หรือปุ่มที่คุณตั้งค่าไว้
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // เรียกฟังก์ชันสำหรับเปิด-ปิดหน้าต่างเควส
            uiManager.ToggleQuestPanel();
        }

         if (Input.GetKeyDown(KeyCode.Space))
        {
            // เรียกฟังก์ชันเพื่ออัปเดตความคืบหน้า
            questManager.UpdateQuestProgress("Wood");
        }
    }
}