// ไฟล์: FishQuest.cs
using UnityEngine;

public class FishQuest : IQuest
{
    public string QuestName { get; }
    public string Description { get; }
    public string ProgressText => $"{_currentCount}/{_requiredCount}";
    public QuestType Type => QuestType.Fish;
    public bool IsCompleted => _currentCount >= _requiredCount;
    
    // ✅ Add this line to resolve the compilation error.
    public bool IsAccepted { get; set; }

    public int MoneyReward => _moneyReward;

    private readonly string _targetFishName;
    private readonly int _requiredCount;
    private int _currentCount;

    private readonly int _moneyReward; 

    public FishQuest(string questName, string description, string targetFishName, int requiredCount, int moneyReward)
    {
        QuestName = questName;
        Description = description;
        _targetFishName = targetFishName;
        _requiredCount = requiredCount;
        _currentCount = 0;
        IsAccepted = false; // Set initial state to false.
        _moneyReward = moneyReward;
    }

    public void StartQuest()
    {
        IsAccepted = true; // Update state when the quest is accepted.
        Debug.Log($"Quest '{QuestName}' started. Objective: Catch {_requiredCount} {_targetFishName}.");
    }

    public void UpdateProgress(params object[] data)
    {
        // Check if the caught fish matches the quest.
        if (data.Length > 0 && data[0] is string fishName && fishName == _targetFishName)
        {
            _currentCount++;
            // Call the function to update the UI.
            if (UIManager.Instance != null)
            {
                UIManager.Instance.UpdateQuestProgressText(this);
            }
            Debug.Log($"Progress: {_currentCount}/{_requiredCount}");
        }
    }

    public void CompleteQuest()
    {
        Debug.Log($"Fishing quest '{QuestName}' completed!");
    }
}