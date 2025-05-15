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
    if(other.CompareTag("Boundary"))
    {
        Die();
    }
}
    public override void Die()
    {
        Debug.Log("Player Death");
        //Play Player Death Animation
        //Level End Screen
        //Destroy(gameObject);
    }
}
