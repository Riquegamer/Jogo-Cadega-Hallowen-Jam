using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryController : MonoBehaviour
{
    public GameObject mouseItem;

    public void DraagItem(GameObject button)
    {
        mouseItem = button;
        if (Mouse.current != null)
            mouseItem.transform.position = Mouse.current.position.ReadValue();
    }

    public void DropItem(GameObject button)
    {
        if (mouseItem == null) return;

        Transform aux = mouseItem.transform.parent;
        mouseItem.transform.SetParent(button.transform.parent);
        button.transform.SetParent(aux);
        mouseItem = null;
    }
}
