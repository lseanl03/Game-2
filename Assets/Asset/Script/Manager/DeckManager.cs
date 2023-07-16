using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class DeckManager : MonoBehaviour
{

    public int characterCardMaxSize = 3;
    public int actionCardMaxSize = 30;
    public List<CharacterCardAndQuantity> characterCardDeck;
    public List<ActionCardAndQuantity> actionCardDeck;

    public static DeckManager instance;
    public CollectionManager collectionManager => CollectionManager.instance;
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
        if (characterCardDeck.Count == 0) AddCharacterCard();
        else
        {
            for (int i = 0; i < characterCardDeck.Count; i++)
            {
                if (characterCardDeck[i].characterCard != collectionManager.characterCardListBattle[i].characterCard)
                {
                    RemoveCharacterCard();
                    AddCharacterCard();
                    break;
                }
            }
        }

        if (actionCardDeck.Count == 0) AddActionCard();
        else
        {
            for (int i = 0; i < actionCardDeck.Count; i++)
            {
                if (actionCardDeck[i].actionCard != collectionManager.actionCardListBattle[i].actionCard)
                {
                    RemoveActionCard();
                    AddActionCard();
                    break;
                }
            }
        }
    }
    public void AddCharacterCard()
    {
        for (int i = 0; i < collectionManager.characterCardCount; i++)
        {
            characterCardDeck.Add(collectionManager.characterCardListBattle[i]);
        }
    }
    public void AddActionCard()
    {
        for (int i = 0; i < collectionManager.actionCardCount; i++)
        {
            actionCardDeck.Add(collectionManager.actionCardListBattle[i]);
        }
    }

    public void RemoveCharacterCard()
    {
        characterCardDeck.Clear();
    }
    public void RemoveActionCard()
    {
        actionCardDeck.Clear();
    }
}
