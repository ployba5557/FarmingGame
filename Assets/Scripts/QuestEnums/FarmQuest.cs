// ไฟล์: FarmQuest.cs
using UnityEngine;

public class FarmQuest : IQuest
{
    public string QuestName { get; }
    public string Description { get; }

    public string ProgressText => $"{_currentCount}/{_requiredCount}";

    public QuestType Type => QuestType.Farm;
    public bool IsCompleted => _currentCount >= _requiredCount;

    // ✅ เพิ่มบรรทัดนี้เพื่อแก้ไขปัญหา
    public bool IsAccepted { get; set; }

    private readonly string _targetItemName;
    private readonly int _requiredCount;
    private int _currentCount;

    public FarmQuest(string questName, string description, string targetItemName, int requiredCount)
    {
        QuestName = questName;
        Description = description;
        _targetItemName = targetItemName;
        _requiredCount = requiredCount;
        _currentCount = 0;
        IsAccepted = false; // กำหนดค่าเริ่มต้นเป็น false
    }

    public void StartQuest()
    {
        IsAccepted = true; // เมื่อเริ่มเควส สถานะจะเปลี่ยนเป็น true
        Debug.Log($"Quest '{QuestName}' started. Objective: Grow and harvest {_requiredCount} {_targetItemName}.");
    }

    public void UpdateProgress(params object[] data)
    {
        if (data.Length > 0 && data[0] is string itemName && itemName == _targetItemName)
        {
            _currentCount++;
            if (UIManager.Instance != null)
            {
                UIManager.Instance.UpdateQuestProgressText(this);
            }
            Debug.Log($"Progress: {_currentCount}/{_requiredCount}");
        }
    }

    public void CompleteQuest()
    {
        Debug.Log($"Farm quest '{QuestName}' completed!");
    }
}