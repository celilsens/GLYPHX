using UnityEngine;
using System.Collections;
using Unity.Cinemachine;

public class PlayerEraserSkill : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;

    private float _eraserCooldown => StatManager.Instance.GetFloatStat(Consts.Upgrades.ERASER_COOLDOWN);
    private float _eraserDamage => StatManager.Instance.GetFloatStat(Consts.Upgrades.BASE_DAMAGE);
    [SerializeField] private CinemachineImpulseSource impulseSource;
    private float _timer;

    private void Start()
    {
        _timer = _eraserCooldown;
    }

    private void Update()
    {
        if (!GameManager.Instance.IsGameActive) return;
        if (!StatManager.Instance.GetBoolStat(Consts.Upgrades.ERASER)) return;

        _timer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.E) && _timer >= _eraserCooldown)
        {
            StartCoroutine(UseEraser());
            _timer = 0f;
        }
    }

    private IEnumerator UseEraser()
    {
        Debug.Log("Eraser activated");

        TriggerImpulse();

        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach (var enemy in enemies)
        {
            enemy._enemyController.enabled = false;
        }

        yield return new WaitForSeconds(1f);

        foreach (var enemy in enemies)
        {
            enemy.TakeDamage(_eraserDamage);
            enemy._enemyController.enabled = true;
        }
    }


    private void TriggerImpulse()
    {
        if (impulseSource != null)
            impulseSource.GenerateImpulse();
    }
}
