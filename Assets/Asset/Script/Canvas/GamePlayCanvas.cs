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
        if(playerDeckManager.characterCardDeckData.Count > 0)
        {
            for (int i = 0; i < playerDeckManager.characterCardDeckData.Count; i++)
            {
                characterCardHand = Instantiate(characterCardHandPrefab, playerCharacterCardField.transform);
                characterCardHand.GetOriginalCardInfo(playerDeckManager.characterCardDeckData[i]);
            }
        }
    }
    public void SpawnInitialActionCard()
    {
        if (gamePlayManager.actionCardInitialDataList.Count > 0)
        {
            for (int i = 0; i < gamePlayManager.actionCardInitialDataList.Count; i++)
            {
                actionCardHand = Instantiate(actionCardHandPrefab, playerActionCardField.transform);
                actionCardHand.GetOriginalCardInfo(gamePlayManager.actionCardInitialDataList[i]);
            }
        }
    }
}
