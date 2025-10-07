using UnityEngine;
using UnityEngine.InputSystem;

public class NPCShopMushroom : MonoBehaviour
{
  [Header("Shop References")]
    public ShopControllerManagerMushroom shopManagerMushroom;
     public string[] dialogue; 
     public string npcName = "Mushroom Seller"; 
     public Sprite npcSprite; 
     private bool playerClose; // ตัวแปรที่ใช้ตรวจสอบว่าผู้เล่นอยู่ในระยะหรือไม่

    // 🚩 ✅ เพิ่ม: เมธอด Start() เพื่อรีเซ็ตสถานะเมื่อ GameObject ถูกโหลด/รีโหลด
    private void Start()
    {
        // ตรวจสอบให้แน่ใจว่าสถานะเริ่มต้นเป็น false เสมอเมื่อ NPC ถูกสร้างขึ้นมาใหม่ในแต่ละวัน
        playerClose = false; 
        
        // 🚩 ✅ ผูกเมธอด ResetDailyStatus เข้ากับ TimeController (ถ้ามีระบบ Event)
        // (คุณอาจต้องเพิ่ม Logic นี้ใน TimeController เอง)
        // if (TimeController.instance != null)
        // {
        //     TimeController.instance.OnStartDay += ResetDailyStatus;
        // }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerClose = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // บังคับให้เป็น true ตลอดเวลาที่ผู้เล่นอยู่ใน Trigger
            playerClose = true; 
        }
    }

    // ✅ เมธอดเดิมที่ตั้งค่าเมื่อออกจาก Trigger
     private void OnTriggerExit2D(Collider2D other)
     {
         if (other.CompareTag("Player"))
         {
             playerClose = false;
         }
     }

    void Update()
     {
         if (playerClose && Keyboard.current.eKey.wasPressedThisFrame)
         {
             DialogueManagerShop.instance.OnYesSelected -= OnYesSelected;
             DialogueManagerShop.instance.OnYesSelected += OnYesSelected;

             DialogueManagerShop.instance.StartDialogue(dialogue, npcName, npcSprite);
         }
     }

    private void OnYesSelected()
    {
        DialogueManagerShop.instance.OnYesSelected -= OnYesSelected;

        // 🚩 ✅ แก้ไข: ใช้ Singleton Instance แทนการผูกใน Inspector
        ShopControllerManagerMushroom manager = ShopControllerManagerMushroom.instance;

        if (manager != null && manager.theShopMushroomController != null)
        {
            // เรียก OpenClose ผ่าน Controller หลัก
            manager.theShopMushroomController.OpenClose();
        }
        else
        {
            // แสดง Error เมื่อ Manager หลักหายไป
            Debug.LogError("ShopControllerManagerMushroom.instance is NULL. Check if the Manager GameObject is in the scene and its Awake() runs correctly.");
        }
    }
         
    
    
    // 🚩 ✅ เพิ่ม: เมธอดนี้จะเรียกเมื่อเริ่มต้นวันใหม่ (ถ้าคุณผูกกับ Event ใน TimeController)
    public void ResetDailyStatus()
{
    playerClose = false;
    // 🚩 หากบรรทัดนี้ไม่แสดงใน Console เมื่อเข้าสู่ Day 2 แสดงว่า Event ไม่ทำงาน
    Debug.Log($"NPC {npcName}: Status reset. playerClose is now {playerClose} on Day {TimeController.instance.currentDay}."); 
}
}