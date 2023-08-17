using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InformationPanel : PanelBase
{
    public float endRoundTime = 30;
    public float endRoundCountDown;
    public TextMeshProUGUI turnText;
    public TextMeshProUGUI roundText;
    public Button endRoundButton;
    public Slider endRoundSlider;

    public void Start()
    {
        endRoundSlider.minValue = 0;
        endRoundSlider.maxValue = endRoundTime;
        SetEndRoundSlider();
    }

    public void Update()
    {
        if (gamePlayManager.currentTurn == TurnState.YourTurn)
            SetTurnText("Your Turn");
        else if (gamePlayManager.currentTurn == TurnState.EnemyTurn)
            SetTurnText("Enemy Turn");
    }
    public void OnEnable()
    {
        endRoundCountDown = endRoundTime;
    }
    public void SetTurnText(string text)
    {
        turnText.text = text;
    }
    public void EndRound()
    {
        if(gamePlayManager.currentTurn == TurnState.YourTurn)
        {
            endRoundCountDown = 0;
            notificationManager.SetNewNotification("I'm ending my round");
            gamePlayManager.playerEndingRound = true;
        }
        else if(gamePlayManager.currentTurn == TurnState.EnemyTurn)
        {
            notificationManager.SetNewNotification("It's the enemy turn");
        }
    }
    public void SetEndRoundTime()
    {
        if (gamePlayManager.actionPhase)
        {
            endRoundCountDown -= Time.deltaTime;
            if (endRoundCountDown <= 0)
            {
                if (gamePlayManager.currentTurn == TurnState.YourTurn)
                {
                    gamePlayManager.UpdateTurnState(TurnState.EnemyTurn);
                }
                else if(gamePlayManager.currentTurn == TurnState.EnemyTurn)
                {
                    gamePlayManager.UpdateTurnState(TurnState.YourTurn);
                }
                endRoundCountDown = endRoundTime;
            }
        }
    }
    public void SetEndRoundSlider()
    {
        endRoundSlider.value = endRoundSlider.maxValue;
    }
}
