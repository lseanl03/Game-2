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

    public GameObject playerSkillPointObj;
    public GameObject enemySkillPointObj;

    public GameObject playerEndingRoundObj;
    public GameObject enemyEndingRoundObj;

    public void Start()
    {
        endRoundSlider.minValue = 0;
        endRoundSlider.maxValue = endRoundTime;
        PlayerEndingRoundObjState(false);
        EnemyEndingRoundObjState(false);
        SetEndRoundSlider();
    }

    public void Update()
    {
        if (gamePlayManager.currentTurn == TurnState.YourTurn)
            SetTurnText("Your Turn");
        else if (gamePlayManager.currentTurn == TurnState.EnemyTurn)
            SetTurnText("Enemy Turn");
    }
    public void PlayerEndingRoundObjState(bool state)
    {
        playerEndingRoundObj.SetActive(state);
    }
    public void EnemyEndingRoundObjState(bool state)
    {
        enemyEndingRoundObj.SetActive(state);
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
        if (gamePlayManager.playerAttacking || gamePlayManager.enemyAttacking || 
            gamePlayManager.playerCanSwitchCharacterDying || gamePlayManager.enemyCanSwitchCharacterDying ||
            gamePlayManager.playerEndingRound) return;

        if (!uiManager.tutorialCanvas.isShowedEndRoundTutorial)
        {
            uiManager.tutorialCanvas.isShowedEndRoundTutorial = true;
            uiManager.tutorialCanvas.ActionTutorial(TutorialType.EndRoundTutorial);
            return;
        }
        if (gamePlayManager.actionPhase)
        {
            if (gamePlayManager.gamePlayCanvas.playerActionCardField.isZooming)
                gamePlayManager.gamePlayCanvas.playerActionCardField.ZoomState(false);
            if (gamePlayManager.currentTurn == TurnState.YourTurn)
            {
                endRoundCountDown = 0;
                StartCoroutine(gamePlayManager.PlayerEndRound());
            }
            else if (gamePlayManager.currentTurn == TurnState.EnemyTurn)
            {
                notificationManager.SetNewNotification("It's the enemy turn");
            }
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
