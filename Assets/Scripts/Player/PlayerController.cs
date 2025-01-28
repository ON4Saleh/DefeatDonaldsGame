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
    [Header("Player Stats")]
    public int score = 0;
    public float waterLevel = 1000f;
    public float moneyLevel = 10000f;
    [Header("Player Respawn")]
    [SerializeField] int respawnAttempts = 2;
    [SerializeField] Vector3 initialPosition;

    private Enemy enemy;
    private Bullet bullet;

    private void Start()
    {
        InitializeComponents();
        initialPosition = transform.position;
    }

    private void Update()
    {
        if (waterLevel <= 0)
        {
            RespawnPlayer();
        }
    }
    public void RespawnPlayer()
    {
        if (respawnAttempts > 0)
        {
            respawnAttempts--;
            transform.position = initialPosition;
            score -= 50;
            waterLevel = 1000f;
        }
        else
        {
            GameOver();
        }
    }
    private void GameOver()
    {
        Debug.Log("Game Over");
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