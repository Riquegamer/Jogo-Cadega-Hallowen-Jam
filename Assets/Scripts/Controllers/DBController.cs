using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class CollectedItemsData
{
    public List<string> collectedIDs = new List<string>();
}

[Serializable]
public class InventorySlotData
{
    public int slotIndex;
    public string itemID;
}

[Serializable]
public class InventorySaveData
{
    public List<InventorySlotData> slots = new List<InventorySlotData>();
}

public class DBController : MonoBehaviour
{
    public static DBController Instance { get; private set; }

    private string inventoryPath;
    private string collectedPath;

    private CollectedItemsData collectedData;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            inventoryPath = Path.Combine(Application.persistentDataPath, "Inventory.json");
            collectedPath = Path.Combine(Application.persistentDataPath, "CollectedItems.json");

            LoadCollectedItems();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #region COLLECTIBLES

    private void LoadCollectedItems()
    {
        if (File.Exists(collectedPath))
        {
            string json = File.ReadAllText(collectedPath);
            collectedData = JsonUtility.FromJson<CollectedItemsData>(json);
        }
        else
        {
            collectedData = new CollectedItemsData();
        }
    }

    private void SaveCollectedItems()
    {
        string json = JsonUtility.ToJson(collectedData, true);
        File.WriteAllText(collectedPath, json);
    }

    public bool IsItemCollected(string collectibleID)
    {
        return collectedData.collectedIDs.Contains(collectibleID);
    }

    public void MarkItemCollected(string collectibleID)
    {
        if (!collectedData.collectedIDs.Contains(collectibleID))
        {
            collectedData.collectedIDs.Add(collectibleID);
            SaveCollectedItems();
        }
    }

    #endregion

    #region INVENTORY

    public void SaveInventory(InventorySystem inventory)
    {
        InventorySaveData saveData = new InventorySaveData();

        for (int i = 0; i < inventory.maxSlots; i++)
        {
            var item = inventory.GetItemInSlot(i);
            saveData.slots.Add(new InventorySlotData
            {
                slotIndex = i,
                itemID = item != null ? item.itemID.ToString() : ""
            });
        }

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(inventoryPath, json);
    }

    public void LoadInventory()
    {
        Debug.Log("[DBController] Tentando carregar inventário...");
        
        if (!File.Exists(inventoryPath))
        {
            Debug.Log("[DBController] Arquivo de inventário não encontrado em: " + inventoryPath);
            return;
        }

        string json = File.ReadAllText(inventoryPath);
        Debug.Log("[DBController] Conteúdo do JSON: " + json);
        
        InventorySaveData saveData = JsonUtility.FromJson<InventorySaveData>(json);

        var inventory = InventorySystem.Instance;
        if (inventory == null)
        {
            Debug.LogError("[DBController] InventorySystem.Instance é null! Não é possível carregar o inventário.");
            return;
        }

        var itemDB = ItemDatabase.Instance;
        if (itemDB == null)
        {
            Debug.LogError("[DBController] ItemDatabase.Instance é null! Não é possível carregar itens.");
            return;
        }

        Debug.Log($"[DBController] Carregando {saveData.slots.Count} slots do inventário...");
        
        for (int i = 0; i < saveData.slots.Count; i++)
        {
            var slot = saveData.slots[i];
            if (!string.IsNullOrEmpty(slot.itemID))
            {
                ItemData item = itemDB.GetItemByID(slot.itemID);
                if (item != null)
                {
                    inventory.LoadItemToSlot(item, slot.slotIndex);
                    Debug.Log($"[DBController] Carregado item {item.itemName} para o slot {slot.slotIndex}");
                }
                else
                {
                    Debug.LogWarning($"[DBController] Item com ID {slot.itemID} não encontrado no ItemDatabase");
                }
            }
        }
        
        Debug.Log("[DBController] Carregamento do inventário concluído!");
    }

    #endregion
}
