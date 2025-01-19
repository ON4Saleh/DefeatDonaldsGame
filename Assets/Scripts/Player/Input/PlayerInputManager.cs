using UnityEngine;
using UnityEngine.LowLevel;


public class PlayerInputManager : MonoBehaviour
{

    private PlayerInput playerInput;
    private PlayerInput.MoveActions moveActions;
    private PlayerController playerController;
    private PlayerLook playerLook;
    private void Awake()
    {
        InitializeComponents();
        RegisterJumpAction();
    }

    private void InitializeComponents()
    {
        playerInput = new PlayerInput();
        moveActions = playerInput.Move;
        playerController = GetComponent<PlayerController>();
        playerLook = GetComponent<PlayerLook>();
    }

    private void RegisterJumpAction()
    {
        moveActions.Jump.performed += ctx => playerController.Jump();
    }
    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void LateUpdate()
    {
        HandleLook();
    }

    private void HandleMovement()
    {
        Vector2 movementInput = moveActions.Movement.ReadValue<Vector2>();
        playerController.MovePlayer(movementInput);
    }

    private void HandleLook()
    {
        Vector2 lookInput = moveActions.Look.ReadValue<Vector2>();
        playerLook.cameraLook(lookInput);
    }

    private void OnEnable()
    {
        moveActions.Enable();
    }

    private void OnDisable()
    {
        moveActions.Disable();
    }
}