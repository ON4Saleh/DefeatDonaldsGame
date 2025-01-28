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
    [SerializeField] Vector3 initialPosition;

    private PlayerHealth plyrhealth;
    private Enemy enemy;
    private Bullet bullet;
    private void Start()
    {
        InitializeComponents();
        initialPosition = transform.position; 
    }

    private void GameOver()
    {
        Debug.Log("Game Over");
    }

    private void InitializeComponents()
    {
        characterController = GetComponent<CharacterController>();
        plyrhealth = GetComponent<PlayerHealth>();
    }
    private void Update()
    {
        // Handle movement and other updates
        MovePlayer(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
        CheckRespawn(); // Check for respawn conditions
    }
    private void CheckRespawn()
    {
        if (plyrhealth.playerWaterLevel <= 0)
        {
            Respawn(new Vector3(0, 0, 0)); // Respawn at water point
        }
        else if (plyrhealth.playerMoneyLevel <= 0)
        {
            Respawn(new Vector3(-29.7f, 0, 8.59f)); // Respawn at money point
        }
    }

    private void Respawn(Vector3 respawnPoint)
    {
        transform.position = respawnPoint; // Set player position to respawn point
        plyrhealth.ResetHealth(); // Reset health and score
        Debug.Log("Player respawned at: " + respawnPoint);
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
    public void HandleBulletHit(GameObject target, string targetTag)
    {
        if (bullet.bulletType == BulletType.PlayerBullet && targetTag == "Enemy")
        {
            HandlePlayerBulletHit(target);
        }
        else if (bullet.bulletType == BulletType.EnemyBullet && targetTag == "Player")
        {
            HandleEnemyBulletHit(target);
        }
    }

    private void HandlePlayerBulletHit(GameObject enemy)
    {
        Enemy enemyStats = enemy.GetComponent<Enemy>();
        if (enemyStats != null)
        {
            if (enemy.name == "Duck")
            {
               /// enemyStats.enemyWaterLevel -= 30;
                //waterLevel -= 10;
                //score += 20;

                //if (enemyStats.enemyWaterLevel <= 0)
                //{
                //    Destroy(enemy);
                //    //waterLevel += 1000;
                //}
            }
        }
    }

    private void HandleEnemyBulletHit(GameObject player)
    {
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
        {
            //waterLevel -= enemy.enemyDamage;
            //score -= 50;

            
        }
    }
}
