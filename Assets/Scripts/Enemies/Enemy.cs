using UnityEngine;
using DG.Tweening;
using System.Collections;
using System;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private int _rewardAmount = 10;
    [SerializeField] private float _maxHealth = 100f;
    [SerializeField] private float _currentHealth;
    [SerializeField] private Transform _fillTransform;

    public event Action OnEnemyDeath;

    private void Start()
    {
        _currentHealth = _maxHealth;
        UpdateFill();
    }

    public void TakeDamage(float damage)
    {
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
        OnEnemyDeath?.Invoke();

        foreach (var collider in GetComponents<Collider2D>())
        {
            collider.enabled = false;
        }

        foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>())
        {
            sr.enabled = false;
        }

        ParticleSystem effect = GetComponentInChildren<ParticleSystem>();

        if (effect != null)
        {
            effect.Play();
            StartCoroutine(WaitForEffectToEnd(effect));
        }

        else
        {
            GameManager.Instance.AddMoney(_rewardAmount);
            Destroy(gameObject);
        }
    }

    private IEnumerator WaitForEffectToEnd(ParticleSystem effect)
    {
        yield return new WaitWhile(() => effect.isPlaying);
        GameManager.Instance.AddMoney(_rewardAmount);
        Destroy(gameObject);
    }

    private void UpdateFill()
    {
        float scaleX = _currentHealth / _maxHealth;
        _fillTransform.localScale = new Vector3(scaleX, _fillTransform.localScale.y, _fillTransform.localScale.z);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            //Debug.Log(gameObject.name + ": Took Damage." + "Current Health: " + _currentHealth);
            TakeDamage(GameManager.Instance.PlayerDamage);
            Destroy(other.gameObject);
        }
    }
}
