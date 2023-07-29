using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectInitialActionCardPanel : PanelBase
{
    public float delayTime = 0f;
    public bool recalled = false;
    public TextMeshProUGUI selectToStartText;
    public Button confirmButton;
    public GameObject holderActionCardToHand;
    public ActionCard actionCardInitialPrefab;
    private ActionCard actionCard;
    private void Start()
    {
        StartCoroutine(ShowText("Select cards to start"));
        SpawnInitialActionCard();
    }
    private void Update()
    {
        if (recalled)
        {
            StartCoroutine(HandleEndGetCard());
        }
    }
    public void SpawnInitialActionCard()
    {
        for (int i = 0; i < gamePlayManager.quantityInitialActionCard; i++)
        {
            actionCard = Instantiate(actionCardInitialPrefab, holderActionCardToHand.transform);
            actionCard.GetOriginalCardInfo(gamePlayManager.playerDeckData.actionCardList[0]);
            gamePlayManager.playerDeckData.actionCardList.Remove(actionCard.actionCardData);
            if (actionCard.GetComponent<Draggable>())
            {
                actionCard.GetComponent<Draggable>().enabled = false;
            }
        }
    }
    public void GetCard()
    {
        if (!recalled)
        {
            for(int i = 0; i< holderActionCardToHand.transform.childCount; i++)
            {
                Transform childTransform = holderActionCardToHand.transform.GetChild(i);
                CardInfo cardInfo = childTransform.GetComponent<CardInfo>();
                if (cardInfo.isRecall)
                {
                    cardInfo.ReCallImageState(false);
                    gamePlayManager.playerDeckData.actionCardList.Add(cardInfo.actionCard.actionCardData);
                    cardInfo.actionCard.GetOriginalCardInfo(gamePlayManager.playerDeckData.actionCardList[0]);
                    gamePlayManager.playerDeckData.actionCardList.Remove(cardInfo.actionCard.actionCardData);
                }
            }
            confirmButton.gameObject.SetActive(false);
            StartCoroutine(ShowText("Done select cards"));
            recalled = true;
        }
        AddActionCardToList();
    }
    public IEnumerator HandleEndGetCard()
    {
        yield return new WaitForSeconds(delayTime);
        gamePlayManager.UpdateGameState(GamePlayState.SelectInitialBattleCharacterCard);
        recalled = false;
    }
    IEnumerator ShowText(string text)
    {
        selectToStartText.text = text;
        yield return null;
    }
    public void AddActionCardToList()
    {
        for (int i = 0; i < holderActionCardToHand.transform.childCount; i++)
        {
            ActionCard actionCard = holderActionCardToHand.transform.GetChild(i).GetComponent<ActionCard>();
            gamePlayManager.actionCardInitialDataList.Add(actionCard.actionCardData);
        }
    }
}
