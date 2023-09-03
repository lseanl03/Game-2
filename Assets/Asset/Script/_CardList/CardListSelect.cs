using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CardListSelect : SelectCardBase
{
    public ContentSizeFitter contentSizeFitter;

    public CharacterCard characterCardPrefab;
    public ActionCard actionCardPrefab;
    public CardSelectType thisCardType;
    public void Start()
    {
        contentSizeFitter = GetComponent<ContentSizeFitter>();
        SpawnCard(thisCardType);
    }
    public void Update()
    {
        CheckCard();
    }

    public void SpawnCard(CardSelectType cardSelectType)
    {
        if (cardSelectType == CardSelectType.CharacterCard)
        {
            for (int i = 0; i < gameManager.characterCardDeckSize; i++)
            {
                CharacterCard characterCard = Instantiate(characterCardPrefab, transform);
                CharacterCardData cardData = gameManager.cardListData.characterCardList[i];
                characterCard.GetOriginalCardInfo(cardData);
            }
        }
        if (cardSelectType == CardSelectType.ActionCard)
        {
            for (int i = 0; i < gameManager.actionCardListSize; i++)
            {
                ActionCard actionCard = Instantiate(actionCardPrefab, transform);
                ActionCardData cardData = gameManager.cardListData.actionCardList[i];
                actionCard.GetCardData(cardData);
            }
        }
    }

    public void CheckCard()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.childCount >= 7)
            {
                contentSizeFitter.enabled = true;
            }
            else
            {
                contentSizeFitter.enabled = false;
            }
        }
    }
}
