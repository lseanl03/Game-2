using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayCardPanel : PanelBase
{
    public bool isUsingCard = false;
    public bool isShowingCardInfo = false;

    public Button playCardButton;
    public Image cardImage;
    public Image backImage;

    public GameObject playCardObj;
    public GameObject cardInfoObj;

    public ActionCard currentActionCard;
    public ActionCardDragHover currentCardDragHover;
    public void Update()
    {
    }
    public void PlayCard()
    {
        if (isUsingCard)
        {
            StartCoroutine(ShowCardInfo());
            StartCoroutine(HandlePlayCard());
        }
    }
    public IEnumerator HandlePlayCard()
    {
        uiManager.battleCanvas.skillPanel.PanelState(true);
        uiManager.battleCanvas.informationPanel.PanelState(true);
        yield return new WaitForSeconds(1);
        playerManager.ConsumeActionPoint(currentActionCard.actionCost);
        currentActionCard.PlayCard();
        Destroy(currentCardDragHover.placeHolder);
        gamePlayManager.playerActionCardList.Remove(currentCardDragHover.actionCard);
    }
    public void UnPlayCard()
    {
        if (isUsingCard)
        {
            currentCardDragHover.ReturnCard();
            PlayCardState(false);
            uiManager.battleCanvas.skillPanel.PanelState(true);
            uiManager.battleCanvas.informationPanel.PanelState(true);
        }
    }
     public IEnumerator ShowCardInfo()
    {
        PlayCardState(false);
        CardImageObj(true);
        yield return new WaitForSeconds(1);
        CardImageObj(false);
    }
    public void GetCardInfo(ActionCard actionCard, ActionCardDragHover cardDragHover)
    {
        currentActionCard = actionCard;
        currentCardDragHover = cardDragHover;
        cardImage.sprite = currentActionCard.cardSprite;
        backImage.color = currentActionCard.backImage.color;
        PlayCardState(true);
        CardImageObj(false);
    }
    public void PlayCardState(bool state)
    {
        isUsingCard = state;
        playCardObj.SetActive(state);
    }
    public void CardImageObj(bool state)
    {
        isShowingCardInfo = state;
        cardInfoObj.SetActive(state);
    }
    public void PlayCardButtonState(bool state)
    {
        playCardButton.gameObject.SetActive(state);
    }
}
