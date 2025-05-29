using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool IsGameActive { get; private set; } = true;

    [Header("Level Data")]
    [SerializeField] private LevelData[] _allLevels;
    public LevelData[] AllLevels => _allLevels;
    public LevelData SelectedLevelData { get; private set; }

    private float _playerMoney;
    private int _unlockedLevel;
    private int _selectedLevelIndex;
    private bool _isGameOver;

    public event Action OnGameOver;
    public event Action OnPause;
    public event Action OnResume;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        LoadPlayerMoney();
        _unlockedLevel = PlayerPrefs.GetInt(Consts.PlyrPrfs.UNLOCKED_LEVEL, 0);
        _selectedLevelIndex = PlayerPrefs.GetInt(Consts.PlyrPrfs.SELECTED_LEVEL, 0);

        if (_allLevels == null || _allLevels.Length == 0)
        {
            _allLevels = Resources.LoadAll<LevelData>("Levels");
            if (_allLevels == null || _allLevels.Length == 0)
            {
                Debug.LogError("All Levels is NULL!!!");
                return;
            }
        }

        _selectedLevelIndex = Mathf.Clamp(_selectedLevelIndex, 0, _allLevels.Length - 1);
        SelectedLevelData = _allLevels[_selectedLevelIndex];
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape)) TogglePause();
        if (Input.GetKeyDown(KeyCode.U)) AddMoney(10000);
    }

    #region Game State
    private void TogglePause()
    {
        if (_isGameOver) return;

        if (IsGameActive) PauseGame();
        else ResumeGame();
    }

    public void PauseGame()
    {
        IsGameActive = false;
        OnPause?.Invoke();
    }

    public void ResumeGame()
    {
        IsGameActive = true;
        OnResume?.Invoke();
    }

    public void GameOver()
    {
        IsGameActive = false;
        _isGameOver = true;
        OnGameOver?.Invoke();
    }

    public void ChangeGameStatus(bool gameStatus) => IsGameActive = gameStatus;
    #endregion

    #region Money
    public float GetMoney() => _playerMoney;

    public string GetMoneyString() => Mathf.FloorToInt(_playerMoney).ToString();

    public void LoadPlayerMoney() => _playerMoney = PlayerPrefs.GetFloat(Consts.PlyrPrfs.PLAYER_MONEY, 0f);

    public void AddMoney(float amount)
    {
        _playerMoney += amount;
        SavePlayerMoney();
    }

    public bool SpendMoney(float amount)
    {
        if (_playerMoney >= amount)
        {
            _playerMoney -= amount;
            SavePlayerMoney();
            return true;
        }
        return false;
    }

    public void SavePlayerMoney()
    {
        PlayerPrefs.SetFloat(Consts.PlyrPrfs.PLAYER_MONEY, _playerMoney);
        PlayerPrefs.Save();
    }
    #endregion

    #region Level
    public int GetMaxUnlockedLevel() => _unlockedLevel;

    public void UnlockNewLevel()
    {
        _unlockedLevel++;
        PlayerPrefs.SetInt(Consts.PlyrPrfs.UNLOCKED_LEVEL, _unlockedLevel);
        PlayerPrefs.Save();
    }

    public void SetSelectedLevel(int index)
    {
        if (_allLevels == null || _allLevels.Length == 0)
        {
            Debug.LogError("AllLevels null!! SetSelectedLevel doesnt work! .");
            return;
        }

        _selectedLevelIndex = Mathf.Clamp(index, 0, _allLevels.Length - 1);
        SelectedLevelData = _allLevels[_selectedLevelIndex];

        PlayerPrefs.SetInt(Consts.PlyrPrfs.SELECTED_LEVEL, _selectedLevelIndex);
        PlayerPrefs.Save();
    }

    public int GetSelectedLevelIndex() => _selectedLevelIndex;
    #endregion
}
