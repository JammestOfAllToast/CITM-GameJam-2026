using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement On Ground")]
    public float moveSpeed;
    public Transform orientation;
    public Transform camOrientation;

    public float groundDrag;

    [Header("Movement On Ground")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    private InputSystem_Actions playerControls;
    private InputAction moveAction;
    private InputAction flyAction;

    Vector3 moveDirection;

    Rigidbody rb;

    public bool gravity;

    private void Awake()
    {
        // Initialize input system
        playerControls = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        moveAction = playerControls.Player.Move;
        moveAction.Enable();
        flyAction = playerControls.Player.Fly;
        flyAction.Enable();
        
        // Enable the player action map
        playerControls.Player.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        flyAction.Disable();
        playerControls.Player.Disable();
    }


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight*0.5f + 0.2f, whatIsGround);
        if (grounded)
        {
            rb.linearDamping = groundDrag;
        } else
        {
            rb.linearDamping = 0;
        }
    }

    void FixedUpdate()
    {
        
        if (gravity) {
            MovePlayer();
        } else
        {
            ZeroGMove();
        }
        SpeedControl();
    
    }

    private void MovePlayer()
    {
        rb.useGravity = true;
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        moveDirection = orientation.forward * moveInput.y + orientation.right * moveInput.x;
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
    } 

    private void SpeedControl()
    {   
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        if (!gravity)
        {
            flatVel.y = rb.linearVelocity.y;
        }

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;

            if (gravity)
            {
                limitedVel.y = rb.linearVelocity.y;
            }

            rb.linearVelocity = new Vector3(limitedVel.x, limitedVel.y, limitedVel.z);
        }
    }

    private void ZeroGMove()
    {
        rb.useGravity = false;
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        float flyInput = flyAction.ReadValue<float>();
        moveDirection = camOrientation.forward * moveInput.y + camOrientation.right * moveInput.x + camOrientation.up * flyInput;
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
    }
}
