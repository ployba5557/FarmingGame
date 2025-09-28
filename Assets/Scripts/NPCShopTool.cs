using UnityEngine;
using UnityEngine.InputSystem;

// 🚩 เปลี่ยนชื่อคลาสเป็น NPCShopTool
public class NPCShopTool : MonoBehaviour
{
    [Header("Shop References")]
    // 🚩 เปลี่ยนตัวแปรอ้างอิงเป็น Manager ของร้าน Tool
    // ตัวแปรนี้จะถูกผูกโดยตรงใน Inspector
    public ShopToolControllerManager shopManagerTool;
    
    // 🚩 เปลี่ยนชื่อ NPC ให้เหมาะสมกับร้านหิน/ไม้
    public string[] dialogue; 
    public string npcName = "Tool Seller"; // กำหนดชื่อ NPC
    public Sprite npcSprite; // กำหนดรูป NPC

    private bool playerClose;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerClose = true;
        }
    }

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
            // Unsubscribe ก่อน Subscribe เพื่อป้องกันการทำงานซ้ำ
            DialogueManagerShop.instance.OnYesSelected -= OnYesSelected;
            DialogueManagerShop.instance.OnYesSelected += OnYesSelected;

            // เริ่มบทสนทนาและส่งข้อมูล NPC
            DialogueManagerShop.instance.StartDialogue(dialogue, npcName, npcSprite);
        }
    }

    private void OnYesSelected()
    {
        // Unsubscribe ทันทีหลังจากที่ผู้เล่นกด Yes
        DialogueManagerShop.instance.OnYesSelected -= OnYesSelected;

        // 🚩 แก้ไข: ใช้ตัวแปร shopManagerTool ที่ผูกใน Inspector
        if (shopManagerTool != null && shopManagerTool.theShopToolController != null)
        {
            // เรียก OpenClose ผ่าน Controller หลัก
            shopManagerTool.theShopToolController.OpenClose();
        }
        else
        {
            // แสดง Error ที่ละเอียดขึ้นเพื่อช่วยในการดีบั๊ก
            Debug.LogError("ShopToolControllerManager is NULL. Check if the NPC's 'Shop Manager Tool' field is assigned in the Inspector.");
        }
    }
}