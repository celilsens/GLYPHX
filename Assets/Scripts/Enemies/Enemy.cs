using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private int _rewardAmount = 10;



    public void TakeDamage(float damage)
    {
        //EnemyTakeDamage
    }

    public void Die()
    {
        //TODO: Play Enemy Death Animation
        GameManager.Instance.AddMoney(_rewardAmount);
        Destroy(gameObject);
    }
}
