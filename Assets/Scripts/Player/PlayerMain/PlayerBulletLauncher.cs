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
        Vector3 right = Vector3.Cross(direction, Vector3.forward);

        bool isTriple = StatManager.Instance.GetBoolStat(Consts.Upgrades.TRIPLE_FIRE);
        bool isDouble = StatManager.Instance.GetBoolStat(Consts.Upgrades.DOUBLE_FIRE);

        if (isTriple)
        {
            FireBulletAtOffset(_firePoint.position - right * 0.3f, direction);
            FireBulletAtOffset(_firePoint.position, direction);
            FireBulletAtOffset(_firePoint.position + right * 0.3f, direction);
        }
        else if (isDouble)
        {
            FireBulletAtOffset(_firePoint.position - right * 0.2f, direction);
            FireBulletAtOffset(_firePoint.position + right * 0.2f, direction);
        }
        else
        {
            FireBulletAtOffset(_firePoint.position, direction);
        }
    }

    private void FireBulletAtOffset(Vector3 position, Vector3 direction)
    {
        GameObject bullet = Instantiate(_bulletPrefab, position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = direction * _bulletSpeed;
        }

        bullet.transform.up = direction;
    }

}