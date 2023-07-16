using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCardBase : MonoBehaviour
{
    protected CardListManager cardListManager => CardListManager.instance;
    public CollectionManager collectionManager => CollectionManager.instance;
    protected UIManager uIManager => UIManager.instance;
    protected GameManager gameManager => GameManager.instance;
    protected DeckManager deckManager => DeckManager.instance;

}
