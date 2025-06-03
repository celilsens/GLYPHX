using UnityEngine;

public class PlayerBulletLauncher : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _firePoint;

    private float _bulletSpeed => StatManager.Instance.GetFloatStat(Consts.Upgrades.BULLET_MOVE_SPEED);
    private float _fireCooldown => StatManager.Instance.GetFloatStat(Consts.Upgrades.RELOAD_SPEED);
    private float _timer;

    private void Start()
    {
        _timer = _fireCooldown;
    }

    private void Update()
    {
        if (!GameManager.Instance.IsGameActive) return;

        _timer += Time.deltaTime;

        if (Input.GetMouseButton(0) && _timer >= _fireCooldown)
        {
            Fire();
            _timer = 0f;
        }
    }

    private void Fire()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;

        Vector3 direction = (mouseWorldPos - _firePoint.position).normalized;

        GameObject bullet = Instantiate(_bulletPrefab, _firePoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = direction * _bulletSpeed;
        }

        bullet.transform.up = direction;
    }
}