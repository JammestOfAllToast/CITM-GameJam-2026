using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool isInside;
    public static bool paused = false;
    private static GameManager instance;

    private GameObject interior;
    private GameObject exterior;
    private TaskManager tm;
    private TimeController tc;
    private PlayerStats stats;

    private InputSystem_Actions uiControls;
    private InputAction pauseAction;

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
        pauseAction.Enable();
        
        uiControls.UI.Enable();
        
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        if (pauseAction != null)
        {
            pauseAction.Disable();
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

        if (paused)
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.Confined;
        } 
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1f;
        }
        Cursor.visible = paused;

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