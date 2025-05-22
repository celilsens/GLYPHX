using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class UpgradeManager
{
    private PlayerData _playerData;

    private Dictionary<UpgradeData, int> _upgradePurchaseCounts = new Dictionary<UpgradeData, int>();

    public UpgradeManager(PlayerData playerData)
    {
        _playerData = playerData;
    }

    public bool TryPurchaseUpgrade(UpgradeData upgrade)
    {
        int count;

        if (_upgradePurchaseCounts.ContainsKey(upgrade))
        {
            count = _upgradePurchaseCounts[upgrade];
        }
        else
        {
            count = 0;
        }

        if (count >= upgrade.maxPurchaseCount)
        {
            Debug.Log(upgrade.upgradeName + "'s Level is MAXED!!!");
            return false;
        }

        int effectiveCost;

        if (upgrade.isRepeatable)
        {
            float growth = Mathf.Pow(upgrade.costGrowthFactor, count);
            effectiveCost = Mathf.RoundToInt(upgrade.cost * growth);
        }
        else
        {
            effectiveCost = upgrade.cost;
        }


        if (GameManager.Instance.SpendMoney(effectiveCost))
        {
            ApplyUpgrade(upgrade);

            if (_upgradePurchaseCounts.ContainsKey(upgrade))
                _upgradePurchaseCounts[upgrade]++;
            else
                _upgradePurchaseCounts.Add(upgrade, 1);

            return true;
        }
        return false;
    }

    public int GetPurchaseCount(UpgradeData upgrade)
    {
        return _upgradePurchaseCounts.ContainsKey(upgrade) ? _upgradePurchaseCounts[upgrade] : 0;
    }

    public void ApplyUpgrade(UpgradeData upgrade)
    {
        string targetField = GetTargetFieldName(upgrade.upgradeName);
        FieldInfo field = typeof(PlayerData).GetField(targetField, BindingFlags.Public | BindingFlags.Instance);
        if (field == null)
        {
            Debug.LogWarning($"Field '{targetField}' not found in PlayerData.");
            return;
        }
        switch (upgrade.effectType)
        {
            case UpgradeEffectType.StatBase:
                ApplyFlatIncrease(field, upgrade.effectValue);
                break;
            case UpgradeEffectType.StatMultiple:
                ApplyMultiplier(field, upgrade.effectValue);
                break;
            case UpgradeEffectType.Activate:
                ApplyActivation(field);
                break;
            default:
                Debug.LogWarning("Unknown upgrade effect type.");
                break;
        }
    }

    private void ApplyFlatIncrease(FieldInfo field, float value)
    {
        if (field.FieldType == typeof(float))
        {
            float current = (float)field.GetValue(_playerData);
            field.SetValue(_playerData, current + value);
        }
    }

    private void ApplyMultiplier(FieldInfo field, float multiplier)
    {
        if (field.FieldType == typeof(float))
        {
            float current = (float)field.GetValue(_playerData);
            field.SetValue(_playerData, current * multiplier);
        }
    }

    private void ApplyActivation(FieldInfo field)
    {
        if (field.FieldType == typeof(bool))
        {
            field.SetValue(_playerData, true);
        }
    }

    private string GetTargetFieldName(string upgradeName)
    {
        return upgradeName.Replace(" ", "");
    }
}
