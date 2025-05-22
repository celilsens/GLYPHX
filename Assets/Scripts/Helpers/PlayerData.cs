[System.Serializable]
public class PlayerData
{
    public float BaseDamage = 10;
    public float DamageMultiplier = 1;
    public float KnockbackPower = 1;
    public float ReloadSpeed = 1;
    public float BulletMoveSpeed = 10f;
    public float ClipSize = 1;
    public float CriticalHitRate = 0;
    public float CriticalHitMultipler = 50;

    public float BaseHealth = 50;
    public float HealthMultiplier = 1f;
    public float HealthRegen = 0f;
    public float BaseShield = 0;
    public float ShieldMultiplier = 1;
    public float ShieldRegen = 0;

    public bool Rocket = false;
    public float RocketPower = 30;
    public float RocketRadius = 1;
    public float RocketCooldown = 1;
    public bool Eraser = false;
    public float EraserCooldown = 1;
    public bool Teleport = false;
    public float TeleportCooldown = 1;

}
