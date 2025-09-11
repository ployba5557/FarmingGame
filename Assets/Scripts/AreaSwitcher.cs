using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaSwitcher : MonoBehaviour
{
    public string sceneToLoad;

    public Transform startPoint;
    public string transitionName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (PlayerController.instance != null)
        {
            if (PlayerPrefs.HasKey("Transition"))
            {
                if (PlayerPrefs.GetString("Transition") == transitionName)
                {
                    PlayerController.instance.transform.position = startPoint.position;

                    // ✅ ล้างค่า Transition หลังใช้งาน เพื่อไม่ให้ AreaSwitcher อื่นทำงานผิดที่
                    PlayerPrefs.DeleteKey("Transition");
                }
            }
        }
        else
        {
            Debug.LogWarning("PlayerController instance is null.");
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            //Debug.Log("Player entered");

            SceneManager.LoadScene(sceneToLoad);

            PlayerPrefs.SetString("Transition", transitionName);
        }
    }
}
