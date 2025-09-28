using UnityEngine;

public class MineableRock : MonoBehaviour
{
    // 🚩 ต้องผูก ID ใน Inspector ให้ไม่ซ้ำกัน
    public string uniqueID; 
    
    public int hitPoints = 3; // ขุดได้ 3 ครั้ง
    public GameObject dropPrefab; // Prefab เศษหิน
    public Transform dropPoint; // จุด spawn
    public float respawnDelay = 7200f; // ✅ เพิ่ม: เวลาเกิดใหม่เป็นวินาที (เช่น 2 ชั่วโมง)
    
    private int startingHitPoints;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        startingHitPoints = hitPoints; // บันทึก HP เริ่มต้น

        // 🚩 เปลี่ยน: ใช้ CheckRespawnStateOnLoad แทนการตรวจสอบ IsRemoved แบบถาวร
        CheckRespawnStateOnLoad();
    }
    
    // 🚩 เพิ่ม: เมธอดสำหรับตรวจสอบสถานะการเกิดใหม่
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
            // 🚩 เปลี่ยน: บันทึกเวลาที่ถูกทำลายลงใน ObjectSaveManager แทน MarkObjectDestroyed
            if (ObjectSaveManager.instance != null)
            {
                ObjectSaveManager.instance.AddRespawnTimestamp(uniqueID);
            }
            
            // 1. ดรอปไอเทม
            if (dropPrefab != null && dropPoint != null)
            {
                Instantiate(dropPrefab, dropPoint.position, Quaternion.identity);
            }
            
            // 2. ซ่อนหิน แทนการ Destroy ถาวร
            gameObject.SetActive(false);
            
            // 🚩 ลบ: Destroy(gameObject) ทิ้ง
            // 🚩 ลบ: ObjectSaveManager.instance.MarkObjectDestroyed(uniqueID); ทิ้ง
        }
    }
}