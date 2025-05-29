using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private float _distanceFromPlayer = 1f;
    [SerializeField] private float _rotationOffset = -90f;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private float _bulletSpeed  => StatManager.Instance.GetFloatStat(Consts.Upgrades.BULLET_MOVE_SPEED);
    [SerializeField] private float _bulletSpawnOffset = 4f;
    [SerializeField] private float _fireCooldown => StatManager.Instance.GetFloatStat(Consts.Upgrades.RELOAD_SPEED);
    private float _lastShotTime = 0f;

    private void Update()
    {
        if (GameManager.Instance.IsGameActive)
        {
            Vector3 mouseWorldPos = GetMouseWorldPosition();
            Vector3 direction = CalculateDirection(mouseWorldPos);
            UpdateGunPosition(direction);
            UpdateGunRotation(direction);
            HandleShooting(direction);
        }

    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Mathf.Abs(Camera.main.transform.position.z - _playerTransform.position.z);
        return Camera.main.ScreenToWorldPoint(mouseScreenPos);
    }

    private Vector3 CalculateDirection(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - _playerTransform.position;
        direction.z = 0;
        return direction.normalized;
    }

    private void UpdateGunPosition(Vector3 direction)
    {
        transform.position = _playerTransform.position + direction * _distanceFromPlayer;
    }

    private void UpdateGunRotation(Vector3 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle + _rotationOffset);
    }

    private void HandleShooting(Vector3 shootDirection)
    {
        if (Input.GetMouseButton(0) && Time.time >= _lastShotTime + _fireCooldown)
        {
            ShootBullet(shootDirection);
            _lastShotTime = Time.time;
        }
    }

    private void ShootBullet(Vector3 shootDirection)
    {
        Vector3 spawnPosition = transform.position + shootDirection * _bulletSpawnOffset;
        GameObject bullet = Instantiate(_bulletPrefab, spawnPosition, transform.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = shootDirection * _bulletSpeed;
        }
    }
}
