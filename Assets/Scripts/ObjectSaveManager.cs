// ไฟล์: ObjectSaveManager.cs
using System.Collections.Generic;
using UnityEngine;

public class ObjectSaveManager : MonoBehaviour
{
    public static ObjectSaveManager instance;

    // สำหรับวัตถุที่หายถาวร
    private HashSet<string> removedObjects = new HashSet<string>();

    // สำหรับวัตถุที่หายเฉพาะวันนั้น
    private HashSet<string> dailyObjects = new HashSet<string>();
    private Dictionary<string, GameObject> dailyObjectMap = new Dictionary<string, GameObject>();

    // ✅ สำหรับวัตถุที่สามารถเกิดใหม่ได้ (เช่น ต้นไม้)
    private Dictionary<string, float> respawnableTimestamps = new Dictionary<string, float>();

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // สำหรับวัตถุที่หายถาวร
    public void MarkObjectDestroyed(string id)
    {
        if (!removedObjects.Contains(id))
        {
            removedObjects.Add(id);
        }
    }

    public bool IsObjectDestroyed(string id)
    {
        return removedObjects.Contains(id);
    }

    // ฟังก์ชันนี้ถูกใช้ใน ChoppableTree.cs เดิม แต่เราจะเปลี่ยนไปใช้ ShouldRespawn() แทน
    public bool IsRemoved(string id)
    {
        return removedObjects.Contains(id);
    }

    // ✅ ฟังก์ชันใหม่: เพิ่มเวลาที่ต้นไม้ถูกทำลาย
    public void AddRespawnTimestamp(string id)
    {
        respawnableTimestamps[id] = Time.time;
    }
    
    // ✅ ฟังก์ชันใหม่: ตรวจสอบว่าถึงเวลาเกิดใหม่หรือยัง
    public bool ShouldRespawn(string id, float respawnDelay)
    {
        if (respawnableTimestamps.ContainsKey(id))
        {
            if (Time.time - respawnableTimestamps[id] >= respawnDelay)
            {
                respawnableTimestamps.Remove(id); // ลบข้อมูลเก่าเมื่อเกิดใหม่แล้ว
                return true;
            }
        }
        return false;
    }
    
    // สำหรับวัตถุรายวัน
    public void RegisterDailyObject(string id, GameObject obj)
    {
        if (!dailyObjects.Contains(id))
        {
            dailyObjects.Add(id);
            dailyObjectMap[id] = obj;
        }
    }
    
    public void ResetDailyObjects()
    {
        foreach (string id in dailyObjects)
        {
            if (dailyObjectMap.ContainsKey(id))
            {
                GameObject obj = dailyObjectMap[id];
                if (obj != null)
                {
                    obj.SetActive(true);
                }
            }
        }
    }
}