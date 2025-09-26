using UnityEngine;
using UnityEngine.InputSystem;

// 🚩 เปลี่ยนชื่อคลาสจาก NPCShopFish เป็น NPCShopMushroom
public class NPCShopMushroom : MonoBehaviour
{
    [Header("Shop References")]
    // ตัวแปรนี้จะถูกผูกโดยตรงใน Inspector
    public ShopControllerManagerMushroom shopManagerMushroom;
    // 🚩 เปลี่ยนชื่อ NPC ให้เหมาะสมกับร้านเห็ด
    
    public string[] dialogue; 
    public string npcName = "Mushroom Seller"; 
    public Sprite npcSprite; 
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

            // ส่งชื่อและรูปภาพของ NPC นี้ไปให้ DialogueManagerShop
            DialogueManagerShop.instance.StartDialogue(dialogue, npcName, npcSprite);
        }
    }

    private void OnYesSelected()
    {
        DialogueManagerShop.instance.OnYesSelected -= OnYesSelected;

        // 🚩 แก้ไข: ใช้ตัวแปร shopManagerMushroom ที่ผูกใน Inspector
    if (shopManagerMushroom != null && shopManagerMushroom.theShopMushroomController != null)
    {
      shopManagerMushroom.theShopMushroomController.OpenClose();
    }
    else
    {
           // แสดง Error ที่ละเอียดขึ้น
      Debug.LogError("ShopControllerManagerMushroom is NULL. Check if the NPC's 'Shop Manager Mushroom' field is assigned in the Inspector.");
    }
    }
}
