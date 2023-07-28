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
    public int characterCardDeckSize;
    public int actionCardListSize;

    public CardListData cardListData;

    public static CardListManager instance;
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
        characterCardDeckSize = cardListData.characterCardList.Count;
        actionCardListSize = cardListData.actionCardList.Count;
    }
}
