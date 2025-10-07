using UnityEngine;

public class MineQuest : IQuest
{
    public string QuestName { get; }
    public string Description { get; }
    public string ProgressText => $"{_currentCount}/{_requiredCount}";
    public QuestType Type => QuestType.Mine;
    public bool IsCompleted => _currentCount >= _requiredCount;
    public bool IsAccepted { get; set; }

    public int MoneyReward => _moneyReward;


    private readonly string _targetItemName;
    private readonly int _requiredCount;
    private int _currentCount;

    private readonly int _moneyReward;

    public MineQuest(string questName, string description, string targetItemName, int requiredCount, int moneyReward)
    {
        QuestName = questName;
        Description = description;
        _targetItemName = targetItemName;
        _requiredCount = requiredCount;
        _currentCount = 0;
        IsAccepted = false;
        _moneyReward = moneyReward;
    }

    public void StartQuest()
    {
        IsAccepted = true;
        Debug.Log($"Quest '{QuestName}' started. Objective: Mine {_requiredCount} {_targetItemName}.");
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
        Debug.Log($"Mine quest '{QuestName}' completed!");
    }
}