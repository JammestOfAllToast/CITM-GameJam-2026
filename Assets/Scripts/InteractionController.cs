using DefaultNamespace;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class InteractionController : MonoBehaviour
{
    [SerializeField]
    Camera playerCamera;
    [SerializeField]
    TextMeshProUGUI interactionText;
    [SerializeField]
    float interactionDistance = 5f;

    IInteractable currentTargetedInteractable;

    private InputSystem_Actions playerControls;
    private InputAction interactAction;

    private void Awake()
    {
        // Initialize input system
        playerControls = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        interactAction = playerControls.Player.Interact;
        interactAction.Enable();
        
        // Enable the player action map
        playerControls.Player.Enable();
    }

    private void OnDisable()
    {
        interactAction.Disable();
        playerControls.Player.Disable();
    }


    public void Update()
    {
        UpdateCurrentInteractable();

        UpdateInteractionText();

        CheckForInteractionInput();
    }

    void UpdateCurrentInteractable()
    {
        var ray = playerCamera.ViewportPointToRay(new Vector2(0.5f, 0.5f));

        Physics.Raycast(ray, out var hit, interactionDistance);

        currentTargetedInteractable = hit.collider?.GetComponent<IInteractable>();
    }

    void UpdateInteractionText()
    {
        if (currentTargetedInteractable == null)
        {
            interactionText.text = string.Empty;
            return;
        }

        interactionText.text = currentTargetedInteractable.InteractMessage;
    }

    void CheckForInteractionInput()
    {
        bool interact = interactAction.IsPressed();
        if (interact && currentTargetedInteractable != null) 
        {
            currentTargetedInteractable.Interact();
        }
    }
}
