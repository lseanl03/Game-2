using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterCardDragHover : MonoBehaviour, IPointerDownHandler
{
    public bool isSelecting = false;
    public bool isSelected = false;
    public float pushDistance = 30f;
    public float pushTime = 0.25f;
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
    public void Update()
    {
        
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
        characterCard.PlaySound(SoundType.ActionCharacter);
        if(transform.parent == gamePlayManager.gamePlayCanvas.playerCharacterCardField.transform)
        {
            SelectIconState(false);
            transform.DOLocalMove(new Vector3(transform.localPosition.x, pushDistance, 0), pushTime);
            isSelected = true;
            characterStats.isActionCharacter = isSelected;
            uiManager.battleCanvas.skillPanel.GetCharacterCard(characterCard.characterCardData, characterCard);
            uiManager.battleCanvas.skillPanel.PanelState(isSelected);

            CharacterSkill[] skill = characterCard.characterCardData.characterCard.characterSkillList.ToArray();
            if (skill.Length >= 2)
            {
                uiManager.battleCanvas.skillPanel.SetSkillImage(skill[0].skillSprite, skill[1].skillSprite, skill[2].skillSprite);
                uiManager.battleCanvas.skillPanel.SetActionPointCostText
                    (characterCard.currentNAActionPointCost, characterCard.currentESActionPointCost, characterCard.currentEBActionPointCost);
            }
        }
        else
        {
            transform.DOLocalMove(new Vector3(transform.localPosition.x, -pushDistance, 0), pushTime);
            isSelected = true;
            characterStats.isActionCharacter = isSelected;
        }
        if (gamePlayManager.playerCanSwitchCharacterDying)
        {
            gamePlayManager.playerCanSwitchCharacterDying = false;
        }
        else if (gamePlayManager.enemyCanSwitchCharacterDying)
        {
            gamePlayManager.enemyCanSwitchCharacterDying= false;
        }
    }
    public void HandleCardSelected()
    {
        transform.DOLocalMove(new Vector3(transform.localPosition.x, 0, 0), pushTime);
        isSelected = false;
        characterStats.isActionCharacter = isSelected;
    }
    public void HandleSelectCard()
    {
        if (characterStats.isDead || !gamePlayManager.enemySelectedCharacterBattleInitial && 
            gamePlayManager.playerSelectedCharacterBattleInitial || gamePlayManager.playerAttacking || characterStats.isActionCharacter) return;

        if(gamePlayManager.currentState == GamePlayState.SelectBattleCharacter || gamePlayManager.playerCanSwitchCharacterDying || 
            gamePlayManager.currentTurn == TurnState.YourTurn && gamePlayManager.actionPhase)
        {
            for (int i = 0; i < transform.parent.childCount; i++)
            {
                CharacterCardDragHover characterCard = transform.parent.GetChild(i).GetComponent<CharacterCardDragHover>();
                if (characterCard != null)
                {
                    if (this.Equals(characterCard))
                    {
                        if (!isSelected && !isSelecting)
                        {
                            ShowSwitchCharacter();
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
                gamePlayManager.gamePlayCanvas.playerActionCardField.ZoomState(false);
        }
    }
    public void ShowSwitchCharacter()
    {

        SelectIconState(true);
        uiManager.battleCanvas.switchCardBattlePanel.PanelState(isSelecting);
        uiManager.battleCanvas.skillPanel.PanelState(!isSelecting);

        if(gamePlayManager.actionPhase && !gamePlayManager.playerCanSwitchCharacterDying)
        {
            uiManager.battleCanvas.switchCardBattlePanel.ActionCostState(true);
        }

        if (!uiManager.tutorialCanvas.isShowedSwitchCharacterTutorial && uiManager.tutorialCanvas.isShowedActionCardTutorial)
        {
            uiManager.tutorialCanvas.isShowedSwitchCharacterTutorial = true;
            uiManager.tutorialCanvas.ActionTutorial(TutorialType.SwitchCharacterTutorial);
        }
    }
}
