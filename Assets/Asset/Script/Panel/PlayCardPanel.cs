using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayCardPanel : PanelBase
{
    public float waitingTime = 1.5f;

    public bool isUsingCard = false;
    public bool isShowingCardInfo = false;

    public Image cardImage;
    public Image backImage;

    public GameObject playCardObj;
    public GameObject cardInfoObj;

    private ActionCard currentActionCard;
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
        if (gamePlayManager.gamePlayCanvas.playerActionCardField.isZooming)
            gamePlayManager.gamePlayCanvas.playerActionCardField.ZoomState(false);

        uiManager.battleCanvas.switchCardBattlePanel.PanelState(false);
        uiManager.battleCanvas.skillPanel.PanelState(true);
        uiManager.battleCanvas.informationPanel.PanelState(true);

        yield return new WaitForSeconds(waitingTime);
        currentActionCard.PlayCard();
        Destroy(currentActionCard.actionCardDragHover.placeHolder);
        Destroy(currentActionCard.gameObject);
        playerManager.ConsumeActionPoint(currentActionCard.actionCost);
        gamePlayManager.playerActionCardList.Remove(currentActionCard.actionCardDragHover.actionCard);
    }
    public void UnPlayCard()
    {
        if (isUsingCard)
        {
            currentActionCard.actionCardDragHover.ReturnCard();
            PlayCardState(false);
            uiManager.battleCanvas.skillPanel.PanelState(true);
            uiManager.battleCanvas.informationPanel.PanelState(true);
        }
    }
     public IEnumerator ShowCardInfo()
    {
        uiManager.HideTooltip();
        PlayCardState(false);
        CardImageObj(true);
        yield return new WaitForSeconds(waitingTime);
        CardImageObj(false);
    }
    public void GetCardInfo(ActionCard actionCard)
    {
        currentActionCard = actionCard;
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
        CanvasGroup canvasGroup = cardInfoObj.GetComponent<CanvasGroup>();
        if (state)
        {
            canvasGroup.alpha = 0.5f;
            canvasGroup.DOFade(1f, 0.2f);
            AudioManager.instance.PlayShowActionCard();
        }
        else
        {
            AudioManager.instance.PlayHideActionCard();
        }
    }
}
