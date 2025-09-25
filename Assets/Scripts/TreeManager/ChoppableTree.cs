// ไฟล์: ChoppableTree.cs
using UnityEngine;

public class ChoppableTree : MonoBehaviour
{
    public int hitPoints = 3; 
    public GameObject logPrefab; 
    public Transform dropPoint;
    public string uniqueID; 
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();

        // ไม่ต้องตรวจสอบ IsRemoved() ใน Start() อีกต่อไป เพราะ TreeRespawnManager จะทำหน้าที่สร้างต้นไม้ขึ้นมาใหม่เอง
    }

    public void Chop()
    {
        hitPoints--;

        if (anim != null)
        {
            anim.SetTrigger("Chop");
        }

        if (hitPoints <= 0)
        {
            // บันทึกเวลาที่ต้นไม้ถูกทำลายใน ObjectSaveManager เพื่อให้เกิดใหม่ได้
            if (ObjectSaveManager.instance != null)
            {
                ObjectSaveManager.instance.AddRespawnTimestamp(uniqueID);
            }
            
            // อัปเดตความคืบหน้าเควส
            if (QuestManager.Instance != null)
            {
                QuestManager.Instance.UpdateQuestProgress("Wood"); 
            }

            // สร้างขอนไม้
            if (logPrefab != null && dropPoint != null)
            {
                Instantiate(logPrefab, dropPoint.position, Quaternion.identity);
                
            }

            // ทำลาย GameObject ของต้นไม้
            Destroy(gameObject);
            
        }
    }
}