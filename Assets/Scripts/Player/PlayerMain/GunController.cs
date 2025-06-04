using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private float _distanceFromPlayer = 1f;
    [SerializeField] private float _rotationOffset = -90f;

    private void Update()
    {
        if (!GameManager.Instance.IsGameActive) return;

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = (mouseWorldPos - _playerTransform.position).normalized;
        direction.z = 0;

        UpdateGunPosition(direction);
        UpdateGunRotation(direction);
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

}
