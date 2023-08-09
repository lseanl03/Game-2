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
    public CharacterCard characterCard;
    public CharacterStats characterStats;
    protected GamePlayManager gamePlayManager => GamePlayManager.instance;
    protected UIManager uiManager => UIManager.instance;
    public void Start()
    {
        characterCard = GetComponent<CharacterCard>();
        characterStats  = GetComponent<CharacterStats>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if(transform.parent == gamePlayManager.gamePlayCanvas.playerCharacterCardField.transform)
        HandleSelectCard();
    }
    public void SelectIconState(bool state)
    {
        isSelecting = state;
        selectIcon.gameObject.SetActive(state);
    }
    public void HandleCardSelecting()
    {
        if(transform.parent == gamePlayManager.gamePlayCanvas.playerCharacterCardField.transform)
        {
            SelectIconState(false);
            transform.localPosition = new Vector2(transform.localPosition.x, 30f);
            isSelected = true;
            characterStats.isActionCharacter = isSelected;
            uiManager.battleCanvas.skillPanel.PanelState(isSelected);
            uiManager.battleCanvas.skillPanel.GetCharacterCard(characterCard.characterCardData, characterCard);
            CharacterSkill[] skill = characterCard.characterCardData.characterCard.characterSkillList.ToArray();
            if (skill.Length >= 2)
            {
                uiManager.battleCanvas.skillPanel.SetSkillImage(skill[0].skillSprite, skill[1].skillSprite, skill[2].skillSprite);
                uiManager.battleCanvas.skillPanel.SetCostText(skill[0].actionPointCost, skill[1].actionPointCost, skill[2].actionPointCost);
            }
        }
        else
        {
            transform.localPosition = new Vector2(transform.localPosition.x, -30f);
            isSelected = true;
            characterStats.isActionCharacter = isSelected;
        }
    }
    public void HandleCardSelected()
    {
        transform.localPosition = new Vector2(transform.localPosition.x, 0f);
        isSelected = false;
        characterStats.isActionCharacter = isSelected;
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
                        uiManager.battleCanvas.skillPanel.PanelState(!isSelecting);
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
