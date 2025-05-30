using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed => StatManager.Instance.GetFloatStat(Consts.Upgrades.SHIP_SPEED);
    [SerializeField] private float _smoothTime = 0.1f;
    [SerializeField] private float _angularSpeed = 500f;
    private float _storedAngularVelocity;
    private Vector2 _storedVelocity;

    private Rigidbody2D _playerRb;
    private Vector2 currentVelocity = Vector2.zero;
    private Vector2 inputDirection;
    private Vector2 lastMovement;

    private void Awake()
    {
        _playerRb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        HandleInputs();
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.IsGameActive)
        {
            if (_storedVelocity != Vector2.zero || _storedAngularVelocity != 0f)
            {
                _playerRb.linearVelocity = _storedVelocity;
                _playerRb.angularVelocity = _storedAngularVelocity;

                _storedVelocity = Vector2.zero;
                _storedAngularVelocity = 0f;
            }

            HandleMovement();
            HandleRotation();
        }
        else
        {
            if (_playerRb.linearVelocity != Vector2.zero || _playerRb.angularVelocity != 0f)
            {
                _storedVelocity = _playerRb.linearVelocity;
                _storedAngularVelocity = _playerRb.angularVelocity;

                _playerRb.linearVelocity = Vector2.zero;
                _playerRb.angularVelocity = 0f;
            }
        }
    }

    private void HandleInputs()
    {
        inputDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
    }

    private void HandleMovement()
    {
        Vector2 targetVelocity = inputDirection * _moveSpeed;
        _playerRb.linearVelocity = Vector2.SmoothDamp(_playerRb.linearVelocity, targetVelocity, ref currentVelocity, _smoothTime);
    }

    private void HandleRotation()
    {
        if (_playerRb.linearVelocity.sqrMagnitude > 0.001f)
        {
            lastMovement = _playerRb.linearVelocity.normalized;
        }

        float targetAngle = Mathf.Atan2(lastMovement.y, lastMovement.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _angularSpeed * Time.fixedDeltaTime);
    }
}
