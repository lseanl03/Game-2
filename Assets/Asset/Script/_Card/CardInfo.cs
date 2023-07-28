using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardInfo : MonoBehaviour, IPointerDownHandler, IPointerClickHandler
{
    public bool isRecall = false;
    public Image recallCardImage;

    public CharacterCard characterCard;
    public ActionCard actionCard;
    public CollectionManager collectionManager => CollectionManager.instance;
    public TooltipManager tooltipManager => TooltipManager.instance;
    public GamePlayManager gamePlayManager => GamePlayManager.instance;
    void Start()
    {
        characterCard = GetComponent<CharacterCard>();
        actionCard = GetComponent<ActionCard>();
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
            tooltipManager.tooltipCanvas.tooltipController.StateObj(true);
            if(characterCard != null)
            {
                tooltipManager.tooltipCanvas.GetCharacterCardInfo(characterCard.characterCardData);
            }
            if (actionCard != null)
            {
                tooltipManager.tooltipCanvas.GetActionCardInfo(actionCard.actionCardData);
            }
        }

        if (gamePlayManager != null)
        {
            if (gamePlayManager.currentState == GamePlayState.SelectInitialActionCard)
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

}
