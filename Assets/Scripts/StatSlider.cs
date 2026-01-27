using UnityEngine;
using UnityEngine.UI;

public class StatSlider : MonoBehaviour
{
    public PlayerStats stats;
    public string type;
    public Slider slider;

    void Start()
    {
        stats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();

    }

    void Update()
    {
        if (type == "par")
        {
            slider.maxValue = stats.ParanoiaHardLimit;
            slider.value = stats.Paranoia;
        } else if (type == "o2")
        {
            slider.maxValue = 100;
            slider.value = stats.Oxygen;
        } else if (type == "elec")
        {
            slider.maxValue = 1;
            slider.value = stats.HasElectricity ? 1f : 0f;
        }else if (type == "fuel")
        {
            slider.maxValue = 100;
            slider.value = stats.Fuel;
        }
    }
}
