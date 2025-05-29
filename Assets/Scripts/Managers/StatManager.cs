using System.Collections.Generic;
using UnityEngine;

public class StatManager : MonoBehaviour
{
    public static StatManager Instance { get; private set; }

    private Dictionary<string, StatValue> stats = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadStats();
    }

    public void SetStat(string key, StatValue value)
    {
        stats[key] = value;
        SaveStat(key, value);
        PlayerPrefs.Save();
    }

    public StatValue GetStat(string key)
    {
        if (stats.TryGetValue(key, out StatValue value))
            return value;

        return null;
    }

    private void SaveStat(string key, StatValue value)
    {
        switch (value.Type)
        {
            case StatType.Float: PlayerPrefs.SetFloat(key, value.GetFloat()); break;
            case StatType.Int: PlayerPrefs.SetInt(key, value.GetInt()); break;
            case StatType.Bool: PlayerPrefs.SetInt(key, value.GetBool() ? 1 : 0); break;
        }
    }

    private void LoadStats()
    {
        // Attack Upgrades
        AddDefaultFloat(Consts.Upgrades.BASE_DAMAGE, 10f);
        AddDefaultFloat(Consts.Upgrades.DAMAGE_MULTIPLIER, 1f);
        AddDefaultFloat(Consts.Upgrades.KNOCKBACK_POWER, 100f);
        AddDefaultFloat(Consts.Upgrades.RELOAD_SPEED, 1f);
        AddDefaultFloat(Consts.Upgrades.BULLET_MOVE_SPEED, 10f);
        AddDefaultFloat(Consts.Upgrades.CRITICAL_HIT_RATE, 0.1f);
        AddDefaultFloat(Consts.Upgrades.CRITICAL_HIT_MULTIPLIER, 2f);
        // Defence Upgrades
        AddDefaultFloat(Consts.Upgrades.BASE_HEALTH, 100f);
        AddDefaultFloat(Consts.Upgrades.HEALTH_MULTIPLIER, 1f);
        AddDefaultFloat(Consts.Upgrades.HEALTH_REGEN, 0f);
        AddDefaultFloat(Consts.Upgrades.BASE_SHIELD, 50f);
        AddDefaultFloat(Consts.Upgrades.SHIELD_MULTIPLIER, 1f);
        AddDefaultFloat(Consts.Upgrades.SHIELD_REGEN, 0f);
        // Skill Upgrades
        AddDefaultBool(Consts.Upgrades.ROCKET, false);
        AddDefaultFloat(Consts.Upgrades.ROCKET_POWER, 50f);
        AddDefaultFloat(Consts.Upgrades.ROCKET_RADIUS, 3f);
        AddDefaultFloat(Consts.Upgrades.ROCKET_COOLDOWN, 5f);
        AddDefaultBool(Consts.Upgrades.ERASER, false);
        AddDefaultFloat(Consts.Upgrades.ERASER_COOLDOWN, 6f);
        AddDefaultBool(Consts.Upgrades.TELEPORT, false);
        AddDefaultFloat(Consts.Upgrades.TELEPORT_COOLDOWN, 10f);
        // Extra Upgrades
        AddDefaultFloat(Consts.Upgrades.SHIP_SPEED, 5f);
        AddDefaultFloat(Consts.Upgrades.MONEY_MULTIPLER, 1f);
        AddDefaultFloat(Consts.Upgrades.DRONE_COUNT, 0);
        AddDefaultFloat(Consts.Upgrades.DRONE_POWER, 25f);
        AddDefaultFloat(Consts.Upgrades.DRONE_ATTACK_SPEED, 1f);
        AddDefaultFloat(Consts.Upgrades.DOUBLE_FIRE, 0);
        AddDefaultFloat(Consts.Upgrades.TRIPLE_FIRE, 0);
    }

    private void AddDefaultFloat(string key, float defaultValue)
    {
        float value = PlayerPrefs.GetFloat(key, defaultValue);
        stats[key] = new StatValue(value);
    }

    public float GetFloatStat(string key)
    {
        var stat = StatManager.Instance.GetStat(key);
        return stat != null ? stat.GetFloat() : 0f;
    }

    private void AddDefaultBool(string key, bool defaultValue)
    {
        int value = PlayerPrefs.GetInt(key, defaultValue ? 1 : 0);
        stats[key] = new StatValue(value == 1);
    }

    public bool GetBoolStat(string key)
    {
        var stat = StatManager.Instance.GetStat(key);
        return stat != null && stat.GetBool();
    }
}
