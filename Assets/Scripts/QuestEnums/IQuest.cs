// ไฟล์: IQuest.cs
// Interface สำหรับเควสทุกประเภท

public interface IQuest
{
    // ชื่อเควส
  string QuestName { get; }
    // คำอธิบายเควส
  string Description { get; }
    // ประเภทของเควส
  QuestType Type { get; }
    // สถานะว่าเควสเสร็จสิ้นแล้วหรือไม่
  bool IsCompleted { get; }
    
    // ✅ เพิ่มบรรทัดนี้เพื่อแก้ไขปัญหา
    bool IsAccepted { get; }

  string ProgressText { get; }

    // ฟังก์ชันสำหรับเริ่มต้นเควส
  void StartQuest();
    // ฟังก์ชันสำหรับอัปเดตความคืบหน้าของเควส
  void UpdateProgress(params object[] data);
    // ฟังก์ชันสำหรับเมื่อเควสเสร็จสิ้น
  void CompleteQuest();
}