using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuSceneUIManager : MonoBehaviour
{
    [Header("Game Buttons")]
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _howToPlayButton;
    [SerializeField] private Button _howToPlayCloseButton;
    [SerializeField] private Button _quitButton;

    [Header("Credits Buttons")]
    [SerializeField] private Button _linkedinButton;
    [SerializeField] private string _linkedinUrl = "https://www.linkedin.com/in/celil-sen/";
    [SerializeField] private Button _githubButton;
    [SerializeField] private string _githubUrl = "https://github.com/celilsens";
    [SerializeField] private Button _itchioButton;
    [SerializeField] private string _itchioUrl = "https://celilsens.itch.io";

    [Header("Popups")]
    [SerializeField] private GameObject _howToPlayPopup;
    [SerializeField] private GameObject _howToPlayPopupOverlay;


    private void Start()
    {
        _playButton.onClick.AddListener(SceneLoadManager.Instance.LoadHangarScene);
        _howToPlayButton.onClick.AddListener(ActivateHowToPlayPopup);
        _quitButton.onClick.AddListener(QuitApp);
        _linkedinButton.onClick.AddListener(() => Application.OpenURL(_linkedinUrl));
        _githubButton.onClick.AddListener(() => Application.OpenURL(_githubUrl));
        _itchioButton.onClick.AddListener(() => Application.OpenURL(_itchioUrl));
        _howToPlayCloseButton.onClick.AddListener(DeactivateHowToPlayPopup);
        _howToPlayPopup.transform.localScale = Vector3.zero;
        _howToPlayPopup.SetActive(false);
    }

    private void ActivateHowToPlayPopup()
    {
        _howToPlayPopup.SetActive(true);
        _howToPlayPopupOverlay.SetActive(true);
        _howToPlayPopup.transform.localScale = Vector3.zero;
        _howToPlayPopup.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
    }

    private void DeactivateHowToPlayPopup()
    {
        _howToPlayPopupOverlay.SetActive(false);
        _howToPlayPopup.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack)
            .OnComplete(() => _howToPlayPopup.SetActive(false));
    }

    private void QuitApp()
    {
        Application.Quit();
    }
}
