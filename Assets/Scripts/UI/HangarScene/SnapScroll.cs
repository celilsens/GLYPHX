using UnityEngine;
using UnityEngine.UI;

public class SnapScroll : MonoBehaviour
{
    [Header("Scroll Settings")]
    [SerializeField] private RectTransform _content;
    [SerializeField] private float _pageWidth = 800f;
    [SerializeField] private float _snapSpeed = 10f;

    [Header("Buttons & Indicators")]
    [SerializeField] private Button _page1Button;
    [SerializeField] private GameObject _page1Fill;
    [SerializeField] private Button _page2Button;
    [SerializeField] private GameObject _page2Fill;

    private int _currentPage = 0;

    private void Awake()
    {
        _page1Button.onClick.AddListener(SnapToPage1);
        _page2Button.onClick.AddListener(SnapToPage2);
    }

    private void Update()
    {
        float targetX = -_currentPage * _pageWidth;
        Vector2 newPos = new Vector2(Mathf.Lerp(_content.anchoredPosition.x, targetX, Time.deltaTime * _snapSpeed), _content.anchoredPosition.y);
        _content.anchoredPosition = newPos;
    }

    public void SnapToPage1()
    {
        _currentPage = 0;
        UpdateIndicators();
    }

    public void SnapToPage2()
    {
        _currentPage = 1;
        UpdateIndicators();
    }

    private void UpdateIndicators()
    {
        switch (_currentPage)
        {
            case 0:
                _page1Fill.SetActive(true);
                _page2Fill.SetActive(false);
                break;
            case 1:
                _page1Fill.SetActive(false);
                _page2Fill.SetActive(true);
                break;
        }
    }
}