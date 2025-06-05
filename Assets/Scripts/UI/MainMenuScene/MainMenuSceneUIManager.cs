using UnityEngine;
using UnityEngine.UI;

public class MainMenuSceneUIManager : MonoBehaviour
{
    [Header ("Game Buttons")]
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _quitButton;

    [Header ("Credits Buttons")]
    [SerializeField] private Button _linkedinButton;
    [SerializeField] private string _linkedinUrl = "https://www.linkedin.com/in/celil-sen/";
    [SerializeField] private Button _githubButton;
    [SerializeField] private string _githubUrl ="https://github.com/celilsens";
    [SerializeField] private Button _itchioButton;
    [SerializeField] private string _itchioUrl="https://celilsens.itch.io";

    private void Start()
    {
        _playButton.onClick.AddListener(SceneLoadManager.Instance.LoadHangarScene);
        _quitButton.onClick.AddListener(QuitApp);
        _linkedinButton.onClick.AddListener(() => Application.OpenURL(_linkedinUrl));
        _githubButton.onClick.AddListener(() => Application.OpenURL(_githubUrl));
        _itchioButton.onClick.AddListener(() => Application.OpenURL(_itchioUrl));

    }

    private void QuitApp()
    {
        Application.Quit();
    }
}
