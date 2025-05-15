using System.Collections;
using UnityEngine;

public class Player : Damageable
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            TakeDamage(50);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Boundary"))
        {
            Die();
        }
    }
    public override void Die()
    {
        Debug.Log("Player Death");
        
        GameManager.Instance.CanPlayerMove = false;

        Rigidbody2D playerRb = GetComponent<Rigidbody2D>();

        if (playerRb != null)
        {
            playerRb.bodyType = RigidbodyType2D.Kinematic;
            playerRb.linearVelocity = Vector2.zero;

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

}
