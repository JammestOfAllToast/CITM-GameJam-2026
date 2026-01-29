using UnityEngine;

public class FlashlightScript : MonoBehaviour
{
    public PlayerStats PS;
    public GameObject Flashlight;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Flashlight.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
            Flashlight.SetActive(!PS.HasElectricity);
    }
}
