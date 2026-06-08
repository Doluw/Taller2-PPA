using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EnemyController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private string bulletTag = "Bullet";
    [SerializeField] private Animator animator;
    [SerializeField] private string moveAnimationParameter = "IsMoving";

    private bool isDead;

    private void Awake()
    {
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
    }

    private void Start()
    {
        if (animator != null && !string.IsNullOrEmpty(moveAnimationParameter))
        {
            animator.SetBool(moveAnimationParameter, true);
        }
    }

    private void Update()
    {
        if (isDead)
        {
            return;
        }

        transform.Translate(Vector3.back * moveSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        HandleHit(other.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        HandleHit(collision.gameObject);
    }

    private void HandleHit(GameObject other)
    {
        if (isDead)
        {
            return;
        }

        if (other.CompareTag(playerTag))
        {
            PlayerController player = other.GetComponent<PlayerController>();

            if (player != null)
            {
                player.Die();
            }

            return;
        }

        if (other.CompareTag(bulletTag))
        {
            Destroy(other);
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        if (animator != null && !string.IsNullOrEmpty(moveAnimationParameter))
        {
            animator.SetBool(moveAnimationParameter, false);
        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayEnemyDeath();
        }

        Destroy(gameObject);
    }
}
