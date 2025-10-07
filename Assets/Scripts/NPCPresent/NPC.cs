using UnityEngine;

public class NPC : MonoBehaviour
{
    public string[] dialogue;
    public bool playerClose;

    // ส่วนนี้จะเหมือนเดิม
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

    // แก้ไขฟังก์ชัน Update
    void Update()
    {
        if (playerClose && Input.GetKeyDown(KeyCode.E))
        {
            // เรียกใช้ DialogueManager
            // ส่งอาร์เรย์ dialogue ของ NPC ตัวนี้เข้าไป
            DialogueManager.instance.StartDialogue(dialogue);
        }
    }
}