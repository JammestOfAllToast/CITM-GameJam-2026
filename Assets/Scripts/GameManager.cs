using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static bool isInside;
    public static bool paused = false;
    public static bool inventoryOpen = false;

    private static GameManager instance;

    private GameObject interior;
    private GameObject exterior;
    private TaskManager tm;
    private TimeController tc;
    private PlayerStats stats;
    private GameObject watch;

    private InputSystem_Actions uiControls;
    private InputAction pauseAction;
    private InputAction inventoryAction;


    public static int stage;
    public static int finalStage = 5;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        uiControls = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        pauseAction = uiControls.UI.Pause;
        inventoryAction = uiControls.UI.Inventory;

        pauseAction.Enable();
        inventoryAction.Enable();
        
        uiControls.UI.Enable();
        
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        if (pauseAction != null)
        {
            pauseAction.Disable();
        }
        if (inventoryAction != null)
        {
            inventoryAction.Disable();
        }
        if (uiControls != null)
        {
            uiControls.UI.Disable();
        }
        
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        InitializeReferences();
    }

    void InitializeReferences()
    {
        interior = GameObject.FindWithTag("ShipInterior");
        exterior = GameObject.FindWithTag("ShipExterior");
        
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            tm = player.GetComponent<TaskManager>();
            stats = player.GetComponent<PlayerStats>();

            watch = GameObject.FindWithTag("Player")
            .GetComponentsInChildren<Transform>(true)
            .FirstOrDefault(t => t.CompareTag("Watch"))
            ?.gameObject;

        }
        
        GameObject timeWizard = GameObject.FindWithTag("TimeWizard");
        if (timeWizard != null)
        {
            tc = timeWizard.GetComponent<TimeController>();
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitializeReferences();
        
        paused = false;
        Time.timeScale = 1f;
    }

    void Update()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        bool isInGameScene = (currentScene == "Scene with ship stats"); // Add more game scene names if needed
        bool isInMenu = (currentScene == "MainMenu" || currentScene == "Credits");
        if (stats != null && stats.IsDead)
        {
            paused = true;
        }
        
        if (tm != null && tc != null && stats != null)
        {
            if (tm.countActiveTasks == 0 && tc.CurrentTime >= tc.EndHour && stats.HasElectricity && !stats.IsDead)
            {
                stats.HasWon = true;
            }
        }

        if (pauseAction != null && pauseAction.WasPressedThisFrame() && (stats == null || !stats.IsDead))
        {
            paused = !paused;
        }

        if (inventoryAction != null && inventoryAction.WasPressedThisFrame() && (stats == null || !stats.IsDead))
        {
            inventoryOpen = !inventoryOpen;
        }

        if (paused)
        {
            Time.timeScale = 0f;
        } 
        else
        {
            Time.timeScale = 1f;
        }

        if (paused || inventoryOpen)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        } else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (inventoryOpen)
        {
            Debug.Log("watch");
            watch.SetActive(true);
        }

        if (interior != null && exterior != null) 
        {
            if (isInside)
            {
                interior.SetActive(true);
                exterior.SetActive(false);
            } 
            else
            {
                interior.SetActive(false);
                exterior.SetActive(true);
            }
        }

        if (isInGameScene)
        {
            if (paused)
            {
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
            } 
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
        else if (isInMenu)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}