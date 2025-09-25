// ไฟล์: QuestManager.cs
// จัดการระบบเควสทั้งหมด

using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    // Singleton pattern เพื่อให้เข้าถึงได้จากทุกที่
    public static QuestManager Instance { get; private set; }

    public UIManager uiManager;

    // รายการเควสที่กำลังดำเนินอยู่
    private readonly List<IQuest> _activeQuests = new List<IQuest>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddNewQuest(IQuest newQuest)
    {
        if (!_activeQuests.Contains(newQuest))
        {
            _activeQuests.Add(newQuest);
            newQuest.StartQuest();
            Debug.Log($"Quest '{newQuest.QuestName}' has been added.");
            
            if (uiManager != null)
            {
                uiManager.UpdateQuestProgressText(newQuest);
                uiManager.DisplayQuestList();
            }
        }
    }
    
    public void RemoveQuest(IQuest quest)
    {
        if (_activeQuests.Contains(quest))
        {
            _activeQuests.Remove(quest);
            Debug.Log($"Quest '{quest.QuestName}' has been cancelled.");
        }
    }

    public void UpdateQuestProgress(string itemName)
    {
        for (int i = _activeQuests.Count - 1; i >= 0; i--)
        {
            IQuest quest = _activeQuests[i];
            
            // อัปเดตความคืบหน้า
            quest.UpdateProgress(itemName);
            
            // อัปเดต UI ทันที
            if (uiManager != null)
            {
                uiManager.UpdateQuestProgressText(quest);
            }
            
            // ตรวจสอบว่าเควสเสร็จสมบูรณ์หรือไม่
            if (quest.IsCompleted)
            {
                CompleteQuest(quest);
            }
        }
    }
    
    public void CompleteQuest(IQuest quest)
    {
        if (_activeQuests.Contains(quest))
        {
            quest.CompleteQuest();
            _activeQuests.Remove(quest);
            Debug.Log($"Quest '{quest.QuestName}' completed and removed from the active list.");

            if (uiManager != null)
            {
                uiManager.DisplayQuestList();
            }
        }
    }
    
    public IQuest FindQuestByName(string questName)
    {
        foreach (var quest in _activeQuests)
        {
            if (quest.QuestName == questName)
            {
                return quest;
            }
        }
        return null;
    }

    public List<IQuest> GetActiveQuests()
    {
        return _activeQuests;
    }
}