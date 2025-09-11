using UnityEngine;
using UnityEngine.InputSystem;

public class NPCShop : MonoBehaviour
{
    [Header("Dialogue and Shop")]
    public string[] dialogue;

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
            // เปลี่ยน DialogueManager.instance เป็น DialogueManagerShop.instance
            DialogueManagerShop.instance.OnYesSelected -= OnYesSelected;
            DialogueManagerShop.instance.OnYesSelected += OnYesSelected;

            DialogueManagerShop.instance.StartDialogue(dialogue);
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
