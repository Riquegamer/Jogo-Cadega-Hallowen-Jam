using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    [SerializeField] private bool _isDialogueOnCollect;
    public ItemData itemData;
    public string collectibleID; // identificador único
    [SerializeField] DialogueData dialogueOnCollect;

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
                if (_isDialogueOnCollect) {
                    DialogueController dialogueController = GetComponent<DialogueController>();
                    if (dialogueController != null)
                    {
                        dialogueController.dialogueData = dialogueOnCollect;
                        dialogueController.Next();
                        Destroy(gameObject);
                    }
                }
                else
                {
                    Destroy(gameObject);
                }
                
            }
            else
            {
                Debug.Log("Inventário cheio! Não foi possível coletar o item.");
            }
        }
    }
}