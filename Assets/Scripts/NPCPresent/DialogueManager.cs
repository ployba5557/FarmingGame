using UnityEngine;
using TMPro;
using System.Collections;
using System; // เพิ่ม using System เพื่อใช้ Action

public class DialogueManager : MonoBehaviour
{
    // ตัวแปรสำหรับสร้าง Singleton (เพื่อเรียกใช้จากที่อื่นได้ง่าย)
    public static DialogueManager instance;

    [Header("UI References")]
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public GameObject continueButton;

    [Header("Dialogue Logic")]
    private string[] dialogueLines;
    private int currentLineIndex;
    public float typingSpeed = 0.05f;

    public Action OnYesSelected { get; internal set; }

    // ส่วนของ Singleton
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // ทำให้ GameObject นี้ไม่ถูกทำลาย
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // ถ้ามี DialogueManager อยู่แล้ว ให้ทำลายตัวเอง
            Destroy(gameObject);
        }
    }

    // เริ่มการสนทนา
    public void StartDialogue(string[] lines)
    {
        dialogueLines = lines;
        currentLineIndex = 0;
        dialoguePanel.SetActive(true);
        continueButton.SetActive(false);
        StartCoroutine(TypeLine());
    }

    // ฟังก์ชันสำหรับพิมพ์ข้อความ
    IEnumerator TypeLine()
    {
        dialogueText.text = "";
        foreach (char letter in dialogueLines[currentLineIndex].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        continueButton.SetActive(true);
    }

    // ฟังก์ชันสำหรับไปบรรทัดถัดไป
    public void NextLine()
    {
        continueButton.SetActive(false);
        if (currentLineIndex < dialogueLines.Length - 1)
        {
            currentLineIndex++;
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
    }

    // จบบทสนทนา
    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        dialogueLines = null;
        currentLineIndex = 0;
    }

    internal void StartDialogue(string[] dialogue, Action openClose)
    {
        throw new NotImplementedException();
    }

    public static implicit operator DialogueManager(DialogueManagerShop v)
    {
        throw new NotImplementedException();
    }
}