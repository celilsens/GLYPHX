using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool IsGameActive = true;
    public float PlayerMaxHealth { get; private set; } = 100f;
    public float PlayerMaxShield { get; private set; } = 50f;
    public int PlayerDamage { get; private set; } = 20;

    private bool _isGameOver;
    private int _playerMoney;

    public event Action OnGameOver;
    public event Action OnPause;
    public event Action OnResume;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadPlayerMoney();
            LoadPlayerStats();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    private void TogglePause()
    {
        if (_isGameOver) return;

        if (IsGameActive)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    public void PauseGame()
    {
        Debug.Log("Game Paused");

        IsGameActive = false;

        OnPause?.Invoke();
    }

    public void ResumeGame()
    {
        Debug.Log("Game Resumed");

        IsGameActive = true;

        OnResume?.Invoke();
    }

    public void GameOver()
    {
        Debug.Log("Game Over!!!");

        IsGameActive = false;
        _isGameOver = true;

        OnGameOver?.Invoke();
    }

    public void LoadPlayerMoney()
    {
        _playerMoney = PlayerPrefs.GetInt("PlayerMoney", 0);
    }

    public void SavePlayerMoney()
    {
        PlayerPrefs.SetInt("PlayerMoney", _playerMoney);
        PlayerPrefs.Save();
    }

    public void AddMoney(int amount)
    {
        _playerMoney += amount;
        SavePlayerMoney();
    }

    public bool SpendMoney(int amount)
    {
        if (_playerMoney >= amount)
        {
            _playerMoney -= amount;
            SavePlayerMoney();
            return true;
        }
        return false;
    }

    public int GetPlayerMoney()
    {
        return _playerMoney;
    }

    public void LoadPlayerStats()
    {
        PlayerMaxHealth = PlayerPrefs.GetFloat("PlayerMaxHealth", 100f);
        PlayerMaxShield = PlayerPrefs.GetFloat("PlayerMaxShield", 50f);
    }

    public void SavePlayerStats()
    {
        PlayerPrefs.SetFloat("PlayerMaxHealth", PlayerMaxHealth);
        PlayerPrefs.SetFloat("PlayerMaxShield", PlayerMaxShield);
        PlayerPrefs.Save();
    }

    public void UpgradePlayerHealth(float additionalHealth)
    {
        PlayerMaxHealth += additionalHealth;
        SavePlayerStats();
    }

    public void UpgradePlayerShield(float additionalShield)
    {
        PlayerMaxShield += additionalShield;
        SavePlayerStats();
    }
}
