using System.Collections;
using System.Collections.Generic;
using System.Security;
using TMPro;
using UnityEngine;

public class SelectTurnPanel : PanelBase
{
    public bool selected = false;
    public bool isYourTurn;
    public float delayTimeDisplay = 0.5f;
    public float timeCount = 0f;
    public float timeEndCount = 5f;
    public float timeDelay = 0f;
    public TextMeshProUGUI selectFirstTurnText;
    public TextMeshProUGUI LoadingText;
    public Coroutine selectFirstTurnCoroutine;
    public Coroutine loadingCoroutine;
    private void Start()
    {
        timeEndCount = Random.Range(timeEndCount, timeEndCount + 1);
        selectFirstTurnCoroutine = StartCoroutine(SelectFirstTurn());
        loadingCoroutine = StartCoroutine(LoadText());
    }
    private void Update()
    {
        if (timeCount >= timeEndCount)
        {
            if(!selected)
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
            yield return new WaitForSeconds(delayTimeDisplay);
            LoadingText.text = "Choosing the first turn..";
            yield return new WaitForSeconds(delayTimeDisplay);
            LoadingText.text = "Choosing the first turn...";
            yield return new WaitForSeconds(delayTimeDisplay);
        }
    }
    IEnumerator SelectFirstTurn()
    {
        yield return new WaitForSeconds(1f);
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
        selected = false;

        StopCoroutine(selectFirstTurnCoroutine);
        StopCoroutine(loadingCoroutine);

        yield return new WaitForSeconds(2);
        PanelState(false);

        gamePlayManager.UpdateGameState(GamePlayState.SelectInitialActionCard);
        if (isYourTurn) 
            gamePlayManager.UpdateTurnState(GamePlayState.YourTurn);
        else 
            gamePlayManager.UpdateTurnState(GamePlayState.EnemyTurn);

    }
}
