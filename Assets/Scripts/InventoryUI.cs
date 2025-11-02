using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;

    public Image[] slotImages;
    public Sprite emptySlotSprite;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("[InventoryUI] Instância inicializada");
        }
        else
        {
            Debug.LogWarning("[InventoryUI] Múltiplas instâncias detectadas!");
            Destroy(gameObject);
            return;
        }

        if (slotImages == null || slotImages.Length == 0)
        {
            Debug.LogError("[InventoryUI] slotImages não configurado!");
            return;
        }

        // Inicializa slots vazios
        for (int i = 0; i < slotImages.Length; i++)
        {
            UpdateSlot(i, null);
            Debug.Log($"[InventoryUI] Slot {i} inicializado como vazio");
        }
    }

    public void UpdateSlot(int index, ItemData item)
    {
        if (index < 0 || index >= slotImages.Length) return;

        if (item != null)
        {
            slotImages[index].sprite = item.icon;
            slotImages[index].color = Color.white;
        }
        else
        {
            slotImages[index].sprite = emptySlotSprite;
            slotImages[index].color = new Color(1, 1, 1, 0.5f);
        }
    }
}
