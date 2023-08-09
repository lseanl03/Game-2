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
            actionCardHand.GetOriginalCardInfo(playerManager.actionCardTakenList[i]);;
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
}
