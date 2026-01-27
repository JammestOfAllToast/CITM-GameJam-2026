using UnityEngine;
using UnityEngine.UI;

public class OxigenUI : MonoBehaviour
{
    private PlayerStats stats;
    public RawImage image;

    void Start()
    {
        stats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();

    }

    // Update is called once per frame
    void Update()
    {
            image.enabled = !stats.IsThereOxygenAround;
     
    }
}
