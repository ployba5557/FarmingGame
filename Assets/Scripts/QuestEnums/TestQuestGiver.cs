// ไฟล์: TestQuestGiver.cs
using System.Collections; // เพิ่มบรรทัดนี้
using System.Collections.Generic; // เพิ่มบรรทัดนี้
using UnityEngine;

public class TestQuestGiver : MonoBehaviour
{
    // กำหนดดีเลย์ระหว่างการเพิ่มเควสแต่ละครั้ง (หน่วยเป็นวินาที)
    public float questDelay = 10f;

    // สร้างลิสต์ของเควสที่คุณต้องการให้เพิ่มเข้ามา
    private List<IQuest> questsToAdd = new List<IQuest>();

    void Start()
    {
        // สร้างรายการเควสทั้งหมดที่คุณต้องการให้ทยอยเพิ่มเข้ามาในเกม
        questsToAdd.Add(new GatherItemsQuest(
            "Cut Trees",
            "Cut down a total of 2 trees to get wood.",
            "Wood",
            2
        ));
        questsToAdd.Add(new FarmQuest(
            "Pumpkin Harvest",
            "Grow and harvest 5 pumpkins.",
            "pumpkin",
            5
        ));
        questsToAdd.Add(new FishQuest(
            "Catch a Fish",
            "Go to the lake and catch 3 fish.",
            "Fish",
            3
        ));
        questsToAdd.Add(new MineQuest(
            "Mine Stones",
            "Mine a total of 10 stones to get Stone.",
            "Stone",
            10
        ));
        questsToAdd.Add(new FishQuest(
            "Catch a Fish",
            "Go to the lake and catch 3 fish.",
            "Fish",
            3
        ));
        questsToAdd.Add(new GatherItemsQuest(
            "Cut Trees",
            "Cut down a total of 2 trees to get wood.",
            "Wood",
            2
        ));
        // เพิ่มเควสอื่นๆ ที่คุณต้องการในลิสต์นี้

        // เริ่ม Coroutine เพื่อเพิ่มเควสทีละรายการ
        StartCoroutine(AddQuestsOverTime());
    }

    private IEnumerator AddQuestsOverTime()
    {
        // รอ 10 วินาทีก่อนที่จะเริ่มมอบเควสแรก
        yield return new WaitForSeconds(10f);

        // วนลูปเพื่อเพิ่มเควสทีละรายการในลิสต์
        foreach (IQuest quest in questsToAdd)
        {
            if (QuestManager.Instance != null)
            {
                // เพิ่มเควสเข้าไปใน QuestManager
                QuestManager.Instance.AddNewQuest(quest);
                Debug.Log($"Quest '{quest.QuestName}' added successfully!");
            }
            else
            {
                Debug.LogError("QuestManager instance not found. Make sure it's in the scene.");
                yield break; // หยุด Coroutine หาก QuestManager ไม่มี
            }

            // รอตามเวลาที่กำหนดก่อนจะเพิ่มเควสถัดไป
            yield return new WaitForSeconds(questDelay);
        }
    }
}