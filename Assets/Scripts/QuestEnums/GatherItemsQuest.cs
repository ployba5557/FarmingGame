// ไฟล์: GatherItemsQuest.cs
using UnityEngine;

public class GatherItemsQuest : IQuest
{
    public string QuestName { get; }
    public string Description { get; }
    public QuestType Type => QuestType.GatherItems;
    public bool IsCompleted => _currentCount >= _requiredCount;

    // ✅ เพิ่มบรรทัดนี้เพื่อแก้ไขปัญหา
    public bool IsAccepted { get; set; }

    public string ProgressText => $"{_currentCount}/{_requiredCount}";

    private readonly string _targetItemName;
    private readonly int _requiredCount;
    private int _currentCount;

    public GatherItemsQuest(string questName, string description, string targetItemName, int requiredCount)
    {
        QuestName = questName;
        Description = description;
        _targetItemName = targetItemName;
        _requiredCount = requiredCount;
        _currentCount = 0;
        IsAccepted = false; // ตั้งค่าเริ่มต้น
    }

    public void StartQuest()
    {
        IsAccepted = true; // เมื่อเริ่มเควส สถานะจะเปลี่ยนเป็น true
        Debug.Log($"Quest '{QuestName}' started. Objective: Gather {_requiredCount} {_targetItemName}.");
    }

    public void UpdateProgress(params object[] data)
    {
        // ตรวจสอบข้อมูลที่ส่งมา
        if (data.Length > 0 && data[0] is string itemName && itemName == _targetItemName)
        {
            _currentCount++;
            Debug.Log($"Progress: {_currentCount}/{_requiredCount}");
            // อัปเดต UI เมื่อความคืบหน้าเปลี่ยนไป
            if (UIManager.Instance != null)
            {
                UIManager.Instance.UpdateQuestProgressText(this);
            }
        }
    }

    public void CompleteQuest()
    {
        Debug.Log($"Quest '{QuestName}' completed! You received a reward.");
        // เพิ่มโค้ดสำหรับให้รางวัลผู้เล่นที่นี่
    }
}