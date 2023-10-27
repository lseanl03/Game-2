using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum CardSelectType
{
    None,
    CharacterCard,
    ActionCard
}
public class GameManager : MonoBehaviour
{
    public int characterCardDeckSize;
    public int actionCardListSize;

    public CardListData cardListData;

    public static GameManager instance;
    protected CollectionManager collectionManager => CollectionManager.instance;
    protected UIManager uIManager => UIManager.instance;

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
        }
        characterCardDeckSize = cardListData.characterCardList.Count;
        actionCardListSize = cardListData.actionCardList.Count;
    }
}
