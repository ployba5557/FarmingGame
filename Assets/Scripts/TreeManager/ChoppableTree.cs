using UnityEngine;

public class ChoppableTree : MonoBehaviour
{
    // 🚩 ต้องผูก ID ใน Inspector ให้ไม่ซ้ำกัน
    public string uniqueID; 
    
    // ข้อมูลการดรอปของขอนไม้
    public GameObject dropPrefab; // Prefab ของไอเทม (ต้องมี PickupItem.cs)
    public Transform dropPoint;
    
    public int hitPoints = 3; // จำนวนครั้งที่ต้องตัด
    public float respawnDelay = 7200f; // เวลาเกิดใหม่เป็นวินาที (3 ชม. หรือตามต้องการ)
    
    // 🚩 ตัวแปร private สำหรับสถานะเริ่มต้น
    private int startingHitPoints;

    private void Start()
    {
        startingHitPoints = hitPoints; // บันทึก HP เริ่มต้น
        // 1. ตรวจสอบสถานะเมื่อเริ่มเกม/โหลดฉาก
        CheckRespawnStateOnLoad();
    }

    // เมธอดที่เรียกเมื่อผู้เล่นตัด
    public void Chop()
    {
        hitPoints--;
        // ... (ใส่ Logic สำหรับ Animation, เสียง, การใช้พลังงาน) ...

        if (hitPoints <= 0)
        {
            // 1. ดรอปไอเทม
            if (dropPrefab != null && dropPoint != null)
            {
                Instantiate(dropPrefab, dropPoint.position, Quaternion.identity);
            }
            
            // 2. บันทึกเวลาที่ถูกทำลายลงใน ObjectSaveManager (✅ แก้ไข)
            if (ObjectSaveManager.instance != null)
            {
                ObjectSaveManager.instance.AddRespawnTimestamp(uniqueID);
            }
            
            // 3. ซ่อนต้นไม้
            gameObject.SetActive(false); 
        }
    }

    private void CheckRespawnStateOnLoad()
    {
        if (ObjectSaveManager.instance != null)
        {
            // 4. ตรวจสอบว่าต้นไม้ควรเกิดใหม่หรือไม่ (ใช้ logic ตามเวลา)
            if (ObjectSaveManager.instance.ShouldRespawn(uniqueID, respawnDelay))
            {
                // ถ้าถึงเวลาเกิดใหม่แล้ว หรือไม่เคยถูกเก็บมาก่อน
                gameObject.SetActive(true);
                // 🚩 รีเซ็ต Hit Points เพื่อให้ตัดได้ใหม่
                hitPoints = startingHitPoints; 
                Debug.Log($"Tree {uniqueID} has RESPWNED.");
            }
            else
            {
                // 5. ถ้ายังไม่ถึงเวลาเกิดใหม่ (ซ่อนไว้)
                gameObject.SetActive(false);
            }
        }
        else
        {
             // ถ้า ObjectSaveManager ยังไม่พร้อม ให้แสดงต้นไม้ไว้ก่อน
             gameObject.SetActive(true);
        }
    }
}