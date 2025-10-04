using UnityEngine;

public class GameStartController : MonoBehaviour
{
    public GameObject malePlayerPrefab;
    public GameObject femalePlayerPrefab;
    public Vector3 spawnPosition = Vector3.zero;

    void Start()
    {
        if (CharacterSelectManager.Instance != null)
        {
            // ตรวจสอบว่าผู้เล่นเลือกใคร
            bool isMale = CharacterSelectManager.Instance.isMale;

            if (isMale)
            {
                Instantiate(malePlayerPrefab, spawnPosition, Quaternion.identity);
            }
            else
            {
                Instantiate(femalePlayerPrefab, spawnPosition, Quaternion.identity);
            }
            
            // 💡 สำคัญ: ลบ Manager ทิ้งเมื่อใช้ข้อมูลเสร็จแล้ว (ถ้าไม่จำเป็นต้องใช้ในเกมอีก)
            // Destroy(CharacterSelectManager.Instance.gameObject); 
        }
        else
        {
            Debug.LogError("Error: CharacterSelectManager not found. Did you start the game from the Character Select Scene?");
        }
    }
}