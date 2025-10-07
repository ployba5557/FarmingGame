using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemManager : MonoBehaviour
{
    private static EventSystemManager instance;

    void Awake()
    {
        // ตรวจสอบว่ามี EventSystemManager นี้อยู่แล้วหรือไม่
        if (instance == null)
        {
            // ถ้ายังไม่มี ให้กำหนดให้ตัวนี้เป็น Instance และป้องกันการถูกทำลาย
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            // ถ้ามีอยู่แล้ว (เช่น ตัวเก่ามาจากฉากอื่น) ให้ทำลายตัวเองทิ้ง
            Destroy(gameObject);
        }
    }
}