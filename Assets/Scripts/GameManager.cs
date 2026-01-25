using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool isInside;
    private GameObject interior;
    private GameObject exterior;

    public int stage;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        interior = GameObject.FindWithTag("ShipInterior");
        exterior = GameObject.FindWithTag("ShipExterior");
    }

    void Update()
    {
        if (interior != null && interior != null) {
            if (isInside)
            {
                interior.SetActive(true);
                exterior.SetActive(false);
            } else
            {
                interior.SetActive(false);
                exterior.SetActive(true);
            }
        }
    }

}
