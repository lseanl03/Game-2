using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

public class ActionCardDragHover : CardBase, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public bool canDrag = false;
    public bool canPlayCard = true;
    public bool isSelecting = false;
    public bool isDragging = false;
    public bool isPushing = false;

    [Header("Prefab")]
    public GameObject placeHolderPrefab;
    public GameObject placeHolder;

    [Header("Holder")]
    public Transform parentToReturn;
    public Transform placeHolderParent;

    [Header("Component")]
    public ActionCard actionCard;
    public CardInfo cardInfo;

    private GamePlayCanvas canvas;
    public void Start()
    {
        actionCard = GetComponent<ActionCard>();
        cardInfo = GetComponent<CardInfo>();
        canvas = transform.root.GetComponentInChildren<GamePlayCanvas>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        HandlePointer(true, 50f);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        HandlePointer(false, 0f);
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (canDrag)
        HandleDrag();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if(canDrag)
        HandleBeginDrag();
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if(canDrag)
        HandleEndDrag();
    }
    public void HandleDrag()
    {
        if (gamePlayManager.startCombat && cardInfo.enabled)
        {
            isDragging = true;
            transform.position = Input.mousePosition;
        }
    }
    public void HandlePointer(bool state, float posY)
    {
        if (transform.parent == gamePlayManager.gamePlayCanvas.playerActionCardField.transform)
        {
            if (gamePlayManager.startCombat && cardInfo.enabled)
            {
                isPushing = state;
                isSelecting = state;
                transform.localPosition = new Vector2(transform.localPosition.x, posY);
            }
        }
    }
    public void HandleBeginDrag()
    {
        uiManager.HideTooltip();

        GetComponent<CanvasGroup>().blocksRaycasts = false;
        placeHolder = Instantiate(placeHolderPrefab, transform.parent);
        placeHolder.transform.SetSiblingIndex(transform.GetSiblingIndex());
        parentToReturn = transform.parent;
        placeHolderParent = parentToReturn;
        transform.SetParent(canvas.transform);
    }

    public void HandleEndDrag()
    {
        isDragging = false;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        if (transform.parent != placeHolderParent) CheckPlayCard();
    }
    public void ReturnCard()
    {
        canPlayCard = false;
        actionCard.CardState(true);
        transform.SetParent(placeHolder.transform.parent);
        transform.SetSiblingIndex(placeHolder.transform.GetSiblingIndex());
        Destroy(placeHolder);
    }

    public void CheckPlayCard()
    {
        if (playerManager.currentActionPoint < actionCard.actionCardData.cardCost)
        {
            ReturnCard();
            notificationManager.SetNewNotification("Action point not enough");
        }
        else
        {
            actionCard.CheckTarget();
            if (canPlayCard)
            {
                actionCard.CardState(false);
                uiManager.battleCanvas.playCardPanel.PanelState(true);
                uiManager.battleCanvas.playCardPanel.GetInfoCard(actionCard.actionCardData, this);
            }
        }
    }
}
