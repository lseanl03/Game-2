using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InformationPanel : PanelBase
{
    public float endTurnTime = 30;
    public float endTurnCountDown;
    public TextMeshProUGUI turnText;
    public TextMeshProUGUI roundText;
    public Button endTurnButton;
    public Slider endTurnSlider;

    public void Start()
    {
        endTurnSlider.minValue = 0;
        endTurnSlider.maxValue = endTurnTime;
    }

    public void Update()
    {
        SetTurnText(gamePlayManager.currentTurn.ToString());
    }
    public void OnEnable()
    {
        endTurnCountDown = endTurnTime;
    }
    public void SetTurnText(string text)
    {
        turnText.text = text;
    }
    public void EndTurn()
    {
        if(gamePlayManager.currentTurn == TurnState.YourTurn)
        {
            endTurnCountDown = 0;
            notificationManager.SetNewNotification("I'm ending my round");
        }
        else if(gamePlayManager.currentTurn == TurnState.EnemyTurn)
        {
            notificationManager.SetNewNotification("It's the enemy turn");
        }
    }
    public void SetEndTurnTime()
    {
        if (gamePlayManager.isFighting)
        {
            endTurnCountDown -= Time.deltaTime;
            if (endTurnCountDown <= 0)
            {
                if (gamePlayManager.currentTurn == TurnState.YourTurn)
                {
                    gamePlayManager.UpdateTurnState(TurnState.EnemyTurn);
                }
                else if(gamePlayManager.currentTurn == TurnState.EnemyTurn)
                {
                    gamePlayManager.UpdateTurnState(TurnState.YourTurn);
                }
                endTurnCountDown = endTurnTime;
            }
        }
    }
    public void SetEndTurnSlider()
    {
        endTurnSlider.value = endTurnCountDown;
    }
}
