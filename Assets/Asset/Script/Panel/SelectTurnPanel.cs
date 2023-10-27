using System.Collections;
using System.Collections.Generic;
using System.Security;
using TMPro;
using UnityEngine;

public class SelectTurnPanel : PanelBase
{
    public bool selected = false;
    public bool isYourTurn;

    public float timeCount = 0f;
    public float timeDelay = 0f;
    public float timeEndCount = 5f;

    public float delayLoadText = 0.5f;
    public float delayEndSelect = 1.5f;
    public float delaySelectFirstTurn = 1f;

    public TextMeshProUGUI selectFirstTurnText;
    public TextMeshProUGUI LoadingText;
    public TextMeshProUGUI showTurnText;

    public Coroutine selectFirstTurnCoroutine;
    public Coroutine loadingCoroutine;
    private void OnEnable()
    {
        timeCount = 0;
        timeEndCount = Random.Range(timeEndCount, timeEndCount + 1);
        StopAllCoroutines();

        selectFirstTurnCoroutine = StartCoroutine(SelectFirstTurn());
        loadingCoroutine = StartCoroutine(LoadText());
    }
    private void Update()
    {
        if (timeCount >= timeEndCount)
        {
            timeCount = 0;
            StartCoroutine(HandleEndSelect());
        }
        else
        {
            timeCount += Time.deltaTime;
            float time = timeCount / timeEndCount;
            timeDelay = Random.Range(time - time/1.5f, time - time/2f);
        }
    }
    IEnumerator LoadText()
    {
        while (true)
        {
            LoadingText.text = "Choosing the first turn.";
            yield return new WaitForSeconds(delayLoadText);
            LoadingText.text = "Choosing the first turn..";
            yield return new WaitForSeconds(delayLoadText);
            LoadingText.text = "Choosing the first turn...";
            yield return new WaitForSeconds(delayLoadText);
        }
    }
    IEnumerator SelectFirstTurn()
    {
        yield return new WaitForSeconds(delaySelectFirstTurn);

        if (uiManager.tutorialCanvas != null)
            uiManager.tutorialCanvas.ActionTutorial(TutorialType.SelectTurnInitial);

        while (true)
        {
            selectFirstTurnText.text = "Your Turn";
            isYourTurn = true;
            yield return new WaitForSeconds(timeDelay);

            selectFirstTurnText.text = "Enemy Turn";
            isYourTurn = false;
            yield return new WaitForSeconds(timeDelay);
        }
    }
    IEnumerator HandleEndSelect()
    {
        StopCoroutine(selectFirstTurnCoroutine);
        StopCoroutine(loadingCoroutine);

        if (uiManager.tutorialCanvas.isActiveAndEnabled)
        {
            isYourTurn = true;
            selectFirstTurnText.text = "Your Turn";
        }
        if (isYourTurn)
            showTurnText.text = "You start first";
        else
            showTurnText.text = "Enemy starts first";

        yield return new WaitForSeconds(delayEndSelect);
        PanelState(false);
        gamePlayManager.UpdateGameState(GamePlayState.SelectInitialActionCard);
        if (isYourTurn)
        {
            gamePlayManager.UpdateTurnState(TurnState.YourTurn);
            gamePlayManager.playerAttackFirst = true;
        }
        else
        {
            gamePlayManager.UpdateTurnState(TurnState.EnemyTurn);
            gamePlayManager.enemyAttackFirst = true;
        }
        showTurnText.text = string.Empty;
    }
}
