using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;

    float xRotation;
    float yRotation;

    
    private InputSystem_Actions playerControls;
    private InputAction lookAction;

    private void Awake()
    {
        // Initialize input system
        playerControls = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        // Get the look action and enable it
        lookAction = playerControls.Player.Look;
        lookAction.Enable();
        
        // Enable the player action map
        playerControls.Player.Enable();
    }

    private void OnDisable()
    {
        lookAction.Disable();
        playerControls.Player.Disable();
    }

    private void Update() 
    {
        if (!GameManager.inventoryOpen)
        {  
            Vector2 mouseDelta = lookAction.ReadValue<Vector2>();

            //get mouse input
            float mouseX = mouseDelta.x * sensX * Time.deltaTime;
            float mouseY = mouseDelta.y * sensY * Time.deltaTime;

            yRotation += mouseX;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            //rotate cam and orientation
            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        }
    }
}
