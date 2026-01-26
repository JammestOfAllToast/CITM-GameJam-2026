using UnityEngine;
using UnityEngine.UI;

public class ShowWhenPause : MonoBehaviour
{
    public GameObject image;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        image.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        image.SetActive(GameManager.paused);
    }
}
