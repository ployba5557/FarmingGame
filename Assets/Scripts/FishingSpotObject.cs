using UnityEngine;

public class FishingSpotObject : MonoBehaviour
{
    public GameObject dropFishPrefab;         // ปลา Prefab ที่จะ spawn
    public Transform dropPoint;               // จุด spawn
    public string uniqueID;                   // สำหรับ save/load
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
        if (anim != null)
        {
            anim.SetTrigger("useFishing");
        }

        float waitTime = Random.Range(1f, 10f); // สุ่มความยากของปลา
        Invoke("FinishFishing", waitTime);     // เรียกเมธอด FinishFishing หลังเวลาที่สุ่มได้
    }

    void FinishFishing()
    {
        gameObject.SetActive(false); // ปิดปลาชั่วคราว

        if (dropFishPrefab != null && dropPoint != null)
        {
            Instantiate(dropFishPrefab, dropPoint.position, Quaternion.identity);

              if (QuestManager.Instance != null)
            {
                // ✅ แก้ไขตรงนี้ให้ส่งชื่อ "Fish" ไปโดยตรง
                QuestManager.Instance.UpdateQuestProgress("Fish");
            }
        }
            

        Destroy(gameObject); // ← อันนี้ "ให้เปิดไว้" ถ้าอยากให้หายหลังตกปลา
    }


}
