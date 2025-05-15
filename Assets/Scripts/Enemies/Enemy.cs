using UnityEngine;

public class Enemy : Damageable
{
    public int rewardAmount = 10;
    public override void Die()
    {
        //Play Enemy Death Animation
        //GameManager.Instance.AddMoney(rewardAmount);
        Destroy(gameObject);
    }
}
