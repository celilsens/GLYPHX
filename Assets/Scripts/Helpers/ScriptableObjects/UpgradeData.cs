using UnityEngine;

public enum UpgradeCategory
{
    Attack,
    Defence,
    Skill,
    Extra
}

[CreateAssetMenu(fileName = "NewUpgrade", menuName = "Upgrades/UpgradeData")]

public class UpgradeData : ScriptableObject
{
    [Header("Upgrade Settings")]
    public UpgradeCategory category;
    public string upgradeName;
    public string key;
    public float effectAmount = 0f;

    [Header("Purchase Settings")]
    public bool isRepeatable = false;
    public int maxPurchaseCount = 1;
    public float baseCost = 100f;
    public float costGrowthFactor = 1.5f;

    [Header("Requirements")]
    public string requiredKey;
}
