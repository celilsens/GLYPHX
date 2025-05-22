using UnityEngine;
using UnityEngine.UI;

public class UpgradeButtonController : MonoBehaviour
{
    [SerializeField] private int _upgradeIndex;
    [SerializeField] private HangerSceneUIManager _hangarSceneUIManager;
    [SerializeField] private Button _upgradeButton;

    private void Awake()
    {
        _upgradeButton = gameObject.GetComponent<Button>();
        _upgradeButton.onClick.AddListener(OnPurchaseButtonClicked);
    }

    public void OnPurchaseButtonClicked()
    {

        UpgradeData upgrade = _hangarSceneUIManager.Upgrades[_upgradeIndex];

        bool purchased = _hangarSceneUIManager.CurrentUpgradeManager.TryPurchaseUpgrade(upgrade);
        if (purchased)
        {
            _hangarSceneUIManager.UpdateUI();
        }
        else
        {
            _hangarSceneUIManager.NotEnoughCurrency();
        }
    }
}
