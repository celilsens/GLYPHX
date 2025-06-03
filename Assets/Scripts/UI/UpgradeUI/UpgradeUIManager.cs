using UnityEngine;
using TMPro;
using System.Collections;

public class UpgradeUIManager : MonoBehaviour
{
    public static UpgradeUIManager Instance { get; private set; }
    public GameObject upgradeUIPrefab;
    public UpgradeData[] upgrades;

    [Header("Category Containers")]
    public Transform attackContainer;
    public Transform defenceContainer;
    public Transform skillContainer;
    public Transform extraContainer;

    [Header("Money Display")]
    public TextMeshProUGUI playerMoneyText;

    private Color _originalColor;
    private float _originalFontSize;

    private void Awake()
    {
        Instance = this;

        if (playerMoneyText != null)
        {
            _originalColor = playerMoneyText.color;
            _originalFontSize = playerMoneyText.fontSize;
        }
    }

    private void Start()
    {
        GenerateUpgradeUIs();
        UpdateMoneyDisplay();
    }

    private void GenerateUpgradeUIs()
    {
        foreach (var upgrade in upgrades)
        {
            Transform parent = GetContainerForCategory(upgrade.category);
            var go = Instantiate(upgradeUIPrefab, parent);
            var ui = go.GetComponent<UpgradeUI>();
            ui.Initialize(upgrade);
        }
    }

    private Transform GetContainerForCategory(UpgradeCategory category)
    {
        return category switch
        {
            UpgradeCategory.Attack => attackContainer,
            UpgradeCategory.Defence => defenceContainer,
            UpgradeCategory.Skill => skillContainer,
            UpgradeCategory.Extra => extraContainer,
            _ => attackContainer
        };
    }

    public void UpdateMoneyDisplay()
    {
        if (playerMoneyText != null)
        {
            playerMoneyText.text = GameManager.Instance.GetMoneyString();
        }
    }

    public void FlashInsufficientFunds()
    {
        if (playerMoneyText != null)
        {
            StopAllCoroutines();
            StartCoroutine(AnimateMoneyFlash());
        }
    }

    private IEnumerator AnimateMoneyFlash()
    {
        float duration = 0.4f;
        float time = 0f;

        Color targetColor = Color.red;
        float targetSize = _originalFontSize * 1.15f;

        while (time < duration)
        {
            float t = time / duration;
            playerMoneyText.color = Color.Lerp(targetColor, _originalColor, t);
            playerMoneyText.fontSize = Mathf.Lerp(targetSize, _originalFontSize, t);
            time += Time.deltaTime;
            yield return null;
        }

        playerMoneyText.color = _originalColor;
        playerMoneyText.fontSize = _originalFontSize;
    }

    public void RefreshAllUpgradeUIs()
    {
        foreach (Transform container in new[] { attackContainer, defenceContainer, skillContainer, extraContainer })
        {
            foreach (Transform child in container)
            {
                var ui = child.GetComponent<UpgradeUI>();
                if (ui != null)
                    ui.UpdateUI();
            }
        }

        UpdateMoneyDisplay();
    }
}
