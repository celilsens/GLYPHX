using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    public float MaxPlayerHealth { get; private set; }
    public float CurrentPlayerHealth { get; private set; }
    public float MaxPlayerShield { get; private set; }
    public float CurrentPlayerShield { get; private set; }

    private Rigidbody2D _playerRB;

    private float _knockbackForce = 50f;

    private void Start()
    {
        StartCoroutine(WaitAndLoadStats());
        StartCoroutine(RegenCoroutine());

        _playerRB = GetComponent<Rigidbody2D>();

        Debug.Log("Player Health is: " + MaxPlayerHealth);
        Debug.Log("Player Shield is: " + MaxPlayerShield);
    }

    private void LoadStatsFromStatManager()
    {
        MaxPlayerHealth = StatManager.Instance.GetFloatStat(Consts.Upgrades.BASE_HEALTH) * StatManager.Instance.GetFloatStat(Consts.Upgrades.HEALTH_MULTIPLIER);
        CurrentPlayerHealth = MaxPlayerHealth;

        MaxPlayerShield = StatManager.Instance.GetFloatStat(Consts.Upgrades.BASE_SHIELD) * StatManager.Instance.GetFloatStat(Consts.Upgrades.SHIELD_MULTIPLIER);
        CurrentPlayerShield = MaxPlayerShield;

    }

    private IEnumerator WaitAndLoadStats()
    {
        yield return new WaitUntil(() => StatManager.Instance != null);

        LoadStatsFromStatManager();
        Debug.Log("Player Health is: " + MaxPlayerHealth);
    }

    private IEnumerator RegenCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            float healthRegen = StatManager.Instance.GetFloatStat(Consts.Upgrades.HEALTH_REGEN);
            float shieldRegen = StatManager.Instance.GetFloatStat(Consts.Upgrades.SHIELD_REGEN);

            if (CurrentPlayerHealth < MaxPlayerHealth)
            {
                CurrentPlayerHealth = Mathf.Min(CurrentPlayerHealth + healthRegen, MaxPlayerHealth);
            }

            if (CurrentPlayerShield < MaxPlayerShield)
            {
                CurrentPlayerShield = Mathf.Min(CurrentPlayerShield + shieldRegen, MaxPlayerShield);
            }
        }
    }


    public void TakeDamage(float damage)
    {
        float remainingDamage = damage;

        if (CurrentPlayerShield > 0f)
        {
            if (CurrentPlayerShield >= remainingDamage)
            {
                CurrentPlayerShield -= remainingDamage;
                remainingDamage = 0f;
            }
            else
            {
                remainingDamage -= CurrentPlayerShield;
                CurrentPlayerShield = 0f;
            }
        }

        if (remainingDamage > 0f)
        {
            CurrentPlayerHealth -= remainingDamage;
            if (CurrentPlayerHealth <= 0f)
            {
                CurrentPlayerHealth = 0f;
                Die();
            }
        }
    }

    public void Die()
    {
        GameManager.Instance.ChangeGameStatus(false);

        if (_playerRB != null)
        {
            _playerRB.bodyType = RigidbodyType2D.Kinematic;
            _playerRB.linearVelocity = Vector2.zero;
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
            GameManager.Instance.GameOver();
        }
    }

    private IEnumerator WaitForEffectToEnd(ParticleSystem effect)
    {
        yield return new WaitWhile(() => effect.isPlaying);
        GameManager.Instance.GameOver();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Vector2 knockbackDirection = (transform.position - collision.transform.position).normalized;
            Knockback(knockbackDirection, _knockbackForce);
            TakeDamage(50f);
        }

        if (collision.CompareTag("Boundary"))
        {
            TakeDamage(500000f);
        }
    }

    private void Knockback(Vector2 direction, float force)
    {
        _playerRB.AddForce(direction * force, ForceMode2D.Impulse);
    }

}
