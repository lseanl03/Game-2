using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeckData : DeckBase
{
    public override void Start()
    {
        GetCharacterCard();
        GetActionCard();
    }
    public void GetCharacterCard()
    {
        if(cardListManager != null)
        {
            for(int i = 0; i < enemyDeckManager.characterCardMaxSize; i++)
            {
                characterCardList.Add(enemyDeckManager.characterCardDeckData[i]);
            }

        }
    }
    public void GetActionCard()
    {
        if (cardListManager != null)
        {
            for (int i = 0; i < enemyDeckManager.actionCardMaxSize; i++)
            {
                actionCardList.Add(enemyDeckManager.actionCardDeckData[i]);
            }
            ShuffleList(actionCardList);
        }
    }
}
