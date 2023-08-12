using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : ControllerBase
{
    public float mixRandom = 0f;
    public float maxRandom = 3f;
    public bool enemySelectedCardBattleInitial = false; 
    public void Start()
    {
    }
    private void Update()
    {
        if (gamePlayManager.playerSelectedCardBattleInitial && !gamePlayManager.actionPhase)
        {
            StartCoroutine(InitialBattleCharacterCard());
        }
        if (gamePlayManager.actionPhase)
        {
            if(gamePlayManager.currentTurn == TurnState.EnemyTurn)
            {
                StartCoroutine(Combat());
            }
        }
        if(enemyManager.currentActionPoint <= 50)
        {
            StartCoroutine(EndRound());
        }
    }
    IEnumerator InitialBattleCharacterCard()
    {
        yield return new WaitForSeconds(Random.Range(mixRandom, maxRandom));
        gamePlayManager.enemyCharacterList[0].characterCardDragHover.HandleCardSelecting();
        enemySelectedCardBattleInitial = true;
        gamePlayManager.UpdateGameState(GamePlayState.ActionPhase);
        StopCoroutine(InitialBattleCharacterCard());
    }
    IEnumerator Combat()
    {
        yield return new WaitForSeconds(Random.Range(mixRandom, maxRandom));
    }
    IEnumerator EndRound()
    {
        yield return new WaitForSeconds(Random.Range(mixRandom, maxRandom));
        gamePlayManager.EnemyEndRound();
        StopCoroutine(EndRound());
    }
}
