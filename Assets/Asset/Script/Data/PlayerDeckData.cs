using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeckData : DeckBase
{
    public override void Start()
    {
        GetCharacterCardData();
        GetActionCardData();
    }
    public void GetCharacterCardData()
    {
        if(playerDeckManager != null)
        {
            for (int i = 0; i < playerDeckManager.characterCardMaxSize; i++)
            {
                characterCardList.Add(playerDeckManager.characterCardDeckData[i]);
            }
        }
    }
    public void GetActionCardData()
    {
        if (playerDeckManager != null)
        {
            for (int i = 0; i < playerDeckManager.actionCardMaxSize; i++)
            {
                actionCardList.Add(playerDeckManager.actionCardDeckData[i]);
            }
            ShuffleList(actionCardList);
        }
    }
}
