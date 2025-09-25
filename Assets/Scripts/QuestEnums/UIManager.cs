// ไฟล์: UIManager.cs
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject questPanel;
    public TextMeshProUGUI questListText; 
    
    public GameObject questListParent; 
    public GameObject questButtonPrefab;
    
    public GameObject questDetailPanel;
    public TextMeshProUGUI detailQuestNameText;
    public TextMeshProUGUI detailQuestDescriptionText;
    public TextMeshProUGUI questProgressText;

    public Button acceptButton;
    public Button cancelButton;
    public Button backButton;
    public Button completeButton;

    private bool isQuestPanelOpen = false;
    private IQuest _selectedQuest;
    
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
        foreach (Transform child in questListParent.transform)
        {
            Destroy(child.gameObject);
        }

        if (QuestManager.Instance == null) return;
        List<IQuest> activeQuests = QuestManager.Instance.GetActiveQuests();

        if (activeQuests.Count == 0)
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
            foreach (IQuest quest in activeQuests)
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

        acceptButton.gameObject.SetActive(!quest.IsAccepted);
        cancelButton.gameObject.SetActive(quest.IsAccepted);
        completeButton.gameObject.SetActive(quest.IsCompleted);

        acceptButton.onClick.RemoveAllListeners();
        cancelButton.onClick.RemoveAllListeners();
        backButton.onClick.RemoveAllListeners();
        completeButton.onClick.RemoveAllListeners();

        if(!quest.IsAccepted)
        {
            acceptButton.onClick.AddListener(AcceptQuest);
        }
        else
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

    public void AcceptQuest()
    {
        if (_selectedQuest != null && QuestManager.Instance != null)
        {
            QuestManager.Instance.AddNewQuest(_selectedQuest);
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
            QuestManager.Instance.CompleteQuest(_selectedQuest);
            HideQuestDetails();
            DisplayQuestList();
        }
    }

    private void HideQuestDetails()
    {
        questDetailPanel.SetActive(false);
        questListParent.SetActive(true);
    }
}