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
            2,
            30
        ));
        questsToAdd.Add(new FarmQuest(
            "Pumpkin Harvest",
            "Grow and harvest 5 pumpkins.",
            "pumpkin",
            5,
            100
        ));
        questsToAdd.Add(new FishQuest(
            "Catch a Fish",
            "Go to the lake and catch 8 fish.",
            "Fish",
            8,
            40
        ));
        questsToAdd.Add(new MineQuest(
            "Mine Stones",
            "Mine a total of 10 stones to get Stone.",
            "Stone",
            10,
            100
        ));
        questsToAdd.Add(new FishQuest(
            "Catch a Fish",
            "Go to the lake and catch 5 fish.",
            "Fish",
            5,
            20
        ));
        questsToAdd.Add(new GatherItemsQuest(
            "Cut Trees",
            "Cut down a total of 5 trees to get wood.",
            "Wood",
            5,
            35
        ));
        questsToAdd.Add(new GatherItemsQuest(
            "Mushroom picking",
            "Collect 5 Mushroom",
            "Mushroom",
            5,
            500
        ));
        questsToAdd.Add(new FarmQuest(
            "Lettuce Harvest",
            "Grow and harvest 3 Lettuce.",
            "lettuce",
            6,
            45
        ));
        questsToAdd.Add(new MineQuest(
            "Mine Stones",
            "Mine a total of 5 stones to get Stone.",
            "Stone",
            5,
            50
        ));
        questsToAdd.Add(new GatherItemsQuest(
            "Cut Trees",
            "Cut down a total of 8 trees to get wood.",
            "Wood",
            8,
            52
        ));
        questsToAdd.Add(new FarmQuest(
            "Carrot Harvest",
            "Grow and harvest 3 Carrot.",
            "carrot",
            6,
            95
        ));
        questsToAdd.Add(new FishQuest(
            "Catch a Fish",
            "Go to the lake and catch 10 fish.",
            "Fish",
            10,
            100
        ));
        questsToAdd.Add(new MineQuest(
            "Mine Stones",
            "Mine a total of 9 stones to get Stone.",
            "Stone",
            9,
            50
        ));
        questsToAdd.Add(new FarmQuest(
            "Hay Harvest",
            "Grow and harvest 3 Hay.",
            "hay",
            6,
            95
        ));
        questsToAdd.Add(new GatherItemsQuest(
            "Mushroom picking",
            "Collect 2 Mushroom",
            "Mushroom",
            5,
            70
        ));
        questsToAdd.Add(new GatherItemsQuest(
            "Cut Trees",
            "Cut down a total of 10 trees to get wood.",
            "Wood",
            10,
            70
        ));
        questsToAdd.Add(new FarmQuest(
            "Pumpkin Harvest",
            "Grow and harvest 10 pumpkins.",
            "pumpkin",
            10,
            50
        ));
        questsToAdd.Add(new FishQuest(
            "Catch a Fish",
            "Go to the lake and catch 7 fish.",
            "Fish",
            7,
            30
        ));
        questsToAdd.Add(new MineQuest(
            "Mine Stones",
            "Mine a total of 10 stones to get Stone.",
            "Stone",
            10,
            50
        ));
        questsToAdd.Add(new FarmQuest(
            "Strawberry Harvest",
            "Grow and harvest 10 strawberry.",
            "strawberry",
            10,
            100
        ));
        questsToAdd.Add(new GatherItemsQuest(
            "Mushroom picking",
            "Collect 2 Mushroom",
            "Mushroom",
            5,
            90
        ));
        questsToAdd.Add(new GatherItemsQuest(
            "Cut Trees",
            "Cut down a total of 3 trees to get wood.",
            "Wood",
            3,
            20
        ));
        questsToAdd.Add(new FarmQuest(
            "Potato Harvest",
            "Grow and harvest 10 Potato.",
            "potato",
            5,
            100
        ));
        questsToAdd.Add(new FishQuest(
            "Catch a Fish",
            "Go to the lake and catch 12 fish.",
            "Fish",
            12,
            100
        ));
        questsToAdd.Add(new MineQuest(
            "Mine Stones",
            "Mine a total of 10 stones to get Stone.",
            "Stone",
            7,
            60
        ));
        questsToAdd.Add(new GatherItemsQuest(
            "Mushroom picking",
            "Collect 2 Mushroom",
            "Mushroom",
            5,
            90
        ));
        questsToAdd.Add(new FarmQuest(
            "Tomato Harvest",
            "Grow and harvest 5 Tomato.",
            "tomato",
            5,
            70
        ));
        questsToAdd.Add(new GatherItemsQuest(
            "Cut Trees",
            "Cut down a total of 13 trees to get wood.",
            "Wood",
            13,
            46
        ));
        questsToAdd.Add(new FishQuest(
            "Catch a Fish",
            "Go to the lake and catch 4 fish.",
            "Fish",
            4,
            25
        ));
        questsToAdd.Add(new FarmQuest(
            "Eggplant Harvest",
            "Grow and harvest 5 Eggplant.",
            "eggplant",
            5,
            100
        ));

        

        
        // เพิ่มเควสอื่นๆ ที่คุณต้องการในลิสต์นี้

        // เริ่ม Coroutine เพื่อเพิ่มเควสทีละรายการ
        StartCoroutine(AddQuestsOverTime());
    }

    private IEnumerator AddQuestsOverTime()
    {
        // รอ 10 วินาทีก่อนที่จะเริ่มมอบเควสแรก
        yield return new WaitForSeconds(20f);

        // วนลูปเพื่อเพิ่มเควสทีละรายการในลิสต์
        foreach (IQuest quest in questsToAdd)
        {
            if (QuestManager.Instance != null)
            {
            // 🛑 เปลี่ยนจาก AddNewQuest เป็น AddAvailableQuest
            QuestManager.Instance.AddAvailableQuest(quest); 
            Debug.Log($"Quest '{quest.QuestName}' added successfully to available list!");
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