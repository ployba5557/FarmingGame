using UnityEngine;

public class MineableRock : MonoBehaviour
{
    // 🚩 ต้องผูก ID ใน Inspector ให้ไม่ซ้ำกัน
    public string uniqueID; 
    
    public int hitPoints = 3; // ขุดได้ 3 ครั้ง
    public GameObject dropPrefab; // Prefab เศษหิน
    public Transform dropPoint; // จุด spawn
    public float respawnDelay = 7200f; // เวลาเกิดใหม่เป็นวินาที
    
    private int startingHitPoints;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        startingHitPoints = hitPoints; // บันทึก HP เริ่มต้น

        CheckRespawnStateOnLoad();
    }
    
    // 🚩 เมธอดสำหรับตรวจสอบสถานะการเกิดใหม่
    private void CheckRespawnStateOnLoad()
    {
        if (ObjectSaveManager.instance != null)
        {
            // ตรวจสอบว่าหินควรเกิดใหม่หรือไม่ (ใช้ logic ตามเวลา)
            if (ObjectSaveManager.instance.ShouldRespawn(uniqueID, respawnDelay))
            {
                // ถ้าถึงเวลาเกิดใหม่แล้ว หรือไม่เคยถูกเก็บมาก่อน
                gameObject.SetActive(true);
                hitPoints = startingHitPoints; // รีเซ็ต Hit Points 
                Debug.Log($"Rock {uniqueID} has RESPWNED.");
            }
            else
            {
                // ถ้ายังไม่ถึงเวลาเกิดใหม่ (ซ่อนไว้)
                gameObject.SetActive(false);
            }
        }
    }


    public void Mine()
    {
        hitPoints--;

        if (anim != null)
        {
            anim.SetTrigger("Mine");
        }

        if (hitPoints <= 0)
        {
            // 1. ดรอปไอเทม
            if (dropPrefab != null && dropPoint != null)
            {
                Instantiate(dropPrefab, dropPoint.position, Quaternion.identity);
            }
            
            // 2. อัปเดตความคืบหน้าของเควส! ✅ โค้ดที่เพิ่มเข้ามา
            if (QuestManager.Instance != null)
            {
                // เรียก QuestManager เพื่อแจ้งว่ามีการเก็บ 'Stone' (หิน) แล้ว 1 หน่วย
                QuestManager.Instance.UpdateQuestProgress("Stone");
            }

            // 3. บันทึกเวลาที่ถูกทำลายลงใน ObjectSaveManager
            if (ObjectSaveManager.instance != null)
            {
                ObjectSaveManager.instance.AddRespawnTimestamp(uniqueID);
            }
            
            // 4. ซ่อนหิน 
            gameObject.SetActive(false);
        }
    }
}