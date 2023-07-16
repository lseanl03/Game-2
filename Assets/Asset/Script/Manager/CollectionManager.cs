using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionManager : MonoBehaviour
{
    public int characterCardCount = 0;
    public int actionCardCount = 0;

    public CollectionCanvas collectionCanvas;

    public List<CharacterCardAndQuantity> characterCardListBattle;
    public List<ActionCardAndQuantity> actionCardListBattle;

    public static CollectionManager instance;
    protected UIManager uIManager => UIManager.instance;
    protected GameManager gameManager => GameManager.instance;
    protected DeckManager deckManager => DeckManager.instance;
    protected NotificationManager notificationManager => NotificationManager.instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Update()
    {
        characterCardCount = characterCardListBattle.Count;
        actionCardCount = actionCardListBattle.Count;
        uIManager.selectCardCanvas.QuantityCharacterCard(characterCardCount, deckManager.characterCardMaxSize);
        uIManager.selectCardCanvas.QuantityActionCard(actionCardCount, deckManager.actionCardMaxSize);
    }
    public void CheckListCard()
    {
        int characterCardSize = characterCardCount;
        int actionCardSize = actionCardCount;
        int characterCardMaxSize = deckManager.characterCardMaxSize;
        int actionCardMaxSize = deckManager.actionCardMaxSize;

        if (characterCardSize == characterCardMaxSize && actionCardSize == actionCardMaxSize)
        {
            deckManager.SaveCard();
            notificationManager.SetNewNotification("Deck saved");
        }
        else if (characterCardSize != characterCardMaxSize && actionCardSize != actionCardMaxSize)
        {
            notificationManager.SetNewNotification("Your deck must have 3 character cards \n and 30 action cards");
        }
        else if (characterCardSize < characterCardMaxSize)
        {
            notificationManager.SetNewNotification("Your deck must have 3 character cards");
        }
        else if (actionCardSize < actionCardMaxSize)
        {
            notificationManager.SetNewNotification("Your deck must have 30 action cards");
        }
    }
}
