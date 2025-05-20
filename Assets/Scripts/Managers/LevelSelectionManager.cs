using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelSelectionManager : MonoBehaviour
{
    public LevelData[] allLevels;
    public GameObject levelButtonPrefab;
    public Transform buttonContainer;
    public int unlockedLevel; 

    void Start()
    {
        unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 0);
        for(int i = 0; i < allLevels.Length; i++)
        {
            GameObject buttonObj = Instantiate(levelButtonPrefab, buttonContainer);
            TMP_Text buttonText = buttonObj.GetComponentInChildren<TMP_Text>();
            buttonText.text = allLevels[i].levelName;
            Button buttonComp = buttonObj.GetComponent<Button>();
            if(i > unlockedLevel)
            {
                buttonComp.interactable = false;
            }
            else
            {
                int index = i;
                buttonComp.onClick.AddListener(() => LoadLevel(index));
            }
        }
    }

    void LoadLevel(int levelIndex)
    {
        PlayerPrefs.SetInt("SelectedLevel", levelIndex);
        SceneManager.LoadScene(Consts.SceneNames.GAME_SCENE);
    }
}
