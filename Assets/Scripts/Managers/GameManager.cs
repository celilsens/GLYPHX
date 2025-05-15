using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool IsGameActive { get; private set; } = true;
    public bool CanPlayerMove = true;
    private bool _isGameOver;
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

    void Update()
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
        DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 0f, 0.2f).SetEase(Ease.InQuad).OnComplete(() => { Time.timeScale = 0f; });
        IsGameActive = false;
        CanPlayerMove = false;
    }

    public void ResumeGame()
    {
        Debug.Log("Game Resumed");
        DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1f, 0.2f).SetEase(Ease.OutQuad).OnComplete(() => { Time.timeScale = 1f; });
        IsGameActive = true;
        CanPlayerMove = true;
    }

    public void GameOver()
    {
        PauseGame();
        _isGameOver = true;
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
