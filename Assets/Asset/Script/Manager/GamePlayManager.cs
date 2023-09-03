using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
    [Header("State")]
    public GamePlayState currentState;
    public TurnState currentTurn;

    public bool canAction = false;

    [Header("Phase")]
    public bool startPhase = false;
    public bool endPhase = false;
    public bool actionPhase = false;
    [Header("Attack First")]
    public bool playerAttackFirst = false;
    public bool enemyAttackFirst = false;
    [Header("EndingRound")]
    public bool playerEndingRound = false;
    public bool enemyEndingRound = false;

    [Header("SelectedCharacter")]
    public bool playerSelectedCharacterBattleInitial = false;
    public bool enemySelectedCharacterBattleInitial = false;

    [Header("Selected Action Card")]
    public bool playerSelectedActionCardInitial = false;
    public bool enemySelectedActionCardInitial = false;

    [Header("Quantity")]
    public int battleCardSwitchCost = 10;
    public int quantityInitialActionCard = 5;


    [Header("Player Card List")]
    public List<CharacterCard> playerCharacterList;
    public List<ActionCard> playerActionCardList;

    [Header("Enemy Card List")]
    public List<CharacterCard> enemyCharacterList;
    public List<ActionCard> enemyActionCardList;

    [Header("Canvas")]
    public GamePlayCanvas gamePlayCanvas;



    public static GamePlayManager instance;
    protected UIManager uiManager => UIManager.instance;
    protected TooltipManager tooltipManager => TooltipManager.instance;
    protected NotificationManager notificationManager => NotificationManager.instance;
    protected PlayerManager playerManager => PlayerManager.instance;
    protected EnemyManager enemyManager => EnemyManager.instance;
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
        if (playerEndingRound && enemyEndingRound && !endPhase)
        {
            UpdateGameState(GamePlayState.EndPhase);
        }
    }

    //---------------------------Update State---------------------------
    public void UpdateGameState(GamePlayState gameState)
    {
        currentState = gameState;
        switch (currentState)
        {
            case GamePlayState.SelectFirstTurn:
                StartCoroutine(HandleSelectFirstTurn());
                break;

            case GamePlayState.SelectInitialActionCard:
                StartCoroutine(HandleSelectInitialActionCard());
                HideTooltip();
                break;

            case GamePlayState.SelectInitialBattleCharacter:
                StartCoroutine(HandleSelectInitialBattleCharacter());
                HideTooltip();
                break;

            case GamePlayState.ActionPhase:
                StartCoroutine(HandleActionPhase());
                HideTooltip();
                break;

            case GamePlayState.StartPhase:
                StartCoroutine(HandleStartPhase());
                HideTooltip();
                break;

            case GamePlayState.EndPhase:
                StartCoroutine(HandleEndPhase());
                HideTooltip();
                break;

            case GamePlayState.Victory:
                HideTooltip();
                break;

            case GamePlayState.Lose:
                HideTooltip();
                break;

            default:
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

    //---------------------------Handle Phase---------------------------
    IEnumerator HandleSelectFirstTurn()
    {
        yield return null;
        if (uiManager != null)
        {
            uiManager.battleCanvas.CanvasState(true);
            uiManager.battleCanvas.selectTurnPanel.PanelState(true);
            uiManager.battleCanvas.skillPanel.PanelState(false);
            uiManager.battleCanvas.informationPanel.PanelState(false);
            uiManager.battleCanvas.selectInitialActionCardPanel.PanelState(false);
            uiManager.battleCanvas.switchCardBattlePanel.PanelState(false);
        }
    }
    IEnumerator HandleSelectInitialBattleCharacter()
    {
        if (uiManager != null)
        {
            uiManager.battleCanvas.selectInitialActionCardPanel.PanelState(false);
            uiManager.battleCanvas.informationPanel.PanelState(true);
            uiManager.battleCanvas.switchCardBattlePanel.PanelState(true);
        }
        gamePlayCanvas.CanvasState(true);
        yield return new WaitForSeconds(1);
        notificationManager.SetNewNotification("Select your first character");
    }
    IEnumerator HandleSelectInitialActionCard()
    {
        yield return null;
        if (uiManager != null)
        {
            uiManager.battleCanvas.selectInitialActionCardPanel.PanelState(true);
        }
    }
    IEnumerator HandleActionPhase()
    {
        if (actionPhase == false)
        {
            yield return new WaitForSeconds(1);
            notificationManager.SetNewNotification("Action Phase");
            actionPhase = true;
            startPhase = false;
            endPhase = false;
        }
    }
    IEnumerator HandleStartPhase()
    {
        if (startPhase == false)
        {
            startPhase = true;
            endPhase = false;
            actionPhase = false;
            playerEndingRound = false;
            enemyEndingRound = false;
            yield return new WaitForSeconds(1);
            notificationManager.SetNewNotification("Start Phase");
            yield return new WaitForSeconds(1);
            playerManager.ResetActionPoint();
            enemyManager.ResetActionPoint();
            yield return new WaitForSeconds(1);
            SetFirstTurn();
            yield return new WaitForSeconds(1);
            UpdateGameState(GamePlayState.ActionPhase);
        }
    }
    IEnumerator HandleEndPhase()
    {
        if (endPhase == false)
        {
            endPhase = true;
            actionPhase = false;
            startPhase = false;
            yield return new WaitForSeconds(1);
            notificationManager.SetNewNotification("End Phase");
            yield return new WaitForSeconds(1);
            StartCoroutine(gamePlayCanvas.DrawCard(2));
            yield return new WaitForSeconds(1);
            UpdateGameState(GamePlayState.StartPhase);
        }
    }
    public void HandleVictory()
    {

    }
    public void HandleLose()
    {

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
    public void UnPlayCard()
    {
        uiManager.battleCanvas.playCardPanel.UnPlayCard();
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
    public IEnumerator PlayerEndRound()
    {
        playerEndingRound = true;
        notificationManager.SetNewNotification("Your is ending round");
        if (!enemyEndingRound)
        {
            playerAttackFirst = true;
            yield return new WaitForSeconds(1);
            UpdateTurnState(TurnState.EnemyTurn);
        }
    }
    public IEnumerator EnemyEndRound()
    {
        enemyEndingRound = true;
        notificationManager.SetNewNotification("Enemy is ending round");
        if (!playerEndingRound)
        {
            enemyAttackFirst = true;
            yield return new WaitForSeconds(1);
            UpdateTurnState(TurnState.YourTurn);
        }
    }
    public void SetFirstTurn()
    {
        if (playerAttackFirst)
        {
            playerAttackFirst = false;
            notificationManager.SetNewNotification("Your attack first");
            UpdateTurnState(TurnState.YourTurn);
        }
        else if (enemyAttackFirst)
        {
            enemyAttackFirst = false;
            notificationManager.SetNewNotification("Enemy attack first");
            UpdateTurnState(TurnState.EnemyTurn);
        }
    }
}
