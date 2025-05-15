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
        ParticleSystem effect = GetComponentInChildren<ParticleSystem>();
        if (effect != null)
        {
            effect.Play();
        }
        //TODO: Level End Screen
        //TODO-Active: Destroy(gameObject);
    }
}
