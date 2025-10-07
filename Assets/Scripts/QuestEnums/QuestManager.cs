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

    private readonly List<IQuest> _availableQuests = new List<IQuest>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddAvailableQuest(IQuest newQuest)
{
    if (!_availableQuests.Contains(newQuest) && !_activeQuests.Contains(newQuest))
    {
        // 🛑 ห้ามเรียก newQuest.StartQuest() ที่นี่
        _availableQuests.Add(newQuest);
        Debug.Log($"Quest '{newQuest.QuestName}' is now available.");
        
        if (uiManager != null)
        {
            uiManager.DisplayQuestList();
        }
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
    
    public void AcceptQuest(IQuest quest)
{
    if (_availableQuests.Contains(quest))
    {
        _availableQuests.Remove(quest);
        _activeQuests.Add(quest);
        quest.StartQuest(); // ✅ เรียก StartQuest() ที่นี่
        
        Debug.Log($"Quest '{quest.QuestName}' has been accepted and started.");

        if (uiManager != null)
        {
            uiManager.DisplayQuestList();
        }
    }
}


    public void UpdateQuestProgress(string itemName)
    {
        for (int i = _activeQuests.Count - 1; i >= 0; i--)
    {
        IQuest quest = _activeQuests[i];
        
        // 1. อัปเดตความคืบหน้า
        quest.UpdateProgress(itemName);
        
        if (uiManager != null)
        {
            uiManager.UpdateQuestProgressText(quest);
        }
        
        // 2. ตรวจสอบว่าเควสเสร็จสมบูรณ์หรือไม่
        if (quest.IsCompleted)
        {
            // 🛑 สำคัญ: ไม่ต้องเรียก CompleteQuest(quest) ตรงนี้
            // 🛑 CompleteQuest จะถูกเรียกเมื่อผู้เล่นกดปุ่มใน UI เท่านั้น
            
            Debug.Log($"Quest '{quest.QuestName}' is ready to complete!"); 
            // 💡 เราปล่อยให้มันยังอยู่ใน _activeQuests จนกว่าผู้เล่นจะกดรับรางวัล
        }
        }
    }
    
    public void CompleteQuest(IQuest quest)
    {
        if (_activeQuests.Contains(quest))
    {
        quest.CompleteQuest();
        _activeQuests.Remove(quest); // ✅ บรรทัดนี้ทำงานเมื่อกดปุ่มใน UI
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

    public List<IQuest> GetAllQuests()
{
    // แสดงเควสที่รอรับและเควสที่กำลังทำ
    List<IQuest> allQuests = new List<IQuest>(_availableQuests);
    allQuests.AddRange(_activeQuests);
    return allQuests;
}
}