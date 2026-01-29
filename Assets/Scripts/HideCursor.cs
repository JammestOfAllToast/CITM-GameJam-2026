using UnityEngine;

public class HideCursor : MonoBehaviour
{
    public GameObject cursor;
    void Update()
    {
        cursor.SetActive(!GameManager.inventoryOpen);
    }
}
