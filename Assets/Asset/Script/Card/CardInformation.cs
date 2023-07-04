using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CardInformation : CardInformationBase, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
        onClick = true;    
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("OnPointerUp");
        onClick = false;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("begin");
        onBegin = true;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("end");
        onBegin = false;
    }
}
