using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
    // ตั้งค่าชื่อ Scene เลือกตัวละครใน Inspector
    public string characterSelectSceneName = "CharacterSelectScene";

    // เมธอดนี้จะถูกเรียกเมื่อกดปุ่ม "Play"
    public void OnPlayButtonClicked()
    {
        // โหลด Scene เลือกตัวละคร
        SceneManager.LoadScene(characterSelectSceneName);
        Debug.Log("Loading Character Select Scene...");
    }
}