using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool isInside;
    private GameObject interior;
    private GameObject exterior;
    private TaskManager tm;
    private TimeController tc;
    private PlayerStats stats;


    public static int stage;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        interior = GameObject.FindWithTag("ShipInterior");
        exterior = GameObject.FindWithTag("ShipExterior");
        tm = GameObject.FindWithTag("Player").GetComponent<TaskManager>();
        stats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
        tc = GameObject.FindWithTag("TimeWizard").GetComponent<TimeController>();
    }

    void Update()
    {
        if (tm.countActiveTasks == 0 && tc.CurrentTime >= tc.EndHour && stats.HasElectricity)//multiplied by whatever really., add stats.Fuel check
        {
            stats.HasWon = true;
        }


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
