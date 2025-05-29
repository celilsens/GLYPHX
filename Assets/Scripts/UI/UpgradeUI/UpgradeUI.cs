using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeUI : MonoBehaviour
{
    [Header("References")]
    public TextMeshProUGUI upgradeNameText;
    public TextMeshProUGUI costText;
    public Button purchaseButton;

    private UpgradeData upgradeData;

    public void Initialize(UpgradeData data)
    {
        upgradeData = data;
        upgradeNameText.text = data.upgradeName;
        UpdateUI();
        purchaseButton.onClick.AddListener(OnPurchaseClicked);
    }

    private void OnPurchaseClicked()
    {
        if (UpgradeManager.Instance.TryPurchase(upgradeData))
        {
            UpgradeUIManager.Instance.RefreshAllUpgradeUIs();
        }
    }

    public void UpdateUI()
    {
        int count = UpgradeManager.Instance.GetCurrentPurchaseCount(upgradeData);

        bool isMaxed = 
            (!upgradeData.isRepeatable && count >= 1) ||
            (upgradeData.isRepeatable && count >= upgradeData.maxPurchaseCount);

        bool hasRequired = true;

        if (!string.IsNullOrEmpty(upgradeData.requiredKey))
        {
            int requiredCount = PlayerPrefs.GetInt($"upgrade_{upgradeData.requiredKey}_count", 0);
            hasRequired = requiredCount > 0;
        }

        if (!hasRequired && count == 0)
        {
            purchaseButton.interactable = false;
            costText.text = "BLOCKED";
            costText.color = Color.gray;
            return;
        }

        purchaseButton.interactable = !isMaxed;

        if (isMaxed)
        {
            costText.text = "MAXED";
            costText.color = Color.yellow;
        }
        else
        {
            float cost = UpgradeManager.Instance.CalculateCurrentCost(upgradeData, count);
            int roundedCost = Mathf.FloorToInt(cost);
            costText.text = $"{roundedCost} $";

            float playerMoney = GameManager.Instance.GetMoney();
            costText.color = playerMoney >= cost ? HexToColor("#1EFF00") : Color.red;
        }
    }

    private Color HexToColor(string hex)
    {
        if (ColorUtility.TryParseHtmlString(hex, out Color color))
            return color;
        return Color.white;
    }
}
