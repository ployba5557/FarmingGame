// ไฟล์: MushroomSpawnManager.cs
using UnityEngine;
using System.Collections.Generic;

public class MushroomSpawnManager : MonoBehaviour
{
    public GameObject mushroomPrefab; 
    public List<Transform> potentialSpawnPoints = new List<Transform>(); 
    
    [Header("Spawn Settings")]
    public float spawnInterval = 360f; 
    public float spawnChancePercentage = 20f; 
    
    private float spawnTimer;

    void Start()
    {
        spawnTimer = spawnInterval;
        // 🚩 หมายเหตุ: เห็ดที่เกิดจาก Manager นี้ 
        // จะต้องใช้ uniqueID ที่กำหนดจาก Manager และใช้ระบบ Respawn Time-based ด้วย
    }

    void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0)
        {
            AttemptSpawnMushrooms();
            spawnTimer = spawnInterval; 
        }
    }

    private void AttemptSpawnMushrooms()
    {
        foreach (Transform spawnPoint in potentialSpawnPoints)
        {
            Collider2D existingMushroom = Physics2D.OverlapCircle(spawnPoint.position, 0.1f);
            
            if (existingMushroom == null) 
            {
                float randomValue = Random.Range(0f, 100f);
                
                if (randomValue <= spawnChancePercentage)
                {
                    // 🚩 สำคัญ: ต้องกำหนด uniqueID ให้กับเห็ดที่ Instantiate ด้วย
                    GameObject newMushroom = Instantiate(mushroomPrefab, spawnPoint.position, Quaternion.identity, spawnPoint);
                    GatherableMushroom mushroomScript = newMushroom.GetComponent<GatherableMushroom>();
                    
                    if (mushroomScript != null)
                    {
                        // ใช้ตำแหน่งเป็น ID สำหรับเห็ดที่เกิดแบบสุ่ม
                        mushroomScript.uniqueID = spawnPoint.position.ToString(); 
                        // เมื่อเห็ดถูกเก็บ มันจะใช้ ID นี้ในการบันทึกเวลาเกิดใหม่
                    }
                    
                    Debug.Log("Mushroom spawned successfully at: " + spawnPoint.name);
                }
            }
        }
    }
}