using System.Collections;
using UnityEngine;

public class DroneBulletController : MonoBehaviour
{
    private float _droneDamage => StatManager.Instance.GetFloatStat(Consts.Upgrades.DRONE_POWER);


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (other.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.TakeDamage(_droneDamage);
            }

            Destroy(gameObject);
        }
        else if (other.CompareTag("Boundary"))
        {
            Destroy(gameObject);
        }
    }
}
