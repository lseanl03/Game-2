using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerActionCardField : MonoBehaviour, IPointerClickHandler
{
    public bool isZooming = false;
    public RectTransform rectTransform;
    public Animator animator;

    protected UIManager uiManager => UIManager.instance;
    public void Start()
    {
        animator = GetComponent<Animator>();
        rectTransform = GetComponent<RectTransform>();
        UnZoom();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log("Click");
        Zoom();
    }
    public void Zoom()
    {
        isZooming = true;
        animator.SetBool("Zoom", true);
        EnableDraggableAndInfo(isZooming);
        uiManager.battleCanvas.switchCardBattlePanel.PanelState(!isZooming);
    }
    public void UnZoom()
    {
        isZooming = false;
        animator.SetBool("Zoom", false);
        EnableDraggableAndInfo(isZooming);
        uiManager.battleCanvas.switchCardBattlePanel.PanelState(!isZooming);
    }
    public void EnableDraggableAndInfo(bool state)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (child.GetComponent<Draggable2>())
            {
                child.GetComponent<Draggable2>().enabled = state;
            }
            if (child.GetComponent<CardInfo>())
            {
                child.GetComponent<CardInfo>().enabled = state;
            }
        }
    }
}
