using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
    public bool startCombat = false;
    public bool waitingTime = true;
    public bool selectedCardBattleInitial = false;

    public int battleCardSwitchCost = 10;
    public int quantityInitialActionCard = 5;

    public GamePlayState currentState;
    public TurnState currentTurn;
    public GamePlayCanvas gamePlayCanvas;
    public PlayerDeckData playerDeckData;
    public EnemyDeckData enemyDeckData;

    public List<ActionCardData> actionCardInitialDataList;


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
                    uiManager.battleCanvas.deckPanel.PanelState(true);
                    uiManager.battleCanvas.selectTurnPanel.PanelState(true);

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

                startCombat = true;

                gamePlayCanvas.CanvasState(true);

                HideTooltip();

                if (uiManager != null)
                {
                    uiManager.battleCanvas.selectInitialActionCardPanel.PanelState(false);

                    uiManager.battleCanvas.informationPanel.PanelState(true);
                    uiManager.battleCanvas.switchCardBattlePanel.PanelState(true);
                }
                break;
            case GamePlayState.DrawCards:

                HideTooltip();
                break;
            case GamePlayState.Victory:

                HideTooltip();
                break;
            case GamePlayState.Lose:

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
    public void HideTooltip()
    {
        tooltipManager.tooltipCanvas.tooltipController.StateObj(false);
    }
}
