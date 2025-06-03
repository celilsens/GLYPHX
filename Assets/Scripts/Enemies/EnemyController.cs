using System.Collections;
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

    private bool isKnockedBack = false;
    private Vector2 knockbackVelocity;

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
        Vector2 finalVelocity = isKnockedBack ? knockbackVelocity : direction * moveSpeed;
        _rb.MovePosition(_rb.position + finalVelocity * Time.fixedDeltaTime);
    }


    public void ApplyKnockback(Vector2 direction, float force, float duration)
    {
        if (!isKnockedBack)
            StartCoroutine(KnockbackRoutine(direction, force, duration));
    }

    private IEnumerator KnockbackRoutine(Vector2 direction, float force, float duration)
    {
        isKnockedBack = true;
        knockbackVelocity = direction * force;

        yield return new WaitForSeconds(duration);

        isKnockedBack = false;
        knockbackVelocity = Vector2.zero;
    }


}
