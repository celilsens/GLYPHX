using UnityEngine;
using UnityEngine.UI;

public class CustomCursor : MonoBehaviour
{
    private RectTransform _cursorRectTransform;

    private void Awake()
    {

        _cursorRectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (GameManager.Instance.IsGameActive)
        {
            Cursor.visible = false;
            _cursorRectTransform.position = Input.mousePosition;
        }
        else
        {
            Cursor.visible = true;
        }
    }

    private void OnDestroy()
    {
        Cursor.visible = true;
    }
}
