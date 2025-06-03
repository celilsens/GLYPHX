using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public bool CanPurchase(UpgradeData data)
    {
        if (!string.IsNullOrEmpty(data.requiredKey))
        {
            int requiredCount = PlayerPrefs.GetInt(GetPurchaseKey(data.requiredKey), 0);
            if (requiredCount == 0)
            {
                return false;
            }
        }

        int currentCount = GetCurrentPurchaseCount(data);
        if (!data.isRepeatable && currentCount > 0)
        {
            return false;
        }

        if (currentCount >= data.maxPurchaseCount)
        {
            return false;
        }

        float cost = CalculateCurrentCost(data, currentCount);
        float money = GameManager.Instance.GetMoney();
        if (money < cost)
        {
            UpgradeUIManager.Instance?.FlashInsufficientFunds();
            return false;
        }

        return true;
    }

    public bool TryPurchase(UpgradeData data)
    {
        if (!CanPurchase(data))
            return false;

        int currentCount = GetCurrentPurchaseCount(data);
        float cost = CalculateCurrentCost(data, currentCount);

        if (!GameManager.Instance.SpendMoney(cost))
            return false;

        var currentStat = StatManager.Instance.GetStat(data.key);
        if (currentStat == null)
            return false;

        switch (currentStat.Type)
        {
            case StatType.Float:
                float newFloatValue = currentStat.GetFloat() + data.effectAmount;
                StatManager.Instance.SetStat(data.key, new StatValue(newFloatValue));
                break;

            case StatType.Bool:
                StatManager.Instance.SetStat(data.key, new StatValue(true));
                break;
        }

        PlayerPrefs.SetInt(GetPurchaseKey(data.key), currentCount + 1);
        PlayerPrefs.Save();

        return true;
    }

    public float CalculateCurrentCost(UpgradeData data, int currentCount)
    {
        if (!data.isRepeatable) return data.baseCost;

        return data.baseCost * Mathf.Pow(data.costGrowthFactor, currentCount);
    }

    public int GetCurrentPurchaseCount(UpgradeData data)
    {
        return PlayerPrefs.GetInt(GetPurchaseKey(data.key), 0);
    }

    private string GetPurchaseKey(string upgradeKey)
    {
        return $"upgrade_{upgradeKey}_count";
    }
}
