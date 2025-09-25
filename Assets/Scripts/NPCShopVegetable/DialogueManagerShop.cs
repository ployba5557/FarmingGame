using UnityEngine;
using TMPro;
using System.Collections;
using System;
using UnityEngine.UI;

public class DialogueManagerShop : MonoBehaviour
{
    public static DialogueManagerShop instance;

    [Header("UI References")]
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public GameObject choicePanel;
    public Button yesButton;
    public Button noButton;
    public Image npcImage; // ✅ เพิ่มตัวนี้
    public TMP_Text npcName; // ✅ เพิ่มตัวนี้


    [Header("Dialogue Logic")]
    private string[] dialogueLines;
    private int currentLineIndex;
    public float typingSpeed = 0.05f;

    public event Action OnYesSelected;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        yesButton.onClick.AddListener(OnYesButtonClick);
        noButton.onClick.AddListener(OnNoButtonClick);
    }

    public void StartDialogue(string[] lines, string npcNameString, Sprite npcSprite)
    {
        dialogueLines = lines;
        currentLineIndex = 0;
        dialoguePanel.SetActive(true);
        choicePanel.SetActive(false);

        // ✅ อัปเดต UI ด้วยชื่อและรูปภาพของ NPC ที่ส่งเข้ามา
        npcName.text = npcNameString;
        npcImage.sprite = npcSprite;

        StartCoroutine(TypeDialogue());
    }

    IEnumerator TypeDialogue()
    {
        dialogueText.text = "";
        foreach (char letter in dialogueLines[currentLineIndex].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        // เมื่อพิมพ์จบ
        if (currentLineIndex < dialogueLines.Length - 1)
        {
            // ถ้ายังไม่ถึงบรรทัดสุดท้าย ให้รอและพิมพ์บรรทัดถัดไป
            yield return new WaitForSeconds(1.0f); // เพิ่มหน่วงเวลาเล็กน้อยก่อนไปบรรทัดถัดไป
            currentLineIndex++;
            StartCoroutine(TypeDialogue());
        }
        else
        {
            // ถ้าเป็นบรรทัดสุดท้าย ให้แสดงปุ่ม Yes/No
            choicePanel.SetActive(true);
        }
    }

    public void OnYesButtonClick()
    {
        EndDialogue();
        OnYesSelected?.Invoke();
    }

    public void OnNoButtonClick()
    {
        EndDialogue();
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        choicePanel.SetActive(false);
        dialogueLines = null;
        currentLineIndex = 0;
    }
}
