using UnityEngine;

public class FishingSpotObject : MonoBehaviour
{
    public GameObject dropFishPrefab;         // ปลา Prefab ที่จะ spawn
    public Transform dropPoint;               // จุด spawn
    public string uniqueID;                   // สำหรับ save/load
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();

        if (ObjectSaveManager.instance != null)
        {
            ObjectSaveManager.instance.RegisterDailyObject(uniqueID, gameObject);
        }
    }

    public void Fish() 
    {
        Debug.Log("Fishing at: " + uniqueID);

        if (anim != null)
        {
            anim.SetTrigger("useFishing");
        }
        else
        {
            Debug.LogWarning("No Animator on: " + uniqueID);
        }
        AudioManager.Instance.PlaySFXPitchAdjusted(7);
        float waitTime = Random.Range(1f, 10f);
        Debug.Log("WaitTime: " + waitTime);
        Invoke("FinishFishing", waitTime);
    }

    void FinishFishing()
    {
        gameObject.SetActive(false); // ปิดจุดตกปลาชั่วคราว

        if (dropFishPrefab != null && dropPoint != null)
        {
            Instantiate(dropFishPrefab, dropPoint.position, Quaternion.identity);

            // 🚩 Logic การอัปเดตเควส ถูกเพิ่มไว้แล้วและถูกต้อง
            if (QuestManager.Instance != null)
            {
                // เรียก QuestManager เพื่ออัปเดตความคืบหน้าของเควส "Fish" (ปลา)
                QuestManager.Instance.UpdateQuestProgress("Fish");
            }
        }
            
        // หากต้องการให้จุดตกปลาหายไปถาวรหลังจากตกเสร็จ
        // หากไม่ต้องการ ให้ใช้การจัดการ Respawn ผ่าน ObjectSaveManager แทน Destroy(gameObject)
        Destroy(gameObject); 
    }
}