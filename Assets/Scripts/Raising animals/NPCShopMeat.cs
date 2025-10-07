using UnityEngine;
using UnityEngine.InputSystem;

// 🚩 เปลี่ยนชื่อคลาสเป็น NPCShopMeat
public class NPCShopMeat : MonoBehaviour
{
    [Header("Shop References")]
    // 🚩 เปลี่ยนตัวแปรอ้างอิงเป็น Manager ของร้าน Meat
    public ShopMeatControllerManager shopManagerMeat;
    
    // 🚩 เปลี่ยนชื่อ NPC ให้เหมาะสมกับร้านเนื้อ
    public string[] dialogue; 
    public string npcName = "Butcher Max"; // กำหนดชื่อ NPC
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
        ShopMeatControllerManager manager = ShopMeatControllerManager.instance; 

        // ตรวจสอบ manager.instance
        if (manager != null && manager.theShopMeatController != null)
        {
            // 🎯 เปิดร้านเนื้อสัตว์
            manager.theShopMeatController.OpenClose();
        }
        else
        {
            Debug.LogError("ShopMeatControllerManager.instance is NULL or theShopMeatController is missing. Please check your setup.");
        }
    }
}