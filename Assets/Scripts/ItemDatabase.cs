using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase Instance;
    public ItemData[] allItems;

    private void Awake()
    {
        Instance = this;
    }

    public ItemData GetItemByID(string id)
    {
        foreach (var item in allItems)
        {
            if (item.itemID.ToString() == id)
                return item;
        }
        return null;
    }
}
