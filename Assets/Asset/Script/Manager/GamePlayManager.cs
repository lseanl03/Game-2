using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
    public bool startCombat = false;
    public bool endCombat = false;
    public bool isFighting = false;
    public bool waitingTime = true;
    public bool selectedCardBattleInitial = false;


    public int battleCardSwitchCost = 10;
    public int quantityInitialActionCard = 5;

    public GamePlayCanvas gamePlayCanvas;

    [Header("State")]
    public GamePlayState currentState;
    public TurnState currentTurn;

    [Header("Data")]
    public PlayerDeckData playerDeckData;
    public EnemyDeckData enemyDeckData;

    [Header("Character Card List")]
    public List<CharacterCard> playerCharacterList;
    public List<CharacterCard> enemyCharacterList;


    public static GamePlayManager instance;
    protected UIManager uiManager => UIManager.instance;
    protected TooltipManager tooltipManager => TooltipManager.instance;
    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        gamePlayCanvas.CanvasState(false);
    }
    private void Start()
    {
        UpdateGameState(GamePlayState.SelectFirstTurn);
    }

    public void UpdateGameState(GamePlayState gameState)
    {
        currentState = gameState;
        switch (currentState)
        {
            case GamePlayState.SelectFirstTurn:
                if(uiManager != null)
                {
                    uiManager.battleCanvas.CanvasState(true);
                    uiManager.battleCanvas.selectTurnPanel.PanelState(true);
                    uiManager.battleCanvas.skillPanel.PanelState(false);
                    uiManager.battleCanvas.informationPanel.PanelState(false);
                    uiManager.battleCanvas.selectInitialActionCardPanel.PanelState(false);
                    uiManager.battleCanvas.switchCardBattlePanel.PanelState(false);
                }
                break;

            case GamePlayState.SelectInitialActionCard:
                HideTooltip();
                if (uiManager != null)
                {
                    uiManager.battleCanvas.selectInitialActionCardPanel.PanelState(true);
                }
                break;

            case GamePlayState.SelectInitialBattleCharacterCard:
                gamePlayCanvas.CanvasState(true);
                HideTooltip();

                if (uiManager != null)
                {
                    uiManager.battleCanvas.selectInitialActionCardPanel.PanelState(false);
                    uiManager.battleCanvas.informationPanel.PanelState(true);
                    uiManager.battleCanvas.switchCardBattlePanel.PanelState(true);
                }
                break;

            case GamePlayState.Combat:
                isFighting = true;
                HideTooltip();
                break;

            case GamePlayState.StartCombat:
                startCombat = true;
                HideTooltip();
                break;

            case GamePlayState.EndCombat:
                endCombat = true;
                HideTooltip();
                break;

            case GamePlayState.DrawCards:

                HideTooltip();
                break;

            case GamePlayState.Victory:
                isFighting = false;
                HideTooltip();
                break;

            case GamePlayState.Lose:
                isFighting = false;
                HideTooltip();
                break;

            default:
                Debug.Log("Out Case");
                break;
        }
    }
    public void UpdateTurnState(TurnState turnState)
    {
        currentTurn = turnState;
        switch(currentTurn)
        {
            case TurnState.YourTurn: 
                break;
            case TurnState.EnemyTurn:
                break;
        }
    }
    public void HighlightCardTarget(ActionTargetType actionTargetType, int actionValue)
    {
        switch (actionTargetType)
        {
            case ActionTargetType.Ally:
                foreach(CharacterCard characterCard in playerCharacterList)
                {
                    if (characterCard.characterStats.isActionCharacter)
                    {
                        characterCard.SetHighlight(true);
                        characterCard.SetValueReceived(actionValue);
                    }
                }
                break;

            case ActionTargetType.Enemy:
                foreach(CharacterCard characterCard in enemyCharacterList)
                {
                    if (characterCard.characterStats.isActionCharacter)
                    {
                        characterCard.SetHighlight(true);
                        characterCard.SetValueReceived(actionValue);
                    }
                }
                break;

            case ActionTargetType.AllAllies:
                foreach (CharacterCard characterCard in playerCharacterList)
                {
                    characterCard.SetHighlight(true);
                    characterCard.SetValueReceived(actionValue);
                }
                break;

            case ActionTargetType.AllEnemies:
                foreach (CharacterCard characterCard in enemyCharacterList)
                {
                    characterCard.SetHighlight(true);
                    characterCard.SetValueReceived(actionValue);
                }
                break;

            case ActionTargetType.ChooseAlly:
                foreach (CharacterCard characterCard in playerCharacterList)
                {
                    if (characterCard.characterStats.isChoosing)
                    {
                        characterCard.SetHighlight(true);
                        characterCard.SetValueReceived(actionValue);
                    }
                }
                break;

            case ActionTargetType.ChooseEnemy:
                foreach (CharacterCard characterCard in enemyCharacterList)
                {
                    if (characterCard.characterStats.isChoosing)
                    {
                        characterCard.SetHighlight(true);
                        characterCard.SetValueReceived(actionValue);
                    }
                }
                break;
        }
    }
    public void HideHighlightsCard()
    {
        uiManager.battleCanvas.skillPanel.isHighlightActive = false;

        foreach (CharacterCard player in playerCharacterList)
            player.SetHighlight(false);

        foreach (CharacterCard enemy in enemyCharacterList)
            enemy.SetHighlight(false);
    }
    public void HideTooltip()
    {
        uiManager.HideTooltip();
    }
    public void HideSwitchCardBattle()
    {
        if (currentState == GamePlayState.Combat)
        {
            uiManager.HideSwitchCardBattle();
            uiManager.ShowSkill();
            HideSelectIcon();
        }
    }
    public void HideSelectIcon()
    {
        foreach (CharacterCard player in playerCharacterList)
            player.characterCardDragHover.SelectIconState(false);

        foreach (CharacterCard enemy in enemyCharacterList)
            enemy.characterCardDragHover.SelectIconState(false);
    }
    public void DealDamageToTargets(ActionTargetType actionTargetType,  int damage)
    {
        switch (actionTargetType)
        {
            case ActionTargetType.Ally:
                foreach (CharacterCard characterCard in playerCharacterList)
                {
                    if (characterCard.characterStats.isActionCharacter)
                    {
                        characterCard.characterStats.TakeDamage(damage);
                    }
                }
                break;

            case ActionTargetType.Enemy:
                foreach (CharacterCard characterCard in enemyCharacterList)
                {
                    if (characterCard.characterStats.isActionCharacter)
                    {
                        characterCard .characterStats.TakeDamage(damage);
                    }
                }
                break;
        }
    }
}
