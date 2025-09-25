using UnityEngine;
using UnityEngine.InputSystem;

public class NPCShopTool : MonoBehaviour
{
    [Header("Dialogue and Shop")]
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
            // Unsubscribe จาก Event ก่อนหน้าเพื่อป้องกันการทำงานซ้ำ
            DialogueManagerShop.instance.OnYesSelected -= OnYesSelected;
            // Subscribe เพื่อรับการแจ้งเตือนเมื่อผู้เล่นกด Yes
            DialogueManagerShop.instance.OnYesSelected += OnYesSelected;

            // เริ่มบทสนทนาและส่งข้อมูล NPC
            DialogueManagerShop.instance.StartDialogue(dialogue, npcName, npcSprite);
        }
    }

    private void OnYesSelected()
    {
        // Unsubscribe ทันทีหลังจากที่ผู้เล่นกด Yes
        DialogueManagerShop.instance.OnYesSelected -= OnYesSelected;

        // เรียกตัวจัดการร้านเครื่องมือเพื่อเปิดหน้าต่างโดยตรง
        if (UIController.instance != null && UIController.instance.theShopTool != null)
        {
            UIController.instance.theShopTool.OpenClose();
        }
    }
}