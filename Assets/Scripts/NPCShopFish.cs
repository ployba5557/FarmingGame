using UnityEngine;
using UnityEngine.InputSystem;

public class NPCShopFish : MonoBehaviour
{
    [Header("Dialogue and Shop")]
    public string[] dialogue;
    public string npcName = "Fisher Seller"; // ✅ เพิ่มตัวนี้
    public Sprite npcSprite; // ✅ เพิ่มตัวนี้
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
            DialogueManagerShop.instance.OnYesSelected -= OnYesSelected;
            DialogueManagerShop.instance.OnYesSelected += OnYesSelected;

            // ✅ ส่งชื่อและรูปภาพของ NPC นี้ไปให้ DialogueManagerShop
            DialogueManagerShop.instance.StartDialogue(dialogue, npcName, npcSprite);
        }
    }

    private void OnYesSelected()
    {
        DialogueManagerShop.instance.OnYesSelected -= OnYesSelected;

        // บรรทัดนี้สำคัญมาก! ให้ NPC ขายปลาไปเรียกตัวจัดการร้านปลา
        if (ShopManageFish.instance.theShopFishController != null)
        {
            ShopManageFish.instance.theShopFishController.OpenClose();
        }
    }
}