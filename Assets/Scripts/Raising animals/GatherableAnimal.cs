using UnityEngine;

public class GatherableAnimal : MonoBehaviour
{
    // 1. 🐮 สร้าง Enum สำหรับชนิดของสัตว์
    public enum AnimalType
    {
        Chicken, // เนื้อไก่
        Pig,     // เนื้อหมู
        Cow      // เนื้อวัว
    }

    [Header("Animal Setup")]
    // 🚩 กำหนดชนิดของสัตว์ใน Inspector
    public AnimalType type = AnimalType.Chicken; 

    // 🚩 ข้อมูลระบุตัวตน (ต้องผูกใน Inspector และต้องไม่ซ้ำกัน)
    public string uniqueID; 
    
    [Header("Respawn")]
    // จำนวนวันเริ่มต้นที่สัตว์จะเริ่มปรากฏตัว
    public int initialSpawnOffsetDay = 0; 
    
    // ตัวแปรสำหรับเก็บค่า ItemID และ RespawnDays ที่ได้จาก AnimalType
    private string itemID;
    private int minRespawnDays;
    
    // สามารถกำหนดจำนวนไอเท็มที่ได้รับจากการเก็บเกี่ยวได้
    private int itemAmount = 1; 

    private void Awake()
    {
        // กำหนด ItemID และ RespawnDays ตามชนิดของสัตว์ที่เลือกใน Inspector
        SetAnimalStats(type);
    }

    private void Start()
    {
        // 1. ตรวจสอบสถานะเมื่อเริ่มเกม/วันใหม่
        CheckRespawnStateOnLoad();
    }
    
    // 🎯 เมธอดสำหรับกำหนดคุณสมบัติของสัตว์
    private void SetAnimalStats(AnimalType animalType)
    {
        switch (animalType)
        {
            case AnimalType.Chicken:
                itemID = "Chicken"; // เนื้อไก่
                minRespawnDays = 2;     // ไก่เกิดใหม่เร็วสุด (เช่น 2 วัน)
                itemAmount = 1;
                break;
            case AnimalType.Pig:
                itemID = "Pig";    // เนื้อหมู
                minRespawnDays = 4;     // หมูเกิดใหม่ปานกลาง (เช่น 4 วัน)
                itemAmount = 1;
                break;
            case AnimalType.Cow:
                itemID = "Cow";    // เนื้อวัว
                minRespawnDays = 7;     // วัวเกิดใหม่ช้าสุด (เช่น 7 วัน)
                itemAmount = 1;
                break;
        }
    }

    // 🎯 เมธอดหลักสำหรับผู้เล่นในการเก็บ/ฆ่า/เก็บเกี่ยว
    public void Gather()
    {
        // 1. มอบไอเทมเข้า ItemController
        if (ItemController.instance != null)
        {
            ItemController.instance.AddItem(itemID, itemAmount);
            Debug.Log($"Collected {itemAmount} of {itemID} from animal {uniqueID}.");
            ItemController.instance.UpdateInventoryUI();
        }
        
        // 2. อัปเดตความคืบหน้าของเควส
        if (QuestManager.Instance != null)
        {
            // QuestManager จะได้รับการอัปเดตด้วย itemID ที่ถูกต้อง
            QuestManager.Instance.UpdateQuestProgress(itemID); 
        }

        // 3. บันทึกวันที่ถูกเก็บลงใน ObjectSaveManager
        if (ObjectSaveManager.instance != null)
        {
            // ใช้ AddMushroomCollectedDay หรือเมธอดที่ใช้บันทึกการเก็บเกี่ยว
            ObjectSaveManager.instance.AddMushroomCollectedDay(uniqueID); 
        }

        // 4. ซ่อน/ทำลายสัตว์
        gameObject.SetActive(false);
    }
    
    // 🎯 Logic การเกิดใหม่ (เรียกใน Start())
    private void CheckRespawnStateOnLoad()
    {
        if (ObjectSaveManager.instance != null && TimeController.instance != null)
        {
            int currentDay = TimeController.instance.currentDay;

            if (currentDay < initialSpawnOffsetDay)
            {
                gameObject.SetActive(false);
                return;
            }

            // 4. ตรวจสอบว่าสัตว์ควรเกิดใหม่หรือไม่ (ต้องใช้ minRespawnDays ที่ได้จากการตั้งค่า)
            // 💡 สำคัญ: ต้องใช้เมธอด ShouldRespawn ที่แก้ไขให้รับค่า minRespawnDays
            if (ObjectSaveManager.instance.ShouldRespawn(uniqueID, minRespawnDays)) 
            {
                gameObject.SetActive(true);
                Debug.Log($"Animal {uniqueID} ({type}) has RESPAWNED RANDOMLY on Day {currentDay}.");
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}