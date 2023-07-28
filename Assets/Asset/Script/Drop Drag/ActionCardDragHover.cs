using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ActionCardDragHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler
{
    public bool isSelecting = false;
    public bool isDragging = false;

    public ActionCard actionCard;
    public CardInfo cardInfo;
    public void Start()
    {
        actionCard = GetComponent<ActionCard>();
        cardInfo = GetComponent<CardInfo>();
    }
    protected GamePlayManager gamePlayManager => GamePlayManager.instance;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (gamePlayManager.startCombat && cardInfo.enabled)
        {
            Debug.Log("Enter");
            isSelecting = true;
            transform.localPosition = new Vector2(transform.localPosition.x, 50f);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (gamePlayManager.startCombat && cardInfo.enabled)
        {
            Debug.Log("Exit");
            isSelecting = false;
            transform.localPosition = new Vector2(transform.localPosition.x, 0f);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Exit");
    }
}
