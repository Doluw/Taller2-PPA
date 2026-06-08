using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Forward Movement")]
    [SerializeField] private float forwardSpeed = 8f;

    [Header("Lane Movement")]
    [SerializeField] private float laneDistance = 3f;
    [SerializeField] private float laneChangeSpeed = 12f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.25f;
    [SerializeField] private LayerMask groundMask = ~0;

    [Header("Shooting")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireCooldown = 0.5f;

    private Rigidbody rb;
    private Collider playerCollider;
    private int currentLane = 1;
    private float nextFireTime;
    private bool isDead;

    public bool IsDead => isDead;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<Collider>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        if (isDead)
        {
            return;
        }

        HandleLaneInput();
        HandleJumpInput();
        HandleShootInput();
    }

    private void FixedUpdate()
    {
        if (isDead)
        {
            return;
        }

        MovePlayer();
    }

    private void HandleLaneInput()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            currentLane = Mathf.Max(0, currentLane - 1);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            currentLane = Mathf.Min(2, currentLane + 1);
        }
    }

    private void HandleJumpInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            Vector3 velocity = rb.velocity;
            velocity.y = 0f;
            rb.velocity = velocity;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void HandleShootInput()
    {
        if (!Input.GetKeyDown(KeyCode.F) || Time.time < nextFireTime || projectilePrefab == null)
        {
            return;
        }

        Transform spawnPoint = firePoint != null ? firePoint : transform;
        Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation);
        nextFireTime = Time.time + fireCooldown;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayShoot();
        }
    }

    private void MovePlayer()
    {
        float targetX = (currentLane - 1) * laneDistance;
        Vector3 position = rb.position;
        float newX = Mathf.Lerp(position.x, targetX, laneChangeSpeed * Time.fixedDeltaTime);

        Vector3 nextPosition = new Vector3(
            newX,
            position.y,
            position.z + forwardSpeed * Time.fixedDeltaTime
        );

        rb.MovePosition(nextPosition);
    }

    private bool IsGrounded()
    {
        Vector3 checkPosition;

        if (groundCheck != null)
        {
            checkPosition = groundCheck.position;
        }
        else if (playerCollider != null)
        {
            Bounds bounds = playerCollider.bounds;
            checkPosition = new Vector3(
                bounds.center.x,
                bounds.min.y + groundCheckRadius * 0.5f,
                bounds.center.z
            );
        }
        else
        {
            checkPosition = transform.position + Vector3.down * 0.55f;
        }

        return Physics.CheckSphere(
            checkPosition,
            groundCheckRadius,
            groundMask,
            QueryTriggerInteraction.Ignore
        );
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsDeadly(other.gameObject))
        {
            Die();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (IsDeadly(collision.gameObject))
        {
            Die();
        }
    }

    private bool IsDeadly(GameObject other)
    {
        return other.CompareTag("Enemy") || other.CompareTag("Obstacle");
    }

    public void Die()
    {
        if (isDead)
        {
            return;
        }

        isDead = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameOver();
        }
    }
}
