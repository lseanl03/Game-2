using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ActionCardDragHover : CardBase, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public bool canDrag = false;
    public bool canPush = false;
    public bool canPlayCard = true;
    public bool canShowPlayCardPanel = false;
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
        if(canPush)
        HandlePointer(true, 50f);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if(canPush)
        HandlePointer(false, 0f);
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (canDrag && !uiManager.battleCanvas.playCardPanel.isShowingCardInfo)
        HandleDrag();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if(canDrag && !uiManager.battleCanvas.playCardPanel.isShowingCardInfo)
        HandleBeginDrag();
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if(canDrag && !uiManager.battleCanvas.playCardPanel.isShowingCardInfo)
        HandleEndDrag();
    }
    public void HandleDrag()
    {
        if (gamePlayManager.actionPhase && cardInfo.enabled)
        {
            isDragging = true;
            transform.position = Input.mousePosition;
        }
    }
    public void HandlePointer(bool state, float posY)
    {
        if (transform.parent == gamePlayManager.gamePlayCanvas.playerActionCardField.transform)
        {
            isPushing = state;
            isSelecting = state;
            transform.localPosition = new Vector2(transform.localPosition.x, posY);
        }
    }
    public void HandleBeginDrag()
    {
        placeHolder = Instantiate(placeHolderPrefab, transform.parent);
        placeHolder.transform.SetSiblingIndex(transform.GetSiblingIndex());
        parentToReturn = transform.parent;
        placeHolderParent = parentToReturn;
        transform.SetParent(canvas.transform);
        GetComponent<CanvasGroup>().blocksRaycasts = false;

        uiManager.HideTooltip();
    }

    public void HandleEndDrag()
    {
        isDragging = false;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        if (transform.parent != placeHolderParent) CheckPlayCard();
    }
    public void ReturnCard()
    {
        if(placeHolder != null)
        {
            canPlayCard = false;
            actionCard.CardState(true);
            transform.SetParent(placeHolder.transform.parent);
            transform.SetSiblingIndex(placeHolder.transform.GetSiblingIndex());
            Destroy(placeHolder);
        }
    }

    public void CheckPlayCard()
    {
        if (playerManager.currentActionPoint < actionCard.actionCardData.actionCost)
        {
            ReturnCard();
            notificationManager.SetNewNotification("Action point not enough");
        }
        else
        {
            actionCard.CheckTarget();
            if (placeHolder != null && canPlayCard)
            {
                uiManager.battleCanvas.skillPanel.PanelState(false);
                uiManager.battleCanvas.informationPanel.PanelState(false);
                uiManager.battleCanvas.playCardPanel.PanelState(true);
                uiManager.battleCanvas.playCardPanel.GetCardInfo(actionCard, this);
                actionCard.CardState(false);
            }
        }
    }
}
