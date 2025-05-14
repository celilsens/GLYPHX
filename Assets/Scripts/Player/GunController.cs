using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float distanceFromPlayer = 1f;
    [SerializeField] private float rotationOffset = -90f;

    void Update()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Mathf.Abs(Camera.main.transform.position.z - player.position.z);
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);

        Vector3 direction = mouseWorldPos - player.position;
        direction.z = 0;
        direction.Normalize();

        transform.position = player.position + direction * distanceFromPlayer;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle + rotationOffset);
    }
}
