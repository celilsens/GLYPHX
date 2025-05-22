using UnityEngine;

[CreateAssetMenu(fileName = "NewUpgradeData", menuName = "Upgrade/Upgrade Data")]
public class UpgradeData : ScriptableObject
{
    public string upgradeName;
    public UpgradeCategory category;
    public int cost;
    public UpgradeEffectType effectType;
    public float effectValue;
    public bool isRepeatable;
    public int maxPurchaseCount = 10;
    public float costGrowthFactor = 2;
}
