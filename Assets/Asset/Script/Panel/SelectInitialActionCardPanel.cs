using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectInitialActionCardPanel : PanelBase
{
    public float delayTime = 0f;
    public bool recalled = false;

    [Header("Prefab")]
    public ActionCard actionCardInitialPrefab;
    private ActionCard actionCard;

    public TextMeshProUGUI selectToStartText;
    public Button confirmButton;
    public GameObject holderActionCardToHand;
    private void Start()
    {
        StartCoroutine(ShowText("Select cards to start"));
        SpawnChooseInitialActionCard();
    }
    private void Update()
    {
        if (recalled)
        {
            StartCoroutine(HandleEndGetCard());
        }
    }
    public void OnEnable()
    {
        ConfirmButtonState(true);
    }
    public void SpawnChooseInitialActionCard()
    {
        for (int i = 0; i < gamePlayManager.quantityInitialActionCard; i++)
        {
            actionCard = Instantiate(actionCardInitialPrefab, holderActionCardToHand.transform);
            playerManager.actionCardTakenList.Add(playerManager.actionCardDeckData[0]);
            playerManager.actionCardDeckData.RemoveAt(0);
            actionCard.GetOriginalCardInfo(playerManager.actionCardTakenList[i]);
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
                    playerManager.actionCardDeckData.Add(playerManager.actionCardTakenList[i]);
                    playerManager.actionCardTakenList.RemoveAt(i);
                    playerManager.actionCardTakenList.Insert(i, playerManager.actionCardDeckData[0]);
                    playerManager.actionCardDeckData.RemoveAt(0);
                    cardInfo.actionCard.GetOriginalCardInfo(playerManager.actionCardTakenList[i]);
                }
            }
            ConfirmButtonState(false);
            StartCoroutine(ShowText("Done select cards"));
            recalled = true;
        }
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
    public void ConfirmButtonState(bool state)
    {
        confirmButton.gameObject.SetActive(state);
    }
}
