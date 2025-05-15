using UnityEngine;

public class Enemy : Damageable
{
    [SerializeField] private int _rewardAmount = 10;
    public override void Die()
    {
        //TODO: Play Enemy Death Animation
        GameManager.Instance.AddMoney(_rewardAmount);
        Destroy(gameObject);
    }
}
