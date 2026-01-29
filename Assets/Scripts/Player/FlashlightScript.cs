using UnityEngine;

public class FlashlightScript : MonoBehaviour
{
    private PlayerStats PS;
    public GameObject Flashlight;
    private GameObject[] ShipLights;

    void Start()
    {
        PS = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
        ShipLights = GameObject.FindGameObjectsWithTag("Light");
        Flashlight.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
            Flashlight.SetActive(!PS.HasElectricity);
            foreach (GameObject light in ShipLights)
            {
                light.SetActive(PS.HasElectricity);
            }
    }
}
