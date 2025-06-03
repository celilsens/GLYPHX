using UnityEngine;

public class StatChecker : MonoBehaviour
{
    [SerializeField] private float _baseDamage;
    [SerializeField] private float _damageMultiplier;
    [SerializeField] private float _knockbackPower;
    [SerializeField] private float _reloadSpeed;
    [SerializeField] private float _bulletMoveSpeed;
    [SerializeField] private float _criticalHitRate;
    [SerializeField] private float _criticalHitMultiplier;
    [SerializeField] private float _baseHealth;
    [SerializeField] private float _healthMultiplier;
    [SerializeField] private float _healthRegen;
    [SerializeField] private float _baseShield;
    [SerializeField] private float _shieldMultiplier;
    [SerializeField] private float _shieldRegen;
    [SerializeField] private bool _rocket;
    [SerializeField] private float _rocketPower;
    [SerializeField] private float _rocketRadius;
    [SerializeField] private float _rocketCooldown;
    [SerializeField] private bool _eraserEnabled;
    [SerializeField] private float _eraserCooldown;
    [SerializeField] private bool _teleportEnabled;
    [SerializeField] private float _teleportCooldown;
    [SerializeField] private float _shipSpeed;
    [SerializeField] private float _moneyMultiplier;
    [SerializeField] private float _droneCount;
    [SerializeField] private float _dronePower;
    [SerializeField] private float _droneAttackSpeed;
    [SerializeField] private float _doubleFire;
    [SerializeField] private float _tripleFire;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            GetUpgradeData();
        }
    }

    private void GetUpgradeData()
    {
        _baseDamage = StatManager.Instance.GetFloatStat(Consts.Upgrades.BASE_DAMAGE);
        _damageMultiplier = StatManager.Instance.GetFloatStat(Consts.Upgrades.DAMAGE_MULTIPLIER);
        _knockbackPower = StatManager.Instance.GetFloatStat(Consts.Upgrades.KNOCKBACK_POWER);
        _reloadSpeed = StatManager.Instance.GetFloatStat(Consts.Upgrades.RELOAD_SPEED);
        _bulletMoveSpeed = StatManager.Instance.GetFloatStat(Consts.Upgrades.BULLET_MOVE_SPEED);
        _criticalHitRate = StatManager.Instance.GetFloatStat(Consts.Upgrades.CRITICAL_HIT_RATE);
        _criticalHitMultiplier = StatManager.Instance.GetFloatStat(Consts.Upgrades.CRITICAL_HIT_MULTIPLIER);
        _baseHealth = StatManager.Instance.GetFloatStat(Consts.Upgrades.BASE_HEALTH);
        _healthMultiplier = StatManager.Instance.GetFloatStat(Consts.Upgrades.HEALTH_MULTIPLIER);
        _healthRegen = StatManager.Instance.GetFloatStat(Consts.Upgrades.HEALTH_REGEN);
        _baseShield = StatManager.Instance.GetFloatStat(Consts.Upgrades.BASE_SHIELD);
        _shieldMultiplier = StatManager.Instance.GetFloatStat(Consts.Upgrades.SHIELD_MULTIPLIER);
        _shieldRegen = StatManager.Instance.GetFloatStat(Consts.Upgrades.SHIELD_REGEN);
        _rocket = StatManager.Instance.GetBoolStat(Consts.Upgrades.ROCKET);
        _rocketPower = StatManager.Instance.GetFloatStat(Consts.Upgrades.ROCKET_POWER);
        _rocketRadius = StatManager.Instance.GetFloatStat(Consts.Upgrades.ROCKET_RADIUS);
        _rocketCooldown = StatManager.Instance.GetFloatStat(Consts.Upgrades.ROCKET_COOLDOWN);
        _eraserEnabled = StatManager.Instance.GetBoolStat(Consts.Upgrades.ERASER);
        _eraserCooldown = StatManager.Instance.GetFloatStat(Consts.Upgrades.ERASER_COOLDOWN);
        _teleportEnabled = StatManager.Instance.GetBoolStat(Consts.Upgrades.TELEPORT);
        _teleportCooldown = StatManager.Instance.GetFloatStat(Consts.Upgrades.TELEPORT_COOLDOWN);
        _shipSpeed = StatManager.Instance.GetFloatStat(Consts.Upgrades.SHIP_SPEED);
        _moneyMultiplier = StatManager.Instance.GetFloatStat(Consts.Upgrades.MONEY_MULTIPLER);
        _droneCount = StatManager.Instance.GetFloatStat(Consts.Upgrades.DRONE_COUNT);
        _dronePower = StatManager.Instance.GetFloatStat(Consts.Upgrades.DRONE_POWER);
        _droneAttackSpeed = StatManager.Instance.GetFloatStat(Consts.Upgrades.DRONE_ATTACK_SPEED);
        _doubleFire = StatManager.Instance.GetFloatStat(Consts.Upgrades.DOUBLE_FIRE);
        _tripleFire = StatManager.Instance.GetFloatStat(Consts.Upgrades.TRIPLE_FIRE);
    }
}
