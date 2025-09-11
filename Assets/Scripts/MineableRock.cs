using UnityEngine;

public class MineableRock : MonoBehaviour
{
    public int hitPoints = 3; // ขุดได้ 3 ครั้ง
    public GameObject dropPrefab; // Prefab เศษหิน
    public Transform dropPoint; // จุด spawn
    public string uniqueID;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();

        if (ObjectSaveManager.instance != null && ObjectSaveManager.instance.IsRemoved(uniqueID))
        {
            Destroy(gameObject); // ❌ เคยถูกทำลายไปแล้ว ไม่ต้องโผล่
            return;
        }
    }

    public void Mine()
    {


        hitPoints--;

        if (anim != null)
        {
            anim.SetTrigger("Mine");
        }

        if (hitPoints <= 0)
        {
            ObjectSaveManager.instance.MarkObjectDestroyed(uniqueID);

            //if (ObjectSaveManager.instance != null && ObjectSaveManager.instance.IsRemoved(uniqueID))
            //{
            //    Destroy(gameObject);
            //}


            if (dropPrefab != null && dropPoint != null)
            {
                Instantiate(dropPrefab, dropPoint.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }
    }
}
