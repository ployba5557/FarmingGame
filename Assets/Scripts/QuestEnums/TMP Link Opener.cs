// ไฟล์: TMPLinkOpener.cs
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TMPLinkOpener : MonoBehaviour, IPointerClickHandler
{
    private TextMeshProUGUI _textMeshPro;
    // เพิ่มตัวแปรสำหรับเก็บ UIManager
    private UIManager _uiManager;

    private void Awake()
    {
        _textMeshPro = GetComponent<TextMeshProUGUI>();
        // ดึง UIManager Instance มาเก็บไว้ในตัวแปรตั้งแต่ Awake
        _uiManager = UIManager.Instance;

        // เพิ่มการ Debug เพื่อตรวจสอบว่า UIManager ถูกหาเจอหรือไม่
        if (_uiManager == null)
        {
            Debug.LogError("UIManager instance not found!");
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // ตรวจสอบว่า UIManager ไม่ใช่ค่าว่างก่อนใช้งาน
        if (_uiManager == null)
        {
            Debug.LogError("UIManager is null. Cannot open quest details.");
            return;
        }

        int linkIndex = TMP_TextUtilities.FindIntersectingLink(_textMeshPro, Input.mousePosition, null);

        if (linkIndex != -1)
        {
            TMP_LinkInfo linkInfo = _textMeshPro.textInfo.linkInfo[linkIndex];
            string questName = linkInfo.GetLinkID();
            
            // หาเควสที่ตรงกับชื่อที่คลิก
            IQuest selectedQuest = QuestManager.Instance.FindQuestByName(questName);
            
            if (selectedQuest != null)
            {
                // เรียก UIManager ผ่านตัวแปร _uiManager
                _uiManager.DisplayQuestDetails(selectedQuest);
            }
        }
    }
}