using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CoinController : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";

    private bool collected;

    private void Awake()
    {
        Collider coinCollider = GetComponent<Collider>();
        coinCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        TryCollect(other.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        TryCollect(collision.gameObject);
    }

    private void TryCollect(GameObject other)
    {
        if (collected || !other.CompareTag(playerTag))
        {
            return;
        }

        collected = true;

        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddCoin();
        }

        Destroy(gameObject);
    }
}
