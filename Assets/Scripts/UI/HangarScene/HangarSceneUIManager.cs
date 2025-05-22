using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HangerSceneUIManager : MonoBehaviour
{
    [Header("Player UI")]
    [SerializeField] private TMP_Text playerMoneyText;

    [Header("Upgrade UI Elements")]
    [SerializeField] private UpgradeItemUI[] upgradeItems;
    [SerializeField] private UpgradeData[] upgrades;

    private UpgradeManager _upgradeManager;

    public UpgradeManager CurrentUpgradeManager => _upgradeManager;
    public UpgradeData[] Upgrades => upgrades;

    private void Start()
    {
        _upgradeManager = new UpgradeManager(GameManager.Instance.PlayerData);

        UpdateUI();
    }

    public void UpdateUI()
    {
        UpdatePlayerMoneyDisplay();
        UpdateUpgradesDisplay();
    }

    private void UpdatePlayerMoneyDisplay()
    {
        int money = GameManager.Instance.GetPlayerMoney();
        playerMoneyText.text = money.ToString();
    }

    private void UpdateUpgradesDisplay()
    {
        int count = Mathf.Min(upgradeItems.Length, upgrades.Length);
        for (int i = 0; i < count; i++)
        {
            UpgradeData upgrade = upgrades[i];
            int purchaseCount = _upgradeManager.GetPurchaseCount(upgrade);
            int effectiveCost = upgrade.isRepeatable ?
                Mathf.RoundToInt(upgrade.cost * Mathf.Pow(upgrade.costGrowthFactor, purchaseCount)) :
                upgrade.cost;

            upgradeItems[i].upgradeNameText.text = upgrade.upgradeName;
            upgradeItems[i].upgradePriceText.text = "$ " + effectiveCost;
        }
    }

    public void NotEnoughCurrency()
    {
        StartCoroutine(MoneyColorChange());
    }
    private IEnumerator MoneyColorChange()
    {
        playerMoneyText.color = Color.red;
        yield return new WaitForSeconds(2f);
        playerMoneyText.color = Color.white;
    }
}

[System.Serializable]
public class UpgradeItemUI
{
    public TMP_Text upgradeNameText;
    public TMP_Text upgradePriceText;
}
