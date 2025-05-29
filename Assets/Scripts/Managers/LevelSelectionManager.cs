using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSelectionManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject levelButtonPrefab;
    [SerializeField] private Transform buttonContainer;
    private TMP_Text _levelNameText;

    private void Start()
    {
        GenerateLevelButtons();
    }

    private void GenerateLevelButtons()
    {
        if (GameManager.Instance == null || GameManager.Instance.AllLevels == null)
        {
            Debug.LogError("GameManager ya da AllLevels null!");
            return;
        }

        var levels = GameManager.Instance.AllLevels;
        int unlockedLevel = GameManager.Instance.GetMaxUnlockedLevel();

        for (int i = 0; i < levels.Length; i++)
        {
            GameObject buttonObj = Instantiate(levelButtonPrefab, buttonContainer);

            _levelNameText = buttonObj.GetComponentInChildren<TMP_Text>();

            if (_levelNameText != null)
            {
                _levelNameText.text = levels[i].levelNumber;
            }
            else
            {
                Debug.LogWarning("LevelName is not in Level prefab.");
            }

            Button button = buttonObj.GetComponentInChildren<Button>();
            if (i > unlockedLevel)
            {
                button.interactable = false;
                _levelNameText.color = Color.red;
            }
            else
            {
                int index = i;
                button.onClick.AddListener(() =>
                {
                    Debug.Log("Button clicked: LevelIndex = " + index);
                    SelectLevel(index);
                });
            }
        }
    }


    private void SelectLevel(int levelIndex)
    {
        GameManager.Instance.SetSelectedLevel(levelIndex);
        SceneLoadManager.Instance.LoadGameScene();
    }
}
