using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardInfo : CardBase, IPointerDownHandler, IPointerClickHandler
{
    public bool isRecall = false;
    public Image recallCardImage;

    public CharacterCard characterCard;
    public ActionCard actionCard;

    public CharacterStats characterStats;
    void Start()
    {
        characterCard = GetComponent<CharacterCard>();
        actionCard = GetComponent<ActionCard>();
        characterStats = GetComponent<CharacterStats>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            HandlePointerDown();
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            HandlePointerClick();
        }
    }
    public void HandlePointerDown()
    {
        if (tooltipManager != null)
        {
            if (characterCard != null)
            {
                tooltipManager.ShowCharacterCardTooltip(characterCard.characterCardData);
                gamePlayManager.HideHighlightsCard();
                CheckApplyStatus();
            }
            else if (actionCard != null)
            {
                gamePlayManager.HideHighlightsCard();
                if (actionCard.cardBack.IsActive()) return;
                else tooltipManager.ShowActionCardTooltip(actionCard.actionCardData);
            }
        }

        if (gamePlayManager != null)
        {
            if (gamePlayManager.currentState == GamePlayState.SelectInitialActionCard && gamePlayManager.playerSelectedActionCardInitial == false)
            {
                ReCallImageState(isRecall = !isRecall);
            }
        }

    }
    public void HandlePointerClick()
    {
        if (collectionManager != null)
        {
            collectionManager.optionalToolCanvas.CanvasState(true);
            if (characterCard != null)
            {
                collectionManager.optionalToolCanvas.GetCharacterCardInfo(characterCard);
            }
            if (actionCard != null)
            {
                collectionManager.optionalToolCanvas.GetActionCardInfo(actionCard);
            }
        }
    }
    public void ReCallImageState(bool state)
    {
        isRecall = state;
        recallCardImage.gameObject.SetActive(isRecall);
    }
    public void CheckApplyStatus()
    {
        if (characterStats.isApplyingStatus)
        {
            tooltipManager.tooltipCanvas.characterCardTooltip.StatusDescriptionObjState(true);
            foreach (Status status in characterStats.statusList)
            {
                tooltipManager.tooltipCanvas.characterCardTooltip.
                    GetStatusInfo(status.statusSprite, status.statusName, status.statusDescription, characterStats.statusList.Count);
            }
        }
        else
        {
            tooltipManager.tooltipCanvas.characterCardTooltip.StatusDescriptionObjState(false);
        }
    }
}
