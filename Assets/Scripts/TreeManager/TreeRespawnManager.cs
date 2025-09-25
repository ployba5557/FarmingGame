// ไฟล์: TreeRespawnManager.cs
using UnityEngine;

public class TreeRespawnManager : MonoBehaviour
{
    public GameObject choppableTreePrefab; 
    public Transform[] respawnPoints;
    public float respawnDelay = 30f; 

    void Update()
    {
        // ✅ เพิ่มการตรวจสอบว่า Transform ยังมีอยู่หรือไม่
        foreach (Transform point in respawnPoints)
        {
            if (point == null)
            {
                continue; // ข้าม Transform ที่ถูกทำลายแล้ว
            }
            
            // ใช้ตำแหน่งของจุดเกิดใหม่เป็น ID เฉพาะ
            string uniqueID = point.position.ToString();

            // ตรวจสอบว่าถึงเวลาที่ต้นไม้ควรจะเกิดใหม่หรือยัง
            if (ObjectSaveManager.instance != null && ObjectSaveManager.instance.ShouldRespawn(uniqueID, respawnDelay))
            {
                // สร้างต้นไม้ขึ้นมาใหม่
                GameObject newTree = Instantiate(choppableTreePrefab, point.position, Quaternion.identity);
                ChoppableTree treeScript = newTree.GetComponent<ChoppableTree>();
                if (treeScript != null)
                {
                    treeScript.uniqueID = uniqueID;
                }
                
                Debug.Log($"Tree at {uniqueID} has respawned.");
            }
        }
    }
}