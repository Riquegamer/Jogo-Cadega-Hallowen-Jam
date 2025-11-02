using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    public ItemData itemData;
    public string collectibleID; // identificador único

    private void Start()
    {
        if (DBController.Instance.IsItemCollected(collectibleID))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            bool collected = InventorySystem.Instance.AddItem(itemData);

            if (collected)
            {
                DBController.Instance.MarkItemCollected(collectibleID);
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Inventário cheio! Não foi possível coletar o item.");
            }
        }
    }
}
