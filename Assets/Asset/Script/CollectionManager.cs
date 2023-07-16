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
}
