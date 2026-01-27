using UnityEngine;

public class WatchManager : MonoBehaviour
{
    void OnEnable()
    {
        GetComponent<Animator>().Play("WatchAnimation", -1, 0f);
    }

    void Update()
    {
        if (!GameManager.inventoryOpen)
        {
            gameObject.SetActive(false);
        }
    }
}
