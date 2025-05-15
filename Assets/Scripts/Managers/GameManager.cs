using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool IsGameActive { get; private set; } = true;
    private int _playerMoney;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadPlayerMoney();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool GetGameState() => IsGameActive;

    public void PauseGame()
    {
        Time.timeScale = 0;
        IsGameActive = false;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        IsGameActive = true;
    }

    public void GameOver()
    {
        PauseGame();
        //TODO: Game Over UI
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
}
