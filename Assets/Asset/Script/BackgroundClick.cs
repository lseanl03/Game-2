using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BackgroundClick : BackgroundClickBase, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("OnPointerClick");
        tooltipManager.HideTooltip();
    }
    public void OnMouseDown()
    {
        Debug.Log("OnMouse");
        if(tooltipManager.showTooltip)
        {
            tooltipManager.HideTooltip();
        }
    }
}
