using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Text.RegularExpressions;

[RequireComponent(typeof(Image))]
public class Control : MonoBehaviour, 
    IBeginDragHandler,IDragHandler, IEndDragHandler
{
    public void OnBeginDrag(PointerEventData eventData)
    {
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
    private void SetDraggedPosition(PointerEventData eventData)
    {
        RectTransform rt = gameObject.GetComponent<RectTransform>();
        int nowAt;
        // transform the screen point to world point int rectangle
        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, eventData.position, eventData.pressEventCamera, out globalMousePos))
        {
            rt.position = globalMousePos;
            nowAt = Convert.ToInt16(name.Substring(4));
            Datamain.Points.OutPos[nowAt].x = (globalMousePos.x - 480f) / 59.5f;
            Datamain.Points.OutPos[nowAt].y = (globalMousePos.y - 300f) / 59.5f;
            PointSys.Balls[nowAt].GetComponent<LineRenderer>().SetPosition
                (1, Datamain.Points.OutPos[nowAt]);
        }
    }

}

// 25,575
// 475,575
// 475,135
// 25,135