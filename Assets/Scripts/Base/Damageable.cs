using UnityEngine;

public abstract class Damageable : MonoBehaviour
{
    public int health = 100;
    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }
    public abstract void Die();
}
