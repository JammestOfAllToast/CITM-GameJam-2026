using UnityEngine;

public class WatchManager : MonoBehaviour
{
    void OnEnable()
    {
        GetComponent<Animator>().Play("WatchAnimation", -1, 1f);
    }

    void Update()
    {
       // GetComponent<Animator>().speed = -1f;
     //   GetComponent<Animator>().Play("WatchAnimation", -1, 1f);
      //  Invoke("Deactivate", GetComponent<Animation>().clip.length);
    }

    void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
