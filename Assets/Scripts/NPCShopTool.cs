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
        // Unsubscribe ทันที
    DialogueManagerShop.instance.OnYesSelected -= OnYesSelected;

    // 🚩 แก้ไข: ใช้ Singleton Instance
    ShopToolControllerManager manager = ShopToolControllerManager.instance; 

    // ตรวจสอบ manager.instance แทน shopManagerTool
    if (manager != null && manager.theShopToolController != null)
    {
        manager.theShopToolController.OpenClose();
    }
    else
    {
        // Error จะบอกให้รู้ว่า Manager ตัวหลักหายไป
        Debug.LogError("ShopToolControllerManager.instance is NULL after loading the new day/scene.");
    }
    }
}