using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayCardPanel : PanelBase
{
    public bool canShow = false;
    public float showTime = 1f;
    public Image cardImage;
    public Image backImage;

    public GameObject playCardObj;
    public GameObject cardImageObj;

    private ActionCardData currentCardData;
    private ActionCardDragHover currentCardDragHover;
    public void Update()
    {
        if (canShow)
        {
            if(ShowCardInfo() != null) StopCoroutine(ShowCardInfo());
            StartCoroutine(ShowCardInfo());
        }
    }
    public void PlayCard()
    {
        canShow = true;
        playerManager.ConsumeActionPoint(currentCardData.cardCost);
        Destroy(currentCardDragHover.placeHolder);
        currentCardDragHover.actionCard.PlayCard();
    }
    public void UnPlayCard()
    {
        PlayCardState(false);
        currentCardDragHover.ReturnCard();
    }
     public IEnumerator ShowCardInfo()
    {
        canShow = false;
        PlayCardState(false);
        CardImageObj(true);
        yield return new WaitForSeconds(showTime);
        CardImageObj(false);
    }
    public void GetInfoCard(ActionCardData actionCardData, ActionCardDragHover cardDragHover)
    {
        currentCardData = actionCardData;
        currentCardDragHover = cardDragHover;
        cardImage.sprite = currentCardData.cardSprite;
        backImage.color = currentCardData.actionCard.colorRarity;
        PlayCardState(true);
        CardImageObj(false);
    }
    public void PlayCardState(bool state)
    {
        playCardObj.SetActive(state);
    }
    public void CardImageObj(bool state)
    {
        cardImageObj.SetActive(state);
    }
}
