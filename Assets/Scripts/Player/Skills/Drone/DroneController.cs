using UnityEngine;

public class DroneController : MonoBehaviour
{
    [Header("Drone Settings")]
    [SerializeField] private float fireCooldown = 1f;
    [SerializeField] private float detectionRadius = 10f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private Rigidbody2D playerRb;

    private float _fireCooldown => StatManager.Instance.GetFloatStat(Consts.Upgrades.DRONE_ATTACK_SPEED);
    private float _timer ;

    private void Start()
    {
        playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!GameManager.Instance.IsGameActive) return;

        _timer += Time.deltaTime;

        Transform target = FindClosestEnemy();

        if (target != null)
        {
            RotateTowards(target.position);

            if (_timer >= _fireCooldown)
            {
                FireAt(target.position);
                _timer = 0f;
            }
        }
        else
        {
            Vector2 moveDir = playerRb.linearVelocity.normalized;

            if (moveDir.sqrMagnitude > 0.01f)
            {
                RotateTowards(transform.position + (Vector3)moveDir);
            }
        }
    }

    private Transform FindClosestEnemy()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRadius);
        Transform closest = null;
        float minDist = Mathf.Infinity;

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                float dist = Vector2.Distance(transform.position, hit.transform.position);
                if (dist < minDist)
                {
                    closest = hit.transform;
                    minDist = dist;
                }
            }
        }

        return closest;
    }

    private void RotateTowards(Vector3 targetPos)
    {
        Vector2 direction = targetPos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
    }

    private void FireAt(Vector3 targetPos)
    {
        Vector2 direction = (targetPos - firePoint.position).normalized;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * bulletSpeed;
        }

        bullet.transform.up = direction;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
