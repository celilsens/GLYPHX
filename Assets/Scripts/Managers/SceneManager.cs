using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerController : MonoBehaviour
{
    // Singleton örneği
    public static SceneManagerController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadMainMenuScene()
    {
        SceneManager.LoadScene(Consts.SceneNames.MAIN_MENU_SCENE);
    }

    public void LoadHangarScene()
    {
        SceneManager.LoadScene(Consts.SceneNames.HANGAR_SCENE);
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene(Consts.SceneNames.GAME_SCENE);
    }

    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
