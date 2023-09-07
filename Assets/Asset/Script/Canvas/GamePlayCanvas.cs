using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayCanvas : CanvasBase
{
    [Header("Field")]
    public GroundField groundField;

    [Header("Character Card")]
    public CharacterCard characterCardHandPrefab;
    private CharacterCard characterCardHand;

    [Header("Action Card")]
    public ActionCard actionCardHandPrefab;
    private ActionCard actionCardHand;

    [Header("Player Field")]
    public PlayerCharacterCardField playerCharacterCardField;
    public PlayerActionCardField playerActionCardField;

    [Header("Enemy Field")]
    public EnemyActionCardField enemyActionCardField;
    public EnemyCharacterCardField enemyCharacterCardField;
    public void Start()
    {
        SpawnInitialCharacterCard();
        SpawnInitialActionCard();
    }
    public void SpawnInitialCharacterCard()
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
    public void SpawnInitialActionCard()
    {
        //spawn action card player
        for (int i = 0; i < gamePlayManager.quantityInitialActionCard; i++)
        {
            actionCardHand = Instantiate(actionCardHandPrefab, playerActionCardField.transform);
            actionCardHand.GetCardData(playerManager.actionCardTakenDataList[i]);

            gamePlayManager.playerActionCardList.Add(actionCardHand);
        }

        //spawn action card enemy
        for (int i = 0; i < gamePlayManager.quantityInitialActionCard; i++)
        {
            actionCardHand = Instantiate(actionCardHandPrefab, enemyActionCardField.transform);
            //actionCardHand.CardBackState(true);
            actionCardHand.ManaState(false);
            enemyManager.actionCardTakenDataList.Add(enemyManager.actionCardDeckData[0]);
            enemyManager.actionCardDeckData.RemoveAt(0);
            actionCardHand.GetCardData(enemyManager.actionCardTakenDataList[i]);

            gamePlayManager.enemyActionCardList.Add(actionCardHand);
        }
    }
    public IEnumerator DrawCard(int value)
    {
        if (playerManager.actionCardDeckData.Count > 0)
        {
            for (int i = 0; i < value; i++)
            {
                actionCardHand = Instantiate(actionCardHandPrefab, playerActionCardField.transform);
                actionCardHand.GetCardData(playerManager.actionCardDeckData[0]);
                playerManager.actionCardTakenDataList.Add(playerManager.actionCardDeckData[0]);
                playerManager.actionCardDeckData.RemoveAt(0);

                gamePlayManager.playerActionCardList.Add(actionCardHand);
                yield return new WaitForSeconds(0.5f);
            }
        }
        if (enemyManager.actionCardDeckData.Count > 0)
        {
            for (int i = 0; i < value; i++)
            {
                actionCardHand = Instantiate(actionCardHandPrefab, enemyActionCardField.transform);
                //actionCardHand.CardBackState(true);
                actionCardHand.ManaState(false);
                actionCardHand.GetCardData(enemyManager.actionCardDeckData[0]);
                enemyManager.actionCardTakenDataList.Add(enemyManager.actionCardDeckData[0]);
                enemyManager.actionCardDeckData.RemoveAt(0);

                gamePlayManager.enemyActionCardList.Add(actionCardHand);
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
    public void PlayerActionCardHandState(bool state)
    {
        foreach(Transform card in playerActionCardField.transform)
        {
            card.gameObject.SetActive(state);
        }
    }
    public void EnemyActionCardHandState(bool state)
    {
        foreach (Transform card in enemyActionCardField.transform)
        {
            card.gameObject.SetActive(state);
        }
    }
}
