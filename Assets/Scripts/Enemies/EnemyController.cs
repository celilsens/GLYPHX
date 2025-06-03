using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 3f;
    public float separationDistance = 1.5f;
    public float separationStrength = 1.5f;

    private Transform _playerTransform;
    private Rigidbody2D _rb;

    private void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (!GameManager.Instance.IsGameActive) return;

        Vector2 moveDirection = CalculateMovementDirection();
        MoveEnemy(moveDirection);
    }

    private Vector2 CalculateMovementDirection()
    {
        Vector2 toPlayer = (_playerTransform.position - transform.position).normalized;
        Vector2 separation = CalculateSeparationForce();

        return (toPlayer + separation * separationStrength).normalized;
    }

    private Vector2 CalculateSeparationForce()
    {
        Vector2 separationForce = Vector2.zero;
        Collider2D[] nearbyEnemies = Physics2D.OverlapCircleAll(transform.position, separationDistance);

        foreach (Collider2D other in nearbyEnemies)
        {
            if (other.gameObject == gameObject || !other.CompareTag("Enemy")) continue;

            Vector2 away = transform.position - other.transform.position;
            float distance = away.magnitude;

            if (distance > 0)
                separationForce += away.normalized / distance;
        }

        return separationForce;
    }

    private void MoveEnemy(Vector2 direction)
    {
        _rb.MovePosition(_rb.position + direction * moveSpeed * Time.fixedDeltaTime);
    }
}
