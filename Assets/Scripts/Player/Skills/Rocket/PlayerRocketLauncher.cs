using UnityEngine;

public class PlayerRocketLauncher : MonoBehaviour
{
    [SerializeField] private GameObject _rocketPrefab;
    [SerializeField] private Transform _firePoint;

    private float _rocketCooldown => StatManager.Instance.GetFloatStat(Consts.Upgrades.ROCKET_COOLDOWN);
    private float _timer;

    private void Start()
    {
        _timer = _rocketCooldown;
    }

    private void Update()
    {
        if (!GameManager.Instance.IsGameActive) return;
        if (!StatManager.Instance.GetBoolStat(Consts.Upgrades.ROCKET)) return;

        _timer += Time.deltaTime;

        if (Input.GetMouseButtonDown(1) && _timer >= _rocketCooldown)
        {
            LaunchRocket();
            _timer = 0f;
        }
    }

    private void LaunchRocket()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;

        Vector3 direction = (mouseWorldPos - _firePoint.position).normalized;

        GameObject rocket = Instantiate(_rocketPrefab, _firePoint.position, Quaternion.identity);
        Rigidbody2D rb = rocket.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = direction * 10f;
        }

        rocket.transform.up = direction;
    }
}