using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class GameSceneUIManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Image _healthBarFillImage;
    [SerializeField] private Image _shieldBarFillImage;
    [SerializeField] private GameObject _backgroundOverlay;

    [Header("UI Panels")]
    [SerializeField] private GameObject _loseUI;
    [SerializeField] private GameObject _settingsUI;

    private Player playerManager;

    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            playerManager = playerObj.GetComponent<Player>();
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameOver += HandleGameOver;
            GameManager.Instance.OnPause += ShowSettingsUI;
            GameManager.Instance.OnResume += HideSettingsUI;
        }

        _backgroundOverlay = GameObject.FindGameObjectWithTag("GameSceneBackgroundOverlay");

        PrepareUI();
        UpdateUI();
    }

    private void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (playerManager != null)
        {
            float targetHealthFill = Mathf.Clamp01(playerManager.CurrentPlayerHealth / playerManager.MaxPlayerHealth);
            float targetShieldFill = Mathf.Clamp01(playerManager.CurrentPlayerShield / playerManager.MaxPlayerShield);

            if (_healthBarFillImage != null)
            {
                if (!Mathf.Approximately(_healthBarFillImage.fillAmount, targetHealthFill))
                {
                    _healthBarFillImage.DOKill();
                    _healthBarFillImage.DOFillAmount(targetHealthFill, 0.5f).SetEase(Ease.OutCubic);
                }
            }

            if (_shieldBarFillImage != null)
            {
                if (!Mathf.Approximately(_shieldBarFillImage.fillAmount, targetShieldFill))
                {
                    _shieldBarFillImage.DOKill();
                    _shieldBarFillImage.DOFillAmount(targetShieldFill, 0.5f).SetEase(Ease.OutCubic);
                }
            }
        }
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameOver -= HandleGameOver;
            GameManager.Instance.OnPause -= ShowSettingsUI;
            GameManager.Instance.OnResume -= HideSettingsUI;
        }
    }

    private void PrepareUI()
    {
        if (_backgroundOverlay != null)
        {
            _backgroundOverlay.SetActive(false);
        }

        if (_loseUI != null)
        {
            _loseUI.transform.localScale = Vector3.zero;
        }
        if (_settingsUI != null)
        {
            _settingsUI.transform.localScale = Vector3.zero;
        }
    }

    private void HandleGameOver()
    {
        ShowLoseUI();
    }

    private void ShowLoseUI()
    {
        _backgroundOverlay.SetActive(true);

        if (_loseUI != null)
        {
            _loseUI.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack);
        }
    }

    private void ShowSettingsUI()
    {
        _backgroundOverlay.SetActive(true);

        if (_settingsUI != null)
        {
            _settingsUI.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack);
        }
    }

    private void HideSettingsUI()
    {
        _backgroundOverlay.SetActive(false);

        if (_settingsUI != null)
        {
            _settingsUI.transform.DOScale(0f, 0.5f).SetEase(Ease.OutBack);
        }
    }
}
