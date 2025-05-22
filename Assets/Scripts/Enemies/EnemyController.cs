using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float separationDistance = 1.5f;
    public float separationStrength = 1.5f;

    private Transform _playerTransform;
    private Rigidbody2D _rb;

    private void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Vector2 directionToPlayer = (_playerTransform.position - transform.position).normalized;

        Vector2 separationForce = Vector2.zero;
        Collider2D[] nearbyEnemies = Physics2D.OverlapCircleAll(transform.position, separationDistance);

        foreach (Collider2D other in nearbyEnemies)
        {
            if (other.gameObject == gameObject || !other.CompareTag("Enemy")) continue;

            Vector2 away = transform.position - other.transform.position;
            float distance = away.magnitude;

            if (distance > 0)
            {
                separationForce += away.normalized / distance;
            }
        }

        Vector2 finalDirection = (directionToPlayer + separationForce * separationStrength).normalized;

        if (GameManager.Instance.IsGameActive)
        {
            _rb.MovePosition(_rb.position + finalDirection * moveSpeed * Time.fixedDeltaTime);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, separationDistance);
    }
}
