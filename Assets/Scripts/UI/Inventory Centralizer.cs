using UnityEngine;

public class InventoryCentralizer : MonoBehaviour
{
    private void Update()
    {
        transform.position += (transform.parent.position - transform.position) * 5 * Time.deltaTime;
    }
}
