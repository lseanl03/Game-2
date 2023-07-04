using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class PlayerDeck : MonoBehaviour
{
    public int deckSize = 30;

    public DeckData deckData;

    public List<CardData> cardDataList = new List<CardData>();
    private void Start()
    {
        AddCardToDeck();
    }
    private void Update()
    {
        deckSize = cardDataList.Count;
    }
    public void Shuffle()
    {
        //for (int i = 0; i < cardDataList.Count; i++)
        //{
        //    int random = Random.Range(i, cardDataList.Count);
        //    CardData temp = cardDataList[i];
        //    cardDataList[i] = cardDataList[random];
        //    cardDataList[random] = temp;
        //}
    }
    public void  AddCardToDeck()
    {
        for (int i = 0; i < deckSize; i++)
        {
            int random = Random.Range(0, deckData.cardList.Count);
            cardDataList.Add(deckData.cardList[random]);
        }
    }
}
