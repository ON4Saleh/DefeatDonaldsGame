using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float playerSpeed = 5f;
    private CharacterController characterController;
    private bool isGrounded;
    private float gravity = -9.8f;
    private Vector3 playerVelocity;
    private float jumpHeight = 2f;
    private int jumpCount = 0;
    private int maxJumps = 2; 

    private void Start()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        characterController = GetComponent<CharacterController>();
    }

    public void MovePlayer(Vector2 input)
    {
        UpdateGroundStatus();

        Vector3 movementDirection = new Vector3(input.x, 0, input.y);
        Vector3 transformedDirection = transform.TransformDirection(movementDirection);
        characterController.Move(transformedDirection * playerSpeed * Time.deltaTime);

        ApplyGravity();
    }

    public void Jump()
    {
        if (isGrounded || jumpCount < maxJumps)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpCount++;
        }
    }

    private void UpdateGroundStatus()
    {
        isGrounded = characterController.isGrounded;

        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
            jumpCount = 0; 
        }
    }

    private void ApplyGravity()
    {
        playerVelocity.y += gravity * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);
    }
}
