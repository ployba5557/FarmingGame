using UnityEngine;
using UnityEngine.InputSystem;

public class NPCShop : MonoBehaviour
{
    [Header("Dialogue and Shop")]
    public string[] dialogue;
    public string npcName = "Vegetable Seller"; // ✅ เพิ่มตัวนี้
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

        // แก้ไขตรงนี้ให้เรียก Shop Controller จาก ShopControllerManager
        if (ShopControllerManager.instance.theShopController != null)
        {
            ShopControllerManager.instance.theShopController.OpenClose();
        }
    }
}
