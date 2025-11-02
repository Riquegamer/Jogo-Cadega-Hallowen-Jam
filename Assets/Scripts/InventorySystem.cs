using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance;

    public int maxSlots = 6;
    [SerializeField] private ItemData[] slots;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // Preserve inspector-assigned or previously serialized slots if present.
        if (slots == null || slots.Length != maxSlots)
        {
            slots = new ItemData[maxSlots];
        }
    }

   
    public bool AddItem(ItemData newItem)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
            {
                slots[i] = newItem;
                if (InventoryUI.Instance != null)
                    InventoryUI.Instance.UpdateSlot(i, newItem);
                Debug.Log($"Item {newItem.itemName} adicionado no slot {i}");
                if (DBController.Instance != null)
                    DBController.Instance.SaveInventory(this);
                return true;
            }
        }

        Debug.Log("Invent�rio cheio!");
        return false;
    }

    public void RemoveItem(int index)
    {
        if (index >= 0 && index < slots.Length)
        {
            slots[index] = null;
            if (InventoryUI.Instance != null)
                InventoryUI.Instance.UpdateSlot(index, null);
            if (DBController.Instance != null)
                DBController.Instance.SaveInventory(this);
        }
    }

    public void LoadItemToSlot(ItemData item, int index)
    {
        if (index < 0 || index >= slots.Length)
        {
            Debug.LogWarning($"[InventorySystem] Tentativa de carregar item para slot inválido: {index}");
            return;
        }

        slots[index] = item;
        Debug.Log($"[InventorySystem] Item {item.itemName} carregado no slot {index}");
        
        if (InventoryUI.Instance != null)
        {
            InventoryUI.Instance.UpdateSlot(index, item);
            Debug.Log($"[InventorySystem] UI atualizada para o slot {index}");
        }
        else
        {
            Debug.LogWarning("[InventorySystem] InventoryUI.Instance é null! Interface não atualizada.");
        }
    }


    public InventorySaveData GetSaveData()
    {
        InventorySaveData saveData = new InventorySaveData();
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] != null)
            {
                saveData.slots.Add(new InventorySlotData
                {
                    slotIndex = i,
                    itemID = slots[i].itemID.ToString()
                });
            }
        }
        return saveData;
    }



    public bool IsFull()
    {
        foreach (var slot in slots)
        {
            if (slot == null)
                return false;
        }
        return true;
    }

    public void AddItemToSlot(ItemData item, int index)
    {
        if (index < 0 || index >= slots.Length) return;

        slots[index] = item;
        if (InventoryUI.Instance != null)
            InventoryUI.Instance.UpdateSlot(index, item);
        if (DBController.Instance != null)
            DBController.Instance.SaveInventory(this);
    }

    // -------------------- M�TODOS PARA SAVE/LOAD --------------------

    // Retorna item no slot (necess�rio para salvar)
    public ItemData GetItemInSlot(int index)
    {
        if (index < 0 || index >= slots.Length) return null;
        return slots[index];
    }



}
