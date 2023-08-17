using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
    public bool startPhase = false;
    public bool endPhase = false;
    public bool actionPhase = false;

    public bool playerEndingRound = false;
    public bool enemyEndingRound = false;

    public bool playerSelectedCharacterBattleInitial = false;
    public bool enemySelectedCharacterBattleInitial = false;

    public bool playerSelectedActionCardInitial = false;
    public bool enemySelectedActionCardInitial = false;


    public int battleCardSwitchCost = 10;
    public int quantityInitialActionCard = 5;

    public GamePlayCanvas gamePlayCanvas;

    [Header("State")]
    public GamePlayState currentState;
    public TurnState currentTurn;

    [Header("Player Card List")]
    public List<CharacterCard> playerCharacterList;
    public List<ActionCard> playerActionCardList;

    [Header("Enemy Card List")]
    public List<CharacterCard> enemyCharacterList;
    public List<ActionCard> enemyActionCardList;


    public static GamePlayManager instance;
    protected UIManager uiManager => UIManager.instance;
    protected TooltipManager tooltipManager => TooltipManager.instance;
    protected NotificationManager notificationManager => NotificationManager.instance;
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
    private void Update()
    {
        if (playerEndingRound && enemyEndingRound)
        {
            playerEndingRound = false;
            enemyEndingRound = false;
            UpdateGameState(GamePlayState.EndPhase);
        }
    }
    public void EnemyEndRound()
    {
        enemyEndingRound = true;
        if (playerEndingRound == false)
        {
            UpdateTurnState(TurnState.YourTurn);
        }
        else
        {
            UpdateGameState(GamePlayState.EndPhase);
        }
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
                notificationManager.SetNewNotification("Select your first character");
                gamePlayCanvas.CanvasState(true);
                HideTooltip();

                if (uiManager != null)
                {
                    uiManager.battleCanvas.selectInitialActionCardPanel.PanelState(false);
                    uiManager.battleCanvas.informationPanel.PanelState(true);
                    uiManager.battleCanvas.switchCardBattlePanel.PanelState(true);
                }
                break;

            case GamePlayState.ActionPhase:
                if(actionPhase == false)
                {
                    actionPhase = true;
                    startPhase = false;
                    endPhase = false;
                    notificationManager.SetNewNotification("Action Phase");
                }
                HideTooltip();
                break;

            case GamePlayState.StartPhase:
                if(startPhase == false)
                {
                    startPhase = true;
                    endPhase = false;
                    actionPhase = false;
                    notificationManager.SetNewNotification("Start Phase");
                }
                HideTooltip();
                break;

            case GamePlayState.EndPhase:
                if(endPhase == false)
                {
                    endPhase = true;
                    actionPhase = false;
                    startPhase = false;
                    notificationManager.SetNewNotification("End Phase");
                    StartCoroutine(gamePlayCanvas.DrawCard(2));
                }
                HideTooltip();
                break;

            case GamePlayState.Victory:
                actionPhase = false;
                HideTooltip();
                break;

            case GamePlayState.Lose:
                actionPhase = false;
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

                if(actionPhase)
                notificationManager.SetNewNotification("Your Turn");
                break;
            case TurnState.EnemyTurn:
                if(actionPhase)
                notificationManager.SetNewNotification("Enemy Turn");

                HideHighlightsCard();
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
        uiManager.battleCanvas.skillPanel.HideHighLight();
        foreach (CharacterCard player in playerCharacterList)
        {
            player.SetHighlight(false);
        }
        foreach (CharacterCard enemy in enemyCharacterList)
        {
            enemy.SetHighlight(false);
        }
    }
    public void HideTooltip()
    {
        uiManager.HideTooltip();
    }
    public void HideSwitchCardBattle()
    {
        if (currentState == GamePlayState.ActionPhase)
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
