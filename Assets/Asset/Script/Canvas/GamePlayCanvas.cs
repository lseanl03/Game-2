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

    [Header("Support Card")]
    public SupportCard supportCardPrefab;
    private SupportCard supportCard;

    [Header("Character Field")]
    public PlayerCharacterCardField playerCharacterCardField;
    public EnemyCharacterCardField enemyCharacterCardField;

    [Header("Action Card Field")]
    public PlayerActionCardField playerActionCardField;
    public EnemyActionCardField enemyActionCardField;

    [Header("Support Card Field")]
    public PlayerSupportCardField playerSupportCardField;
    public EnemySupportCardField enemySupportCardField;

    public void Start()
    {
        SpawnInitialCharacterCard();
        SpawnInitialActionCard();
    }
    public void SpawnSupportCard(ActionCard actionCard, bool isPlayer)
    {
        if(isPlayer)
        {
            supportCard = Instantiate(supportCardPrefab, playerSupportCardField.transform);
            supportCard.GetCardData(actionCard.actionCardData);

            gamePlayManager.playerSupportCardList.Add(supportCard);
        }
        else
        {
            supportCard = Instantiate(supportCardPrefab, enemySupportCardField.transform);
            supportCard.GetCardData(actionCard.actionCardData);

            gamePlayManager.enemySupportCardList.Add(supportCard);
        }
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
            actionCardHand.CardBackState(true);
            actionCardHand.ManaState(false);
            enemyManager.actionCardTakenDataList.Add(enemyManager.actionCardDeckData[0]);
            actionCardHand.GetCardData(enemyManager.actionCardTakenDataList[i]);

            gamePlayManager.enemyActionCardList.Add(actionCardHand);
            enemyManager.actionCardDeckData.RemoveAt(0);
        }
    }
    public void DrawCardsUsingSupportCards(int value, bool isPlayer)
    {
        if (isPlayer) StartCoroutine(PlayerDrawCard(value));
        else StartCoroutine(EnemyDrawCard(value));
    }
    public IEnumerator DrawCardEndPhase(int value)
    {
        yield return StartCoroutine(PlayerDrawCard(value));
        yield return StartCoroutine(EnemyDrawCard(value));
    }
    public IEnumerator PlayerDrawCard(int value)
    {

        for (int i = 0; i < value; i++)
        {
            if (playerManager.actionCardDeckData.Count > 0)
            {
                if (playerActionCardField.transform.childCount < 10)
                {
                    AudioManager.instance.PlayDrawCard();
                    gamePlayManager.playerDrawingActionCard = true;

                    actionCardHand = Instantiate(actionCardHandPrefab, playerActionCardField.transform);
                    actionCardHand.GetCardData(playerManager.actionCardDeckData[0]);
                    playerManager.actionCardTakenDataList.Add(playerManager.actionCardDeckData[0]);
                    playerManager.actionCardDeckData.RemoveAt(0);

                    gamePlayManager.playerActionCardList.Add(actionCardHand);
                    yield return new WaitForSeconds(0.25f);
                }
            }
        }
        if (gamePlayManager.playerDrawingActionCard) gamePlayManager.playerDrawingActionCard = false;
    }
    public IEnumerator EnemyDrawCard(int value)
    {

        for (int i = 0; i < value; i++)
        {
            if (enemyManager.actionCardDeckData.Count > 0)
            {
                if (enemyActionCardField.transform.childCount < 10)
                {
                    AudioManager.instance.PlayDrawCard();
                    gamePlayManager.enemyDrawingActionCard = true;

                    actionCardHand = Instantiate(actionCardHandPrefab, enemyActionCardField.transform);
                    actionCardHand.CardBackState(true);
                    actionCardHand.ManaState(false);
                    actionCardHand.GetCardData(enemyManager.actionCardDeckData[0]);
                    enemyManager.actionCardTakenDataList.Add(enemyManager.actionCardDeckData[0]);
                    enemyManager.actionCardDeckData.RemoveAt(0);

                    gamePlayManager.enemyActionCardList.Add(actionCardHand);
                    yield return new WaitForSeconds(0.25f);
                }
            }
        }
        if (gamePlayManager.enemyDrawingActionCard) gamePlayManager.enemyDrawingActionCard = false;
    }
    public void ResetActionCard()
    {
        for (int i = playerManager.actionCardTakenDataList.Count - 1; i >= 0; i--)
        {
            playerManager.actionCardDeckData.Add(playerManager.actionCardTakenDataList[i]);
            playerManager.actionCardTakenDataList.RemoveAt(i);
        }

        for (int i = enemyManager.actionCardTakenDataList.Count - 1; i >= 0; i--)
        {
            enemyManager.actionCardDeckData.Add(enemyManager.actionCardTakenDataList[i]);
            enemyManager.actionCardTakenDataList.RemoveAt(i);
        }
    }
}
