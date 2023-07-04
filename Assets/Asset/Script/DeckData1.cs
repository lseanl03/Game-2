using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Deck", menuName = "TCG/Deck")]
public class DeckData : ScriptableObject
{
    public List<CardData> cardList;

}
