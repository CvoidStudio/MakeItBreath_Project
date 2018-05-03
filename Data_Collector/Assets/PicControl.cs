using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PicControl : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler//, IScrollHandler
{

    public void OnBeginDrag(PointerEventData eventData)
    {
        MousePos = Input.mousePosition;
        SetDraggedPosition(eventData);
    }
    // during dragging
    public void OnDrag(PointerEventData eventData)
    {
        SetDraggedPosition(eventData);
    }

    // end dragging
    public void OnEndDrag(PointerEventData eventData)
    {
        SetDraggedPosition(eventData);
    }

    /// <summary>
    /// set position of the dragged game object
    /// </summary>
    /// <param name="eventData"></param>
    private Vector3 MousePos;
    
    private bool in_Rect(float mx, float my, float x, float y, float w, float h)
    {
        float dx = mx - x;
        float dy = my - y;
        return (dx > 0) && (dx < w) && (dy > 0) && (dy < h);
    }

    
    private void SetDraggedPosition(PointerEventData eventData)
    {
        RectTransform rt = gameObject.GetComponent<RectTransform>();
        // transform the screen point to world point int rectangle
        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, eventData.position, eventData.pressEventCamera, out globalMousePos)) {
            //if (in_Rect(globalMousePos.x, globalMousePos.y, 240f, 360f, 450f, 450f))
            {
                rt.position = globalMousePos - MousePos;
                MousePos = globalMousePos;
            }
        }
    }
}
