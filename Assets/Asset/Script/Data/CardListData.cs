using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "NewCardList", menuName = "TCG/CardList")]
public class CardListData : ScriptableObject
{
    public List<CharacterCardData> characterCardList;
    public List<ActionCardData> actionCardList;
}