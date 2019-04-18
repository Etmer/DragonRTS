using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] RectTransform _interactableRect;
    [SerializeField] RectTransform _interactableRect2;
    private Rect rect;
    private Rect rect2;

    public Texture text;

    [Header("UI Info Box")]
    [SerializeField] private RectTransform _uIInfoBoxRectTransform;
    [SerializeField] private Text _uIInfoBoxContent;

    public bool PointInRect(Vector2 point)
    {
        GetRect(_interactableRect, ref rect);
        GetRect(_interactableRect2, ref rect2);
        return !rect.Contains(point) && !rect2.Contains(point);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawGUITexture(rect,text);
        Gizmos.DrawGUITexture(rect2, text);
    }

    private void GetRect(RectTransform input, ref Rect output)
    {
        float X = input.position.x;
        float Y = input.localPosition.y +  Screen.height/2;
        output.Set(X,Y, input.rect.width, input.rect.height);
    }

    public void SetInfoBox(string content, Vector3 position)
    {
        _uIInfoBoxRectTransform.position = Camera.main.WorldToScreenPoint(position);
        _uIInfoBoxContent.text = content;
    }
}
