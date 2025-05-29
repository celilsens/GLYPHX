using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class HangerSceneUIManager : MonoBehaviour
{

    [Header("Panels")]
    [SerializeField] private GameObject _shopPanel;
    [SerializeField] private GameObject _levelPanel;
    [SerializeField] private float _panelAnimDuration = .25f;

    [Header("Buttons")]
    [SerializeField] private Button _shopButton;
    [SerializeField] private GameObject _shopIcon;
    [SerializeField] private Button _levelsButton;
    [SerializeField] private GameObject _levelsIcon;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private GameObject _mainMenuIcon;

    [SerializeField] private float scaleFactor = 1.1f;
    [SerializeField] private float animationDuration = 0.3f;

    private void Start()
    {
        _shopPanel.transform.localScale = Vector3.one;
        _levelPanel.transform.localScale = Vector3.zero;
        SetupHoverEvents(_shopButton, _shopIcon);
        SetupHoverEvents(_levelsButton, _levelsIcon);
        SetupHoverEvents(_mainMenuButton, _mainMenuIcon);

        _shopButton.onClick.AddListener(OpenShop);
        _levelsButton.onClick.AddListener(OpenLevels);
        _mainMenuButton.onClick.AddListener(SceneLoadManager.Instance.LoadMainMenuScene);
    }

    private void OpenShop()
    {
        _shopPanel.transform.DOScale(Vector3.one, _panelAnimDuration).SetEase(Ease.OutBack);

        _levelPanel.transform.DOScale(Vector3.zero, _panelAnimDuration).SetEase(Ease.InBack);
    }

    private void OpenLevels()
    {
        _levelPanel.transform.DOScale(Vector3.one, _panelAnimDuration).SetEase(Ease.OutBack);

        _shopPanel.transform.DOScale(Vector3.zero, _panelAnimDuration).SetEase(Ease.InBack);
    }

    private void SetupHoverEvents(Button button, GameObject icon)
    {
        EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
            trigger = button.gameObject.AddComponent<EventTrigger>();

        trigger.triggers = new System.Collections.Generic.List<EventTrigger.Entry>();

        EventTrigger.Entry entryEnter = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerEnter
        };
        entryEnter.callback.AddListener((_) => ScaleIcon(icon, true));
        trigger.triggers.Add(entryEnter);

        EventTrigger.Entry entryExit = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerExit
        };
        entryExit.callback.AddListener((_) => ScaleIcon(icon, false));
        trigger.triggers.Add(entryExit);
    }

    private void ScaleIcon(GameObject icon, bool enlarge)
    {
        Vector3 targetScale = enlarge ? Vector3.one * scaleFactor : Vector3.one;
        icon.transform.DOScale(targetScale, animationDuration).SetEase(Ease.OutBack);
    }
}
