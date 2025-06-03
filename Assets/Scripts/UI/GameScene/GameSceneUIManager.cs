using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using System;

public class GameSceneUIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _playerObject;

    [Header("UI Elements")]
    [SerializeField] private Image _healthBarFillImage;
    [SerializeField] private Image _shieldBarFillImage;
    [SerializeField] private GameObject _blackBackgroundOverlay;
    [SerializeField] private GameObject _redBackgroundOverlay;

    [Header("UI Panels")]
    [SerializeField] private GameObject _winUI;
    [SerializeField] private GameObject _loseUI;
    [SerializeField] private GameObject _settingsUI;

    [Header("Win UI Elements")]
    [SerializeField] private Button _winUIRestartButton;
    [SerializeField] private Button _winUIHangarButton;
    [SerializeField] private Button _winUINextLevelButton;
    [SerializeField] private TMP_Text _winUIMoneyText;
    [SerializeField] private GameObject _winUINextLevel;
    [SerializeField] private GameObject _winUIComplate;

    [Header("Lose UI Elements")]
    [SerializeField] private Button _loseUIRestartButton;
    [SerializeField] private Button _loseUIHangarButton;
    [SerializeField] private TMP_Text _loseUIMoneyText;

    [Header("Settings UI Elements")]
    [SerializeField] private Button _settingsUIOpenButton;
    [SerializeField] private Button _settingsUICloseButton;
    [SerializeField] private Button _settingsUIRestartButton;
    [SerializeField] private Button _settingsUIResumeButton;
    [SerializeField] private Button _settingsUIHangarButton;

    private Player _playerManager;

    private void Start()
    {
        if (_playerObject != null)
        {
            _playerManager = _playerObject.GetComponent<Player>();
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameOver += HandleGameOver;
            GameManager.Instance.OnPause += ShowSettingsUI;
            GameManager.Instance.OnResume += HideSettingsUI;
        }

        LevelManager levelManager = FindFirstObjectByType<LevelManager>();

        if (levelManager != null)
        {
            levelManager.OnLevelCompleted += ShowWinUI;
        }

        PrepareUI();
        UpdateUI();
    }

    private void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (_playerManager != null)
        {
            float targetHealthFill = 1f;
            float targetShieldFill = 1f;

            if (_playerManager.MaxPlayerHealth > 0f)
                targetHealthFill = Mathf.Clamp01(_playerManager.CurrentPlayerHealth / _playerManager.MaxPlayerHealth);

            if (_playerManager.MaxPlayerShield > 0f)
                targetShieldFill = Mathf.Clamp01(_playerManager.CurrentPlayerShield / _playerManager.MaxPlayerShield);

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
        if (_winUI != null) _winUI.transform.localScale = Vector3.zero;
        if (_loseUI != null) _loseUI.transform.localScale = Vector3.zero;
        if (_settingsUI != null) _settingsUI.transform.localScale = Vector3.zero;

        _blackBackgroundOverlay.SetActive(false);
        _redBackgroundOverlay.SetActive(false);

        _winUIRestartButton?.onClick.AddListener(RestartGame);
        _winUIHangarButton?.onClick.AddListener(GoToHangar);
        _winUINextLevelButton?.onClick.AddListener(WinUINextLevel);

        _loseUIRestartButton?.onClick.AddListener(RestartGame);
        _loseUIHangarButton?.onClick.AddListener(GoToHangar);

        _settingsUIOpenButton?.onClick.AddListener(GameManager.Instance.PauseGame);
        _settingsUICloseButton?.onClick.AddListener(GameManager.Instance.ResumeGame);
        _settingsUIResumeButton?.onClick.AddListener(GameManager.Instance.ResumeGame);
        _settingsUIRestartButton?.onClick.AddListener(RestartGame);
        _settingsUIHangarButton?.onClick.AddListener(GoToHangar);
    }

    private void HandleGameOver()
    {
        ShowLoseUI();
    }

    private void ShowWinUI()
    {
        GameManager.Instance.ChangeGameStatus(false);
        _blackBackgroundOverlay.SetActive(true);

        if (_winUIMoneyText != null)
        {
            _winUIMoneyText.text = "Total Money: " + GameManager.Instance.GetMoneyString();
        }

        int currentLevel = GameManager.Instance.GetSelectedLevelIndex();
        int totalLevels = GameManager.Instance.AllLevels.Length;

        if (currentLevel >= totalLevels - 1)
        {
            _winUINextLevel.SetActive(false);
            _winUIComplate.SetActive(true);
        }
        else
        {
            _winUINextLevel.SetActive(true);
            _winUIComplate.SetActive(false);
        }

        _winUI.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack);
    }

    private void ShowLoseUI()
    {
        GameManager.Instance.ChangeGameStatus(false);
        if (_loseUIMoneyText != null)
        {
            _loseUIMoneyText.text = "You Gained: " + GameManager.Instance.GetMoneyString();
        }

        _redBackgroundOverlay.SetActive(true);
        if (_loseUI != null)
        {
            _loseUI.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack);
        }
    }

    private void ShowSettingsUI()
    {
        _blackBackgroundOverlay.SetActive(true);
        if (_settingsUI != null)
        {
            _settingsUI.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack);
        }
    }

    private void HideSettingsUI()
    {
        _blackBackgroundOverlay.SetActive(false);
        if (_settingsUI != null)
        {
            _settingsUI.transform.DOScale(0f, 0.5f).SetEase(Ease.OutBack);
        }
    }

    private void WinUINextLevel()
    {
        int currentLevel = GameManager.Instance.GetSelectedLevelIndex();
        int maxUnlockedLevel = GameManager.Instance.GetMaxUnlockedLevel();

        if (currentLevel < maxUnlockedLevel)
        {
            GameManager.Instance.SetSelectedLevel(currentLevel + 1);
        }
        else
        {
            _winUINextLevel.SetActive(false);
            _winUIComplate.SetActive(true);
        }

        RestartGame();
    }

    private void RestartGame()
    {
        SceneLoadManager.Instance.LoadGameScene();
    }

    private void GoToHangar()
    {
        SceneLoadManager.Instance.LoadHangarScene();
    }
}
