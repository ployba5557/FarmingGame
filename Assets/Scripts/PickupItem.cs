using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public string itemName = "Log";
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // เพิ่มไอเทมเข้า Inventory
            if (ItemController.instance != null)
            {
                ItemController.instance.AddItem(itemName, 1);
            }
            Destroy(gameObject);
        }
    }
}