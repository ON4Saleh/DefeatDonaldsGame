using UnityEngine;
using static Bullet;

public class PlayerController : MonoBehaviour
{
    [Header("Player Movement")]
    [SerializeField] float playerSpeed = 5f;
    [SerializeField] CharacterController characterController;
    [SerializeField] bool isGrounded;
    [SerializeField] float gravity = -9.8f;
    [SerializeField] Vector3 playerVelocity;
    [SerializeField] float jumpHeight = 2f;

    [Header("Player Respawn")]
    [SerializeField] int respawnAttempts = 2;
    [SerializeField] public Vector3 initialPosition = new Vector3(0, 0, 0);
    [SerializeField] public Vector3 moneyPosition = new Vector3(-16.9400005f, 0.579999983f, 7.42000008f); // Updated position

    private PlayerHealth playerHealth;

    void Start()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        characterController = GetComponent<CharacterController>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    void Update()
    {
        // Handle movement and other updates
        MovePlayer(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
        CheckRespawn(); // Check for respawn conditions
    }

    public void CheckRespawn()
    {
        if (playerHealth.playerWaterLevel <= 0)
        {
            Respawn(initialPosition); // Respawn at water point
        }
        else if (playerHealth.playerMoneyLevel <= 0)
        {
            Respawn(moneyPosition); // Respawn at money point
        }
    }

    private void Respawn(Vector3 respawnPoint)
    {
        if (respawnAttempts > 0)
        {
            transform.position = respawnPoint; // Set player position to respawn point
            playerHealth.ResetHealth(respawnPoint); // Reset health and score
            playerHealth.playerScore += 200;
            respawnAttempts--;
        }
        else
        {
            GameOver(); // Game over if no respawn attempts left
        }
    }


    public void GameOver()
    {
        Debug.Log("Game Over");
        // Display final score and stats if needed
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
        if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        }
    }
    private void UpdateGroundStatus()
    {
        isGrounded = characterController.isGrounded;

        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }
    }

    private void ApplyGravity()
    {
        playerVelocity.y += gravity * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);
    }
}
