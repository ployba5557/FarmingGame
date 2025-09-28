// // ไฟล์: TreeRespawnManager.cs
// using UnityEngine;

// public class TreeRespawnManager : MonoBehaviour
// {
//     public GameObject choppableTreePrefab; 
//     public Transform[] respawnPoints;
//     public float respawnDelay = 7200f; // 2 ชั่วโมง
//     // 🚩 หมายเหตุ: สคริปต์นี้เหมาะสำหรับจุดเกิดใหม่ 'ว่างเปล่า' 
//     // ถ้าคุณมีต้นไม้ที่วางไว้ล่วงหน้า ให้ใช้ CheckRespawnStateOnLoad ใน ChoppableTree.cs แทน

//     void Update()
//     {
//         // ... (โค้ดเดิมของคุณ)
//         foreach (Transform point in respawnPoints)
//         {
//             if (point == null)
//             {
//                 continue; 
//             }
            
//             string uniqueID = point.position.ToString();

//             // ตรวจสอบว่าถึงเวลาที่ต้นไม้ควรจะเกิดใหม่หรือยัง
//             if (ObjectSaveManager.instance != null && ObjectSaveManager.instance.ShouldRespawn(uniqueID, respawnDelay))
//             {
//                 // สร้างต้นไม้ขึ้นมาใหม่
//                 GameObject newTree = Instantiate(choppableTreePrefab, point.position, Quaternion.identity);
//                 ChoppableTree treeScript = newTree.GetComponent<ChoppableTree>();
//                 if (treeScript != null)
//                 {
//                     treeScript.uniqueID = uniqueID; 
//                 }
                
//                 Debug.Log($"Tree at {uniqueID} has respawned.");
//             }
//         }
//     }
// }