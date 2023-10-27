using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardInfo : CardBase, IPointerDownHandler, IPointerClickHandler, IPointerUpHandler
{
    public bool isRecall = false;
    public Image recallCardImage;

    public CharacterCard characterCard;
    public ActionCard actionCard;
    public SupportCard supportCard;

    public CharacterStats characterStats;
    void Start()
    {
        characterCard = GetComponent<CharacterCard>();
        actionCard = GetComponent<ActionCard>();
        characterStats = GetComponent<CharacterStats>();
        supportCard = GetComponent<SupportCard>();
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            HandlePointerUp();
        }
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
                AudioManager.instance.PlayOnClickCharacterCard();
                tooltipManager.ShowCharacterCardTooltip(characterCard);
                CheckApplyStatus();
                if (uiManager.tutorialCanvas != null && !uiManager.tutorialCanvas.isShowedCharacterTutorial)
                {
                    uiManager.tutorialCanvas.isShowedCharacterTutorial = true;
                    uiManager.tutorialCanvas.ActionTutorial(TutorialType.CharacterTutorial);

                }
            }
            else if (actionCard != null)
            {
                AudioManager.instance.PlayOnClickActionCard();
                if (actionCard.cardBack.IsActive()) return;
                else tooltipManager.ShowActionCardTooltip(actionCard);
            }
            else if(supportCard != null)
            {
                AudioManager.instance.PlayOnClickActionCard();
                tooltipManager.ShowSupportCardTooltip(supportCard);
            }
        }

        if (gamePlayManager != null)
        {
            if (gamePlayManager.currentState == GamePlayState.SelectInitialActionCard && gamePlayManager.playerSelectedActionCardInitial == false)
            {
                ReCallImageState(isRecall = !isRecall);
            }
        }
        if(collectionManager.collectionCanvas.isActiveAndEnabled)
        {
            transform.DOScale(0.95f, 0.1f);
        }

    }
    public void HandlePointerUp()
    {
        if (collectionManager.collectionCanvas.isActiveAndEnabled)
        {
            transform.DOScale(1f, 0.1f);
        }
    }
    public void HandlePointerClick()
    {
        if (collectionManager.collectionCanvas.isActiveAndEnabled)
        {
            collectionManager.optionalToolCanvas.CanvasState(true);
            if (characterCard != null)
            {
                AudioManager.instance.PlayOnClickCharacterCard();
                collectionManager.optionalToolCanvas.GetCharacterCardInfo(characterCard);
            }
            if (actionCard != null)
            {
                AudioManager.instance.PlayOnClickActionCard();
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
        tooltipManager.tooltipCanvas.characterCardTooltip.StatusDescriptionObjState(true);
        tooltipManager.tooltipCanvas.characterCardTooltip.GetStatusInfo(characterStats.statusList);
        tooltipManager.tooltipCanvas.characterCardTooltip.GetBreakingStatusInfo(characterStats.breakingList);
        if (!characterStats.isApplyingStatus && !characterStats.isApplyBreaking)
        {
            tooltipManager.tooltipCanvas.characterCardTooltip.StatusDescriptionObjState(false);
        }
    }
}
