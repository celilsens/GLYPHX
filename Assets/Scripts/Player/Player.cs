using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    public float MaxPlayerHealth { get; private set; }
    public float CurrentPlayerHealth { get; private set; }
    public float MaxPlayerShield { get; private set; }
    public float CurrentPlayerShield { get; private set; }

    private void Start()
    {
        GameManager gm = GameManager.Instance;
        MaxPlayerHealth = gm.PlayerMaxHealth;
        MaxPlayerShield = gm.PlayerMaxShield;
        CurrentPlayerHealth = MaxPlayerHealth;
        CurrentPlayerShield = MaxPlayerShield;

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
        Debug.Log("Player Dead");

        GameManager.Instance.ChangeGameStatus(false);

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.linearVelocity = Vector2.zero;
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
            TakeDamage(50f);
        }

        if (collision.CompareTag("Boundary"))
        {
            TakeDamage(500000f);
        }
    }

}
