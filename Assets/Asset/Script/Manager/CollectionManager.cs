using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionManager : MonoBehaviour
{
    public int characterCardCount = 0;
    public int actionCardCount = 0;

    public CollectionCanvas collectionCanvas;
    public OptionalToolCanvas optionalToolCanvas;

    public List<CharacterCardData> characterCardDataList;
    public List<ActionCardData> actionCardDataList;

    public static CollectionManager instance;
    protected UIManager uIManager => UIManager.instance;
    protected GameManager gameManager => GameManager.instance;
    protected PlayerDeckManager deckManager => PlayerDeckManager.instance;
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
        characterCardCount = characterCardDataList.Count;
        actionCardCount = actionCardDataList.Count;
        uIManager.selectCardCanvas.QuantityCharacterCard(characterCardCount, deckManager.characterCardMaxSize);
        uIManager.selectCardCanvas.QuantityActionCard(actionCardCount, deckManager.actionCardMaxSize);
    }
}
