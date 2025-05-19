using UnityEngine;

public class PlayerBulletController : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _xRange = 55f;
    [SerializeField] private float _yRange = 40f;
    private float _currentSpeed;
    private Rigidbody2D _bulletRb;

    private void Awake()
    {
        _bulletRb = GetComponent<Rigidbody2D>();
        _currentSpeed = _speed;
    }

    private void Start()
    {
        _bulletRb.linearVelocity = transform.up * _speed;
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance != null && !GameManager.Instance.IsGameActive)
        {
            _bulletRb.linearVelocity = Vector2.zero;
        }
        else
        {
            _bulletRb.linearVelocity = transform.up * _currentSpeed;
        }
    }

    private void Update()
    {
        DestroyOutOfBounds();
    }

    private void DestroyOutOfBounds()
    {
        Vector2 position = transform.position;

        if (Mathf.Abs(position.x) > _xRange || Mathf.Abs(position.x) < -_xRange)
        {
            Destroy(gameObject);
        }
        else if (Mathf.Abs(position.y) > _yRange || Mathf.Abs(position.y) < -_yRange)
        {
            Destroy(gameObject);
        }
    }
}
