using UnityEngine;
using UnityEngine.UI;

public class CustomCursor : MonoBehaviour
{
    private RectTransform _cursorRectTransform;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Cursor.visible = false;
        _cursorRectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        _cursorRectTransform.position = Input.mousePosition;
    }

    private void OnDestroy()
    {
        Cursor.visible = true;
    }
}
