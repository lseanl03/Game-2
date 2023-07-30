using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayCanvas : CanvasBase
{
    [Header("CharacterCard")]
    public CharacterCard characterCardHandPrefab;
    public CharacterCard characterCardHand;
    public PlayerCharacterCardField playerCharacterCardField;
    public EnemyCharacterCardField enemyCharacterCardField;

    [Header("ActionCard")]
    public ActionCard actionCardHandPrefab;
    public ActionCard actionCardHand;
    public PlayerActionCardField playerActionCardField;
    public EnemyActionCardField enemyActionCardField;
    public void Start()
    {
        SpawnInitialCharacterCard();
        SpawnInitialActionCard();
    }
    public void OnEnable()
    {

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
            }
        }

        //spawn character enemy
        if(enemyManager.characterCardDeckData.Count > 0)
        {
            for(int i = 0;i < enemyManager.characterCardDeckData.Count; i++)
            {
                characterCardHand = Instantiate(characterCardHandPrefab, enemyCharacterCardField.transform);
                characterCardHand.GetOriginalCardInfo(enemyManager.characterCardDeckData[i]);
            }
        }
    }
    public void SpawnInitialActionCard()
    {
        //spawn action card player
        if (gamePlayManager.actionCardInitialDataList.Count > 0)
        {
            for (int i = 0; i < gamePlayManager.quantityInitialActionCard; i++)
            {
                actionCardHand = Instantiate(actionCardHandPrefab, playerActionCardField.transform);
                actionCardHand.GetOriginalCardInfo(gamePlayManager.actionCardInitialDataList[i]);
            }
        }

        //spawn action card enemy
        if (gamePlayManager.actionCardInitialDataList.Count > 0)
        {
            for (int i = 0; i < gamePlayManager.quantityInitialActionCard; i++)
            {
                actionCardHand = Instantiate(actionCardHandPrefab, enemyActionCardField.transform);
                actionCardHand.GetOriginalCardInfo(enemyManager.actionCardDeckData[i]);
                actionCardHand.CardBackState(true);
                actionCardHand.ManaState(false);
            }
        }
    }
}
