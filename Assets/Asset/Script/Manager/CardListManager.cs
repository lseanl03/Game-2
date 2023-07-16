using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardSelectType
{
    None,
    CharacterCard,
    ActionCard
}
public class CardListManager : MonoBehaviour
{
    public static CardListManager instance;

    public CardList cardList;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        cardList.characterCardDeckSize = cardList.characterCardList.Count;
        cardList.actionCardDeckSize = cardList.actionCardList.Count;
    }
}
