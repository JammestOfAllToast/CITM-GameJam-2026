using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static bool isInside;
    public static bool paused = false;

    private GameObject interior;
    private GameObject exterior;
    private TaskManager tm;
    private TimeController tc;
    private PlayerStats stats;

    private InputSystem_Actions uiControls;
    private InputAction pauseAction;


    public static int stage;
    public static int finalStage = 5;


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

        if (pauseAction.WasPressedThisFrame())
        {
            paused = !paused;
        }

        if (paused)
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.Confined;
        } else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1f;
        }
            Cursor.visible = paused;

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


    private void Awake()
    {
        // Initialize input system
        uiControls = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        // Get the look action and enable it
        pauseAction = uiControls.UI.Pause;
        pauseAction.Enable();
        
        // Enable the player action map
        uiControls.UI.Enable();
    }

    private void OnDisable()
    {
        pauseAction.Disable();
        uiControls.UI.Disable();
    }

}
