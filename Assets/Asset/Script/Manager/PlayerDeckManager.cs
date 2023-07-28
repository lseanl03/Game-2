using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class PlayerDeckManager : DeckManagerBase
{
    public bool deckSaved = false;

    public static PlayerDeckManager instance;
    public void Awake()
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
    }
    public void SaveCard()
    {
        if (characterCardDeckData.Count == 0) AddCharacterCard();
        else
        {
            for (int i = 0; i < characterCardDeckData.Count; i++)
            {
                if (characterCardDeckData[i] != collectionManager.characterCardDataList[i])
                {
                    RemoveCharacterCard();
                    AddCharacterCard();
                    break;
                }
            }
        }

        if (actionCardDeckData.Count == 0) AddActionCard();
        else
        {
            for (int i = 0; i < actionCardDeckData.Count; i++)
            {
                if (actionCardDeckData[i] != collectionManager.actionCardDataList[i])
                {
                    RemoveActionCard();
                    AddActionCard();
                    break;
                }
            }
        }
        deckSaved = true;
    }
    public void AddCharacterCard()
    {
        for (int i = 0; i < collectionManager.characterCardCount; i++)
        {
            characterCardDeckData.Add(collectionManager.characterCardDataList[i]);
        }
    }
    public void AddActionCard()
    {
        for (int i = 0; i < collectionManager.actionCardCount; i++)
        {
            actionCardDeckData.Add(collectionManager.actionCardDataList[i]);
        }
    }

    public void RemoveCharacterCard()
    {
        characterCardDeckData.Clear();
    }
    public void RemoveActionCard()
    {
        actionCardDeckData.Clear();
    }
}
