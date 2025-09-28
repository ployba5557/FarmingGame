using UnityEngine;

public class GatherableMushroom : MonoBehaviour
{
    // 🚩 ต้องผูก ID ใน Inspector ให้ไม่ซ้ำกันสำหรับเห็ดแต่ละต้น
    public string uniqueID; 
    
    public string itemID = "Mushroom"; // ID ไอเท็มเห็ด
    public int itemAmount = 1;
    public int initialSpawnOffsetDay = 1; 
    //public float respawnDelay = 10800f; // เวลาเกิดใหม่เป็นวินาที (3 ชม.)

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
        }

        // 2. บันทึกวันที่ถูกเก็บลงใน ObjectSaveManager (✅ แก้ไข)
        if (ObjectSaveManager.instance != null)
        {
            ObjectSaveManager.instance.AddMushroomCollectedDay(uniqueID);
        }

        // 3. ซ่อนเห็ด
        gameObject.SetActive(false);
    }
    
    // 🚩 ✅ เพิ่ม: เมธอดนี้จะถูกเรียกโดย DayEndController
    // เพื่อ "เก็บ" เห็ดที่ผู้เล่นพลาดไปโดยไม่ให้ไอเทม
    public void MarkAsMissed()
    {
        // 1. บันทึกวันที่ถูกเก็บลงใน ObjectSaveManager
        if (ObjectSaveManager.instance != null)
        {
            // บันทึกวันที่ถูก "พลาด" ไป เพื่อเริ่มการนับเวลาเกิดใหม่แบบสุ่ม
            ObjectSaveManager.instance.AddMushroomCollectedDay(uniqueID);
            Debug.Log($"Mushroom {uniqueID} was MISSED and marked for randomized respawn.");
        }
        
        // 2. ซ่อนเห็ด
        gameObject.SetActive(false); 
        
        // ไม่ต้องเพิ่มไอเทมเข้า Inventory
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
