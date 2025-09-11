using UnityEngine;

public class ChoppableTree : MonoBehaviour
{
    public int hitPoints = 3; // ตัดได้ 3 ครั้ง
    public GameObject logPrefab;  // ใส่ Prefab ขอนไม้ใน Inspector
    public Transform dropPoint;   // จุดที่ขอนไม้โผล่มา
    public string uniqueID;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();

        if (ObjectSaveManager.instance != null && ObjectSaveManager.instance.IsRemoved(uniqueID))
        {
            Destroy(gameObject); // ❌ เคยถูกทำลายไปแล้ว ไม่ต้องโผล่
            return;
        }
    }

    public void Chop()
    {
        hitPoints--;

        if (anim != null)
        {
            anim.SetTrigger("Chop"); // ถ้ามี animation ให้แสดง
        }

        if (hitPoints <= 0)
        {
            // ✅ เพิ่มตรงนี้เพื่อบอกว่า "ต้นไม้นี้ถูกทำลายแล้ว"
            ObjectSaveManager.instance.MarkObjectDestroyed(uniqueID);

            //if (ObjectSaveManager.instance != null && ObjectSaveManager.instance.IsRemoved(uniqueID))
            //{
            //    Destroy(gameObject);
            //}

             //✅ สร้างขอนไม้ก่อนทำลาย
            if (logPrefab != null && dropPoint != null)
            {
                Instantiate(logPrefab, dropPoint.position, Quaternion.identity); // สร้างขอนไม้
            }

            // ✅ ลบต้นไม้
            Destroy(gameObject);
        }
    }
}
