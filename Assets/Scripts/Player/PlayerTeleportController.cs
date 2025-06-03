using UnityEngine;

public class PlayerTeleportController : MonoBehaviour
{
    [SerializeField] private float teleportRange = 10f;
    [SerializeField] private KeyCode teleportKey = KeyCode.Space;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Transform boundaryCenter;
    [SerializeField] private float boundaryRadius = 15f;

    private float _teleportCooldown => StatManager.Instance.GetFloatStat(Consts.Upgrades.TELEPORT_COOLDOWN);
    private float _timer;

    private void Update()
    {
        if (!GameManager.Instance.IsGameActive) return;
        if (!StatManager.Instance.GetBoolStat(Consts.Upgrades.TELEPORT)) return;

        _timer += Time.deltaTime;

        if (Input.GetKeyDown(teleportKey) && _timer >= _teleportCooldown)
        {
            AttemptTeleport();
            _timer = 0f;
        }
    }

    private void AttemptTeleport()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;

        float distance = Vector3.Distance(transform.position, mouseWorldPos);

        Vector3 targetPosition = distance <= teleportRange
            ? mouseWorldPos
            : transform.position + (mouseWorldPos - transform.position).normalized * teleportRange;

        if (IsPositionValid(targetPosition))
        {
            transform.position = targetPosition;
        }
    }

    private bool IsPositionValid(Vector3 position)
    {
        Collider2D enemyHit = Physics2D.OverlapCircle(position, 0.5f, enemyLayer);
        if (enemyHit != null) return false;

        float distanceFromCenter = Vector3.Distance(position, boundaryCenter.position);
        return distanceFromCenter <= boundaryRadius;
    }
}
