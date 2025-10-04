using UnityEngine;

public class GatherableMushroom : MonoBehaviour
{
    // 🚩 ต้องผูก ID ใน Inspector ให้ไม่ซ้ำกันสำหรับเห็ดแต่ละต้น
    public string uniqueID; 
    
    public string itemID = "Mushroom"; // ID ไอเท็มเห็ด (ใช้สำหรับ Quest Progress)
    public int itemAmount = 1;
    public int initialSpawnOffsetDay = 1; 

    private void Start()
    {
        // 1. ตรวจสอบสถานะเมื่อเริ่มเกม/วันใหม่ เพื่อป้องกันการเกิดใหม่ทันที
        CheckRespawnStateOnLoad();
    }

    public void Gather()
    {
        // 1. มอบไอเทมเข้า ItemController
        if (ItemController.instance != null)
        {
            ItemController.instance.AddItem(itemID, itemAmount);
            Debug.Log($"Collected {itemAmount} of {itemID}.");
            ItemController.instance.UpdateInventoryUI();
        }
        
        // 2. อัปเดตความคืบหน้าของเควส! ✅ โค้ดที่เพิ่มเข้ามา
        if (QuestManager.Instance != null)
        {
            // เรียก QuestManager โดยใช้ itemID ("Mushroom")
            QuestManager.Instance.UpdateQuestProgress(itemID);
        }

        // 3. บันทึกวันที่ถูกเก็บลงใน ObjectSaveManager
        if (ObjectSaveManager.instance != null)
        {
            ObjectSaveManager.instance.AddMushroomCollectedDay(uniqueID);
        }

        // 4. ซ่อนเห็ด
        gameObject.SetActive(false);
    }
    
    // 🚩 ✅ เมธอดนี้จะถูกเรียกโดย DayEndController
    public void MarkAsMissed()
    {
        // 1. บันทึกวันที่ถูกเก็บลงใน ObjectSaveManager
        if (ObjectSaveManager.instance != null)
        {
            ObjectSaveManager.instance.AddMushroomCollectedDay(uniqueID);
            Debug.Log($"Mushroom {uniqueID} was MISSED and marked for randomized respawn.");
        }
        
        // 2. ซ่อนเห็ด
        gameObject.SetActive(false); 
        
        // ไม่ต้องเพิ่มไอเทมเข้า Inventory หรืออัปเดตเควส
    }

    private void CheckRespawnStateOnLoad()
    {
        if (ObjectSaveManager.instance != null && TimeController.instance != null)
        {
            int currentDay = TimeController.instance.currentDay;

            if (currentDay < initialSpawnOffsetDay)
            {
                gameObject.SetActive(false);
                return; // ออกจากเมธอด ไม่ต้องตรวจสอบ Respawn Logic
            }

            // 4. ตรวจสอบว่าเห็ดควรเกิดใหม่หรือไม่ (ใช้ logic สุ่มวัน)
            if (ObjectSaveManager.instance.ShouldMushroomRespawn(uniqueID))
            {
                // ถ้าควรเกิดใหม่ (TRUE) - Logic สุ่มผ่าน หรือไม่เคยถูกเก็บเลย
                gameObject.SetActive(true);
                Debug.Log($"Mushroom {uniqueID} has RESPWNED RANDOMLY on Day {currentDay}.");
            }
            else
            {
                // 5. ถ้ายังไม่ถึงเวลาเกิดใหม่ (FALSE) ให้ซ่อนไว้
                gameObject.SetActive(false);
            }
        }
    }
}