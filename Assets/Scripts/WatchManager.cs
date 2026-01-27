using UnityEngine;

public class WatchManager : MonoBehaviour
{
    private bool closed;
    void OnEnable()
    {
        GetComponent<Animator>().Play("WatchAnimation", -1, 1f);
        closed = false;
    }

    void Update()
    {
        if (!GameManager.inventoryOpen && !closed)
        {
            closed = true;
            GetComponent<Animator>().speed = -1f;
            GetComponent<Animator>().Play("WatchAnimation", -1, 1f);
            Invoke("Deactivate", GetComponent<Animation>().clip.length);
        }
    }

    void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
