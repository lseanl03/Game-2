using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;


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
            for (int i = 0; i < cardListManager.cardList.characterCardDeckSize; i++)
            {
                CharacterCard characterCard = Instantiate(characterCardPrefab, transform);
                CharacterCardAndQuantity characterCardAndQuantity = cardListManager.cardList.characterCardList[i];
                characterCard.GetOriginalCardInfo(characterCardAndQuantity);
            }
        }
        if (cardSelectType == CardSelectType.ActionCard)
        {
            for (int i = 0; i < cardListManager.cardList.actionCardDeckSize; i++)
            {
                ActionCard actionCard = Instantiate(actionCardPrefab, transform);
                ActionCardAndQuantity actionCardAndQuantity = cardListManager.cardList.actionCardList[i];
                actionCard.GetOriginalCardInfo(actionCardAndQuantity);
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
