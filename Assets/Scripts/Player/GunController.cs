using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float distanceFromPlayer = 1f;
    [SerializeField] private float rotationOffset = -90f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float bulletSpawnOffset = 1f;

    void Update()
    {
        Vector3 mouseWorldPos = GetMouseWorldPosition();
        Vector3 direction = CalculateDirection(mouseWorldPos);

        UpdateGunPosition(direction);
        UpdateGunRotation(direction);
        HandleShooting(direction);
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Mathf.Abs(Camera.main.transform.position.z - player.position.z);
        return Camera.main.ScreenToWorldPoint(mouseScreenPos);
    }

    private Vector3 CalculateDirection(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - player.position;
        direction.z = 0;
        return direction.normalized;
    }

    private void UpdateGunPosition(Vector3 direction)
    {
        transform.position = player.position + direction * distanceFromPlayer;
    }

    private void UpdateGunRotation(Vector3 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle + rotationOffset);
    }


    private void HandleShooting(Vector3 shootDirection)
    {
        if (Input.GetMouseButton(0))
        {
            ShootBullet(shootDirection);
        }
    }

    private void ShootBullet(Vector3 shootDirection)
{
    
    Vector3 spawnPosition = transform.position + shootDirection * bulletSpawnOffset;
    
    GameObject bullet = Instantiate(bulletPrefab, spawnPosition, transform.rotation);
    Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

    if (rb != null)
    {
        rb.linearVelocity = shootDirection * bulletSpeed;
    }
}
}
