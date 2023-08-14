using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayCanvas : CanvasBase
{
    [Header("CharacterCard")]
    private CharacterCard characterCardHand;
    public CharacterCard characterCardHandPrefab;

    [Header("ActionCard")]
    private ActionCard actionCardHand;
    public ActionCard actionCardHandPrefab;

    [Header("Player")]
    public PlayerCharacterCardField playerCharacterCardField;
    public PlayerActionCardField playerActionCardField;

    [Header("EnemyField")]
    public EnemyActionCardField enemyActionCardField;
    public EnemyCharacterCardField enemyCharacterCardField;
    public void Start()
    {
        DrawInitialCharacterCard();
        DrawInitialActionCard();
    }
    public void DrawInitialCharacterCard()
    {
        //spawn character player
        if(playerManager.characterCardDeckData.Count > 0)
        {
            for (int i = 0; i < playerManager.characterCardDeckData.Count; i++)
            {
                characterCardHand = Instantiate(characterCardHandPrefab, playerCharacterCardField.transform);
                characterCardHand.GetOriginalCardInfo(playerManager.characterCardDeckData[i]);

                gamePlayManager.playerCharacterList.Add(characterCardHand);
            }
        }

        //spawn character enemy
        if(enemyManager.characterCardDeckData.Count > 0)
        {
            for(int i = 0;i < enemyManager.characterCardDeckData.Count; i++)
            {
                characterCardHand = Instantiate(characterCardHandPrefab, enemyCharacterCardField.transform);
                characterCardHand.GetOriginalCardInfo(enemyManager.characterCardDeckData[i]);

                gamePlayManager.enemyCharacterList.Add(characterCardHand);
            }
        }
    }
    public void DrawInitialActionCard()
    {
        //spawn action card player
        for (int i = 0; i < gamePlayManager.quantityInitialActionCard; i++)
        {
            actionCardHand = Instantiate(actionCardHandPrefab, playerActionCardField.transform);
            actionCardHand.GetOriginalCardInfo(playerManager.actionCardTakenList[i]);
        }

        //spawn action card enemy
        for (int i = 0; i < gamePlayManager.quantityInitialActionCard; i++)
        {
            actionCardHand = Instantiate(actionCardHandPrefab, enemyActionCardField.transform);
            actionCardHand.CardBackState(true);
            actionCardHand.ManaState(false);
            enemyManager.actionCardTakenList.Add(enemyManager.actionCardDeckData[0]);
            enemyManager.actionCardDeckData.RemoveAt(0);
            actionCardHand.GetOriginalCardInfo(enemyManager.actionCardTakenList[i]);
        }
    }
    public IEnumerator DrawCard(int value)
    {
        yield return new WaitForSeconds(1f);
        if (playerManager.actionCardDeckData.Count > 0)
        {
            for (int i = 0; i < value; i++)
            {
                actionCardHand = Instantiate(actionCardHandPrefab, playerActionCardField.transform);
                actionCardHand.GetOriginalCardInfo(playerManager.actionCardDeckData[0]);
                playerManager.actionCardTakenList.Add(playerManager.actionCardDeckData[0]);
                playerManager.actionCardDeckData.RemoveAt(0);
                yield return new WaitForSeconds(0.5f);
            }
        }
        if (enemyManager.actionCardDeckData.Count > 0)
        {
            for (int i = 0; i < value; i++)
            {
                actionCardHand = Instantiate(actionCardHandPrefab, enemyActionCardField.transform);
                actionCardHand.CardBackState(true);
                actionCardHand.ManaState(false);
                actionCardHand.GetOriginalCardInfo(enemyManager.actionCardDeckData[0]);
                enemyManager.actionCardTakenList.Add(enemyManager.actionCardDeckData[0]);
                enemyManager.actionCardDeckData.RemoveAt(0);
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}
