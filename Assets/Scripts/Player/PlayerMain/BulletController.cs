using System.Collections;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private float _damage => StatManager.Instance.GetFloatStat(Consts.Upgrades.BASE_DAMAGE) * StatManager.Instance.GetFloatStat(Consts.Upgrades.DAMAGE_MULTIPLIER);
    private float _critChance => StatManager.Instance.GetFloatStat(Consts.Upgrades.CRITICAL_HIT_RATE);
    private float _critMultiplier => StatManager.Instance.GetFloatStat(Consts.Upgrades.CRITICAL_HIT_MULTIPLIER);
    private float _knockbackPower => StatManager.Instance.GetFloatStat(Consts.Upgrades.KNOCKBACK_POWER);

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (other.TryGetComponent<IDamageable>(out var damageable))
            {
                if (other.TryGetComponent<EnemyController>(out var enemyController))
                {
                    Vector2 knockbackDir = (other.transform.position - transform.position).normalized;
                    enemyController.ApplyKnockback(knockbackDir, _knockbackPower, 0.2f);
                }

                damageable.TakeDamage(CalculateDamage());
            }

            Destroy(gameObject);
        }
        else if (other.CompareTag("Boundary"))
        {
            Destroy(gameObject);
        }
    }


    private float CalculateDamage()
    {
        float damage = _damage;
        if (Random.value <= _critChance)
        {
            damage *= _critMultiplier;
        }
        return damage;
    }
}
