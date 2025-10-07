using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq; // สำหรับ Dictionary/HashSet serialization

public class ObjectSaveManager : MonoBehaviour
{
    public static ObjectSaveManager instance;

    // --- 1. ระบบ Respawn ตามเวลา (หิน/ไม้) ---
    public Dictionary<string, double> respawnableTimestamps = new Dictionary<string, double>();

    // --- 2. ระบบ Respawn แบบสุ่มตามวัน (เห็ด) ---
    // Key: uniqueID, Value: วันที่ (TimeController.instance.currentDay) ที่ถูกเก็บล่าสุด
    public Dictionary<string, int> mushroomCollectedDay = new Dictionary<string, int>(); 
    
     public Dictionary<string, int> animalCollectedDay = new Dictionary<string, int>();

    // --- 3. ระบบหายถาวร ---
    private HashSet<string> removedObjects = new HashSet<string>(); 

    // --- 4. ระบบเกิดใหม่รายวัน ---
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

    // =========================================================================
    //                            A. Time-Based Respawn (หิน/ไม้)
    // =========================================================================
    // ... (เมธอด AddRespawnTimestamp และ ShouldRespawn เดิม) ...

    // บันทึกเวลาปัจจุบันที่ไอเทมถูกเก็บ
    public void AddRespawnTimestamp(string id)
    {
        double currentTime = (DateTime.Now - new DateTime(1970, 1, 1)).TotalSeconds;
        
        if (respawnableTimestamps.ContainsKey(id))
        {
            respawnableTimestamps[id] = currentTime;
        }
        else
        {
            respawnableTimestamps.Add(id, currentTime);
        }
    }

    // ตรวจสอบว่าถึงเวลาเกิดใหม่หรือยัง
    public bool ShouldRespawn(string id, float respawnDelay)
    {
        if (!respawnableTimestamps.ContainsKey(id))
        {
            return true;
        }

        double lastCollectedTime = respawnableTimestamps[id];
        double respawnTime = lastCollectedTime + respawnDelay; 
        double currentTime = (DateTime.Now - new DateTime(1970, 1, 1)).TotalSeconds;
        
        if (currentTime >= respawnTime)
        {
            respawnableTimestamps.Remove(id);
            return true;
        }

        return false;
    }


    // =========================================================================
    //                            B. Daily Random Respawn (เห็ด)
    // =========================================================================
    
    // บันทึกวันที่เห็ดถูกเก็บ
    public void AddMushroomCollectedDay(string id)
    {
        // 🚩 แก้ไข: ใช้ TimeController แทน DayManager
        if (TimeController.instance != null) 
        {
            int currentDay = TimeController.instance.currentDay;
            if (mushroomCollectedDay.ContainsKey(id))
            {
                mushroomCollectedDay[id] = currentDay;
            }
            else
            {
                mushroomCollectedDay.Add(id, currentDay);
            }
        }
    }

    // ตรวจสอบว่าเห็ดควรเกิดใหม่แบบสุ่มหรือไม่ (โอกาสเพิ่มขึ้นตามวัน)
    public bool ShouldMushroomRespawn(string id)
    {
        if (!mushroomCollectedDay.ContainsKey(id))
        {
            return true;
        }

        // 🚩 แก้ไข: ใช้ TimeController แทน DayManager
        if (TimeController.instance == null) return false;

        int currentDay = TimeController.instance.currentDay;
        int dayCollected = mushroomCollectedDay[id];

        int daysPassed = currentDay - dayCollected;

        int minDays = 3;
        int maxDays = 5;

        if (daysPassed >= minDays)
        {
            float probability = (float)daysPassed / maxDays;

            if (UnityEngine.Random.value < probability)
            {
                mushroomCollectedDay.Remove(id);
                return true;
            }
        }

        return false;
    }

    // =========================================================================
    //                            E. Daily Respawn (เนื้อสัตว์)
    // =========================================================================

    // 🎯 บันทึกวันที่สัตว์ถูกเก็บ
    public void AddAnimalCollectedDay(string id)
    {
        if (TimeController.instance != null)
        {
            int currentDay = TimeController.instance.currentDay;
            if (animalCollectedDay.ContainsKey(id))
            {
                animalCollectedDay[id] = currentDay;
            }
            else
            {
                animalCollectedDay.Add(id, currentDay);
            }
        }
    }
    
    // 🎯 ตรวจสอบว่าสัตว์ควรเกิดใหม่แบบสุ่มหรือไม่ (ต้องใช้ minRespawnDays จาก GatherableAnimal)
    // 💡 เมธอดนี้ถูกออกแบบมาเพื่อรองรับ logic ที่อยู่ใน GatherableAnimal.cs ที่ผมให้ไปก่อนหน้านี้
    public bool ShouldAnimalRespawn(string id, int minDaysRequired)
    {
        if (!animalCollectedDay.ContainsKey(id))
        {
            return true;
        }

        if (TimeController.instance == null) return false;
        
        int currentDay = TimeController.instance.currentDay;
        int dayCollected = animalCollectedDay[id];

        int daysPassed = currentDay - dayCollected;
        
        // 1. ตรวจสอบว่าถึงวันเกิดใหม่ขั้นต่ำหรือยัง
        if (daysPassed >= minDaysRequired)
        {
            // 2. ใช้โอกาสสุ่มเกิดใหม่ (โอกาส 50% ต่อวันหลังจากวันขั้นต่ำ)
            // คุณอาจปรับให้โอกาสเพิ่มขึ้นตาม daysPassed ก็ได้
            float probability = 0.5f; // โอกาส 50% ในการเกิดใหม่เมื่อถึงวันขั้นต่ำ
            
            if (UnityEngine.Random.value < probability)
            {
                animalCollectedDay.Remove(id);
                return true;
            }
        }

        return false;
    }


    // =========================================================================
    //                            C. Permanent Removal / D. Daily Reset (เดิม)
    // =========================================================================

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
    
    public void RegisterDailyObject(string id, GameObject obj)
    {
        if (!dailyObjects.Contains(id))
        {
            dailyObjects.Add(id);
            dailyObjectMap[id] = obj;
        }
    }

    // เรียกตอนเช้าวันใหม่ (จาก DayManager)
    public void ResetDailyObjects()
    {
        // ... (Logic ResetDailyObjects เดิม) ...
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