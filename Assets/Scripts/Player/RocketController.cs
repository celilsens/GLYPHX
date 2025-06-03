using System.Collections;
using UnityEngine;

public class RocketController : MonoBehaviour
{
    private float _power => StatManager.Instance.GetFloatStat(Consts.Upgrades.ROCKET_POWER);
    private float _radius => StatManager.Instance.GetFloatStat(Consts.Upgrades.ROCKET_RADIUS);
    [SerializeField] private GameObject _explosionEffect;

    private bool _hasExploded = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_hasExploded) return;

        if (other.CompareTag("Enemy"))
        {
            Explode();
        }

        if (other.CompareTag("Boundary"))
        {
            Destroy(gameObject);
        }
    }

    private void Explode()
    {
        _hasExploded = true;

        foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>())
        {
            sr.enabled = false;
        }

        ParticleSystem effect = GetComponentInChildren<ParticleSystem>();

        if (_explosionEffect != null)
        {
            GameObject fx = Instantiate(_explosionEffect, transform.position, Quaternion.identity);
            ParticleSystem ps = fx.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                Debug.Log("Effect instantiated and playing");
                ps.Play();
                StartCoroutine(WaitForEffectToEnd(ps, fx));
            }
            else
            {
                CalculateDamage();
                Destroy(gameObject);
            }
        }
        else
        {
            CalculateDamage();
            Destroy(gameObject);
        }
    }

    private IEnumerator WaitForEffectToEnd(ParticleSystem effect, GameObject fx)
    {
        yield return new WaitWhile(() => effect.isPlaying);
        CalculateDamage();
        Destroy(fx);
        Destroy(gameObject);
    }

    private void CalculateDamage()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _radius);

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy") && hit.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.TakeDamage(_power);
            }
        }
    }
}
