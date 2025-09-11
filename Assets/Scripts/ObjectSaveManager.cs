using System.Collections.Generic;
using UnityEngine;

public class ObjectSaveManager : MonoBehaviour
{
    public static ObjectSaveManager instance;

    // ❌ หายถาวร
    private HashSet<string> removedObjects = new HashSet<string>();

    // ✅ หายเฉพาะวันนั้น (ตกปลา)
    private HashSet<string> dailyObjects = new HashSet<string>();
    private Dictionary<string, GameObject> dailyObjectMap = new Dictionary<string, GameObject>();

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

    public void RegisterDailyObject(string id, GameObject obj)
    {
        if (!dailyObjects.Contains(id))
        {
            dailyObjects.Add(id);
            dailyObjectMap[id] = obj;
        }
    }


    // ✅ สำหรับหายถาวร
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

    public bool IsRemoved(string id)
    {
        return removedObjects.Contains(id);
    }

    // ✅ เรียกตอนเช้าวันใหม่
    public void ResetDailyObjects()
    {
        foreach (string id in dailyObjects)
        {
            if (dailyObjectMap.ContainsKey(id))
            {
                GameObject obj = dailyObjectMap[id];
                if (obj != null)
                {
                    obj.SetActive(true); // เปิดใหม่
                }
            }
        }
    }
}
