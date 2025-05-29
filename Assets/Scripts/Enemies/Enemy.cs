using UnityEngine;
using DG.Tweening;
using System.Collections;
using System;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private float _rewardAmount = 100f;
    [SerializeField] private float _maxHealth = 100f;
    [SerializeField] private float _currentHealth;
    [SerializeField] private Transform _fillTransform;

    public event Action OnEnemyDeath;

    private bool isDead = false;

    private void Start()
    {
        _currentHealth = _maxHealth;
        UpdateFill();
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        _currentHealth -= damage;

        if (_currentHealth > 0)
        {
            float targetScale = _currentHealth / _maxHealth;
            DOTween.To(() => _fillTransform.localScale.x,
                       x => _fillTransform.localScale = new Vector3(x, x, _fillTransform.localScale.z),
                       targetScale, 0.5f);
        }
        else
        {
            Die();
        }
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        foreach (var collider in GetComponents<Collider2D>())
            collider.enabled = false;

        foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>())
            sr.enabled = false;

        ParticleSystem effect = GetComponentInChildren<ParticleSystem>();

        if (effect != null)
        {
            effect.Play();
            StartCoroutine(WaitForEffectToEnd(effect));
        }
        else
        {
            DestroyEnemy();
        }
    }

    private IEnumerator WaitForEffectToEnd(ParticleSystem effect)
    {
        yield return new WaitWhile(() => effect.isPlaying);
        DestroyEnemy();
    }

    private void DestroyEnemy()
    {
        GameManager.Instance.AddMoney(_rewardAmount);
        OnEnemyDeath?.Invoke();
        OnEnemyDeath = null;
        Destroy(gameObject);
    }

    private void UpdateFill()
    {
        float scaleX = _currentHealth / _maxHealth;
        _fillTransform.localScale = new Vector3(scaleX, _fillTransform.localScale.y, _fillTransform.localScale.z);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return;

        if (other.CompareTag("Bullet"))
        {
            float baseDamage = StatManager.Instance.GetFloatStat(Consts.Upgrades.BASE_DAMAGE);
            float damageMultiplier = StatManager.Instance.GetFloatStat(Consts.Upgrades.DAMAGE_MULTIPLIER);
            float criticalHitRate = StatManager.Instance.GetFloatStat(Consts.Upgrades.CRITICAL_HIT_RATE);
            float criticalHitMultiplier = StatManager.Instance.GetFloatStat(Consts.Upgrades.CRITICAL_HIT_MULTIPLIER);

            float damageTaken = baseDamage * damageMultiplier;

            if (UnityEngine.Random.value <= criticalHitRate)
            {
                damageTaken *= criticalHitMultiplier;
            }

            TakeDamage(damageTaken);
            Destroy(other.gameObject);
        }
    }
}
