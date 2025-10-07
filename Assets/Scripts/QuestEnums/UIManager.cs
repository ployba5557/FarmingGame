using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections; // ต้องเพิ่มบรรทัดนี้สำหรับ Coroutine
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    // === UI Panels and Containers ===
    public GameObject questPanel;
    public GameObject questListParent;
    public GameObject questButtonPrefab;
    
    public GameObject questDetailPanel;

    // === Text Components ===
    public TextMeshProUGUI questListText; // (อาจไม่ได้ใช้ถ้าใช้ Prefab ปุ่ม)
    public TextMeshProUGUI detailQuestNameText;
    public TextMeshProUGUI detailQuestDescriptionText;
    public TextMeshProUGUI questProgressText;

    [Header("Scene Management")]
    public string lobbySceneName = "LobbyScene"; 

      [Header("Confirmation Panel")]
    public GameObject confirmationPanel;
    public Button confirmLobbyButton;
    public Button cancelLobbyButton;

    // ✅ เพิ่มตัวแปรสำหรับปุ่มออก (ถ้ามีปุ่มเฉพาะในฉาก)
    public Button goToLobbyButton;
    
    // === Reward Notification UI ===
    // ✅ เพิ่มตัวแปรสำหรับข้อความแจ้งเตือนรางวัล
    public TextMeshProUGUI rewardNotificationText; 
    public float notificationDisplayTime = 3f;

    // === Buttons ===
    public Button acceptButton;
    public Button cancelButton;
    public Button backButton;
    public Button completeButton;

    // === Internal State ===
    private bool isQuestPanelOpen = false;
    private IQuest _selectedQuest;

    public GameObject questionPanel;

    // === Singleton ===
    public static UIManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // ลบ Listener ที่เคยถูกผูกไว้ใน Inspector หรือโค้ดอื่นๆ ออกทั้งหมด
        if (goToLobbyButton != null)
        {
            goToLobbyButton.onClick.RemoveAllListeners();
            
            // ผูก Listener ที่ถูกต้องเพียงตัวเดียวด้วยโค้ด
            goToLobbyButton.onClick.AddListener(ShowLobbyConfirmation);
        }
    }

    // ===================================
    // QUEST PANEL MANAGEMENT
    // ===================================

    public void ToggleQuestPanel()
    {
        isQuestPanelOpen = !isQuestPanelOpen;
        questPanel.SetActive(isQuestPanelOpen);
        
        if (isQuestPanelOpen)
        {
            DisplayQuestList();
            HideQuestDetails();
        }
    }

    public void DisplayQuestList()
    {
        // ล้างปุ่มเควสเดิม
        foreach (Transform child in questListParent.transform)
        {
            Destroy(child.gameObject);
        }

        if (QuestManager.Instance == null) return;
        // 🛑 ใช้ GetAllQuests() เพื่อแสดงทั้งเควสที่รอรับและกำลังทำ
        List<IQuest> allQuests = QuestManager.Instance.GetAllQuests();

        if (allQuests.Count == 0)
        {
            GameObject noQuestText = Instantiate(questButtonPrefab, questListParent.transform);
            noQuestText.GetComponentInChildren<TextMeshProUGUI>().text = "You have no active quests.";
            if(noQuestText.GetComponent<Button>() != null)
            {
                noQuestText.GetComponent<Button>().enabled = false;
            }
        }
        else
        {
            foreach (IQuest quest in allQuests)
            {
                GameObject newButtonObj = Instantiate(questButtonPrefab, questListParent.transform);
                Button newButton = newButtonObj.GetComponent<Button>();
                newButtonObj.GetComponentInChildren<TextMeshProUGUI>().text = quest.QuestName;
                newButton.onClick.AddListener(() => DisplayQuestDetails(quest));
            }
        }
    }

    public void DisplayQuestDetails(IQuest quest)
    {
        _selectedQuest = quest;
        questListParent.SetActive(false);
        questDetailPanel.SetActive(true);
        
        detailQuestNameText.text = quest.QuestName;
        detailQuestDescriptionText.text = quest.Description;
        UpdateQuestProgressText(quest);

        // Logic การแสดงปุ่มตามสถานะเควส:
        // Accept: แสดงถ้ายังไม่ถูกรับ
        acceptButton.gameObject.SetActive(!quest.IsAccepted);
        
        // Cancel: แสดงถ้าถูกรับแล้ว (และยังไม่เสร็จ)
        // 💡 เราจะแสดง Cancel เมื่อ IsAccepted เป็นจริง และ IsCompleted เป็นเท็จ
        cancelButton.gameObject.SetActive(quest.IsAccepted && !quest.IsCompleted); 
        
        // Complete: แสดงถ้าเควสเสร็จสมบูรณ์แล้ว
        completeButton.gameObject.SetActive(quest.IsCompleted);

        // ล้าง Listener เดิม
        acceptButton.onClick.RemoveAllListeners();
        cancelButton.onClick.RemoveAllListeners();
        backButton.onClick.RemoveAllListeners();
        completeButton.onClick.RemoveAllListeners();

        // ผูก Listener ใหม่
        if(!quest.IsAccepted)
        {
            acceptButton.onClick.AddListener(AcceptQuest);
        }
        else if (quest.IsAccepted && !quest.IsCompleted)
        {
            cancelButton.onClick.AddListener(CancelQuest);
        }
        
        backButton.onClick.AddListener(HideQuestDetails);
        completeButton.onClick.AddListener(CompleteQuest);
    }
    
    public void UpdateQuestProgressText(IQuest quest)
    {
        if (questProgressText != null)
        {
            questProgressText.text = quest.ProgressText;
        }
    }

    private void HideQuestDetails()
    {
        questDetailPanel.SetActive(false);
        questListParent.SetActive(true);
    }

    // ===================================
    // QUEST ACTIONS
    // ===================================

    public void AcceptQuest()
    {
        if (_selectedQuest != null && QuestManager.Instance != null)
        {
            // 🛑 เรียกเมธอดใหม่ AcceptQuest ที่จะย้ายเควสไป Active List
            QuestManager.Instance.AcceptQuest(_selectedQuest); 
            HideQuestDetails();
            DisplayQuestList();
        }
    }

    public void CancelQuest()
    {
        if (_selectedQuest != null && QuestManager.Instance != null)
        {
            QuestManager.Instance.RemoveQuest(_selectedQuest);
            HideQuestDetails();
            DisplayQuestList();
        }
    }
    
    public void CompleteQuest()
    {
        if (_selectedQuest != null && _selectedQuest.IsCompleted && QuestManager.Instance != null)
        {
            // ✅ 1. มอบรางวัล
            GiveRewards(_selectedQuest);

            // 2. สั่ง QuestManager ลบเควสออกจาก Active List
            QuestManager.Instance.CompleteQuest(_selectedQuest);
            
            HideQuestDetails();
            DisplayQuestList();
        }
    }

    // ===================================
    // REWARD LOGIC
    // ===================================

    // ✅ เมธอดสำหรับมอบรางวัล
    private void GiveRewards(IQuest quest)
    {
        // มอบรางวัลเงิน
        if (CurrencyController.instance != null && quest.MoneyReward > 0)
        {
            float rewardAmount = quest.MoneyReward;
            CurrencyController.instance.AddMoney(rewardAmount); 
            
            // แสดงข้อความแจ้งเตือน
            ShowRewardNotification($"get reward {rewardAmount}!", notificationDisplayTime);
        }
        
        // 💡 สามารถเพิ่ม Logic สำหรับรางวัลไอเทมตรงนี้ในอนาคต
    }

    // ✅ เมธอดสำหรับแสดงข้อความแจ้งเตือนรางวัล
    public void ShowRewardNotification(string message, float duration)
    {
        if (rewardNotificationText != null)
        {
            // หยุด Coroutine เดิม (ถ้ามี) เพื่อให้ข้อความใหม่แสดงได้
            StopAllCoroutines();

            rewardNotificationText.text = message;
            rewardNotificationText.gameObject.SetActive(true);

            // เริ่ม Coroutine เพื่อรอเวลาแล้วซ่อนข้อความ
            StartCoroutine(HideNotificationAfterDelay(duration));
        }
    }

    // ===================================
    // SCENE MANAGEMENT
    // ===================================

    // ✅ 1. เมธอดสำหรับแสดงหน้าต่างยืนยัน (ผูกกับปุ่ม GoToLobbyButton เดิม)
    public void ShowLobbyConfirmation()
    {
        // ปิด UI อื่นๆ ที่อาจจะแสดงอยู่ (เช่น Quest Panel)
        if (questPanel.activeSelf)
        {
            questPanel.SetActive(false);
        }

         confirmationPanel.SetActive(true);
    
     // ✅ สำคัญ: ต้อง RemoveAllListeners ก่อน Add เสมอ
        confirmLobbyButton.onClick.RemoveAllListeners();
        cancelLobbyButton.onClick.RemoveAllListeners();

        // ผูก Listener ใหม่:
        confirmLobbyButton.onClick.AddListener(GoToLobby);
        cancelLobbyButton.onClick.AddListener(HideLobbyConfirmation);
    }
    
    // ✅ 2. เมธอดสำหรับซ่อนหน้าต่างยืนยัน
    public void HideLobbyConfirmation()
    {
        confirmationPanel.SetActive(false);
    }

    //Go to HomeLobby
     public void GoToLobby()
    {
        // 💡 คุณอาจต้องการเพิ่มโค้ดบันทึกเกม (Save Game) ที่นี่ก่อน
        
        Debug.Log($"Loading Lobby scene: {lobbySceneName}");
        
        // ใช้ SceneManager เพื่อโหลดฉากใหม่
        SceneManager.LoadScene(lobbySceneName);
    }

    // ✅ Coroutine สำหรับซ่อนข้อความ
    private IEnumerator HideNotificationAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if(rewardNotificationText != null)
        {
            rewardNotificationText.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// เปิด/ปิดพาเนลคำแนะนำ (Question Panel)
    /// </summary>
    public void ToggleQuestionPanel()
    {
        if (questionPanel != null)
        {
            // 🛑 สั่งปิดพาเนล Quest เสมอเมื่อเปิด Question Panel
            if (questPanel.activeSelf)
            {
                questPanel.SetActive(false);
            }

            // Toggle สถานะของ Question Panel
            bool isActive = !questionPanel.activeSelf;
            questionPanel.SetActive(isActive);
            Debug.Log($"Question Panel: {(isActive ? "Opened" : "Closed")}");
        }
        else
        {
            Debug.LogError("Question Panel GameObject is not assigned in the UIManager Inspector!");
        }
    }
}