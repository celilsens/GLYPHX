using UnityEngine;
using UnityEngine.UI;

public class MainMenuSceneUIManager : MonoBehaviour
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _quitButton;

    private void Start()
    {
        _playButton.onClick.AddListener(SceneLoadManager.Instance.LoadHangarScene);
        _playButton.onClick.AddListener(QuitApp);
    }

    private void QuitApp()
    {
        Application.Quit();
    }
}
