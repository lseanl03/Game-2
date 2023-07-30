using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterCardDragHover : MonoBehaviour, IPointerDownHandler
{
    public bool isSelecting = false;
    public bool isSelected = false;
    public Image selectIcon;
    protected GamePlayManager gamePlayManager => GamePlayManager.instance;
    protected UIManager uiManager => UIManager.instance;
    public void OnPointerDown(PointerEventData eventData)
    {
        HandleSelectCard();
    }
    public void SelectIconState(bool state)
    {
        isSelecting = state;
        selectIcon.gameObject.SetActive(state);
    }
    public void HandleCardSelecting()
    {
        SelectIconState(false);
        transform.localPosition = new Vector2(transform.localPosition.x, 30f);
        isSelected = true;
    }
    public void HandleCardSelected()
    {
        transform.localPosition = new Vector2(transform.localPosition.x, 0f);
        isSelected = false;
    }
    public void HandleSelectCard()
    {
        if(gamePlayManager.currentTurn == TurnState.YourTurn || !gamePlayManager.selectedCardBattleInitial)
        {
            for (int i = 0; i < transform.parent.childCount; i++)
            {
                CharacterCardDragHover characterCard = transform.parent.GetChild(i).GetComponent<CharacterCardDragHover>();
                if (this.Equals(characterCard))
                {
                    if (!isSelected && !isSelecting)
                    {
                        SelectIconState(true);
                        uiManager.battleCanvas.switchCardBattlePanel.PanelState(isSelecting);
                    }
                }
                else
                {
                    if (characterCard.isSelecting && !characterCard.isSelected)
                    {
                        characterCard.SelectIconState(false);
                    }
                }
            }
        }

        if (gamePlayManager.gamePlayCanvas.playerActionCardField.isZooming)
        {
            gamePlayManager.gamePlayCanvas.playerActionCardField.ZoomState(false);
        }
    }
}
