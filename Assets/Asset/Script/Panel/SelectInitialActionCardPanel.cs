using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectInitialActionCardPanel : PanelBase
{
    public float delayTime;

    [Header("Prefab")]
    public ActionCard actionCardInitialPrefab;
    private ActionCard actionCard;

    [Header("Component")]
    public TextMeshProUGUI startingHandText;
    public TextMeshProUGUI selectCardToSwitchText;
    public TextMeshProUGUI doneSelectCardText;
    public Button confirmButton;
    public GameObject holderActionCardToHand;
    private void Start()
    {
        SetStartingHandText("Starting Hand");
        SetSelectCardToSwitchText("Select card(s) to switch");
        SpawnChooseInitialActionCard();
    }
    public void SpawnChooseInitialActionCard()
    {
        for (int i = 0; i < gamePlayManager.quantityInitialActionCard; i++)
        {
            actionCard = Instantiate(actionCardInitialPrefab, holderActionCardToHand.transform);
            playerManager.actionCardTakenDataList.Add(playerManager.actionCardDeckData[0]);
            playerManager.actionCardDeckData.RemoveAt(0);
            actionCard.GetCardData(playerManager.actionCardTakenDataList[i]);
            if (actionCard.GetComponent<Draggable>())
            {
                actionCard.GetComponent<Draggable>().enabled = false;
            }
        }
    }
    public void GetActionCard()
    {
        for (int i = 0; i < holderActionCardToHand.transform.childCount; i++)
        {
            Transform childTransform = holderActionCardToHand.transform.GetChild(i);
            CardInfo cardInfo = childTransform.GetComponent<CardInfo>();
            if (cardInfo.isRecall)
            {
                cardInfo.ReCallImageState(false);
                playerManager.actionCardDeckData.Add(playerManager.actionCardTakenDataList[i]);
                playerManager.actionCardTakenDataList.RemoveAt(i);
                playerManager.actionCardTakenDataList.Insert(i, playerManager.actionCardDeckData[0]);
                playerManager.actionCardDeckData.RemoveAt(0);
                cardInfo.actionCard.GetCardData(playerManager.actionCardTakenDataList[i]);
            }
        }
        StartCoroutine(HandleAfterGetActionCard());
    }
    IEnumerator HandleAfterGetActionCard()
    {
        ConfirmButtonState(false);
        SetDoneSelectCardText("Done Select Cards");
        gamePlayManager.playerSelectedActionCardInitial = true;
        yield return new WaitForSeconds(delayTime);
        gamePlayManager.UpdateGameState(GamePlayState.SelectBattleCharacter);
    }
    void SetStartingHandText(string text)
    {
        startingHandText.text = text;
    }
    void SetSelectCardToSwitchText(string text)
    {
        selectCardToSwitchText.text = text;
    }
    void SetDoneSelectCardText(string text)
    {
        doneSelectCardText.text = text;
    }
    void ConfirmButtonState(bool state)
    {
        confirmButton.gameObject.SetActive(state);
    }
}
