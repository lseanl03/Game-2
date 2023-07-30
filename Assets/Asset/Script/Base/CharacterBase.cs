using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    [Header("Action Point")]
    public int initialActionPoint = 100;
    public int currentActionPoint;

    [Header("Deck")]
    public int characterCardMaxSize = 3;
    public int actionCardMaxSize = 30;
    public List<CharacterCardData> characterCardDeckData;
    public List<ActionCardData> actionCardDeckData;
    public CollectionManager collectionManager => CollectionManager.instance;
    protected CardListManager cardListManager => CardListManager.instance;
    public void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
    public virtual void Start()
    {
        currentActionPoint = initialActionPoint;
    }
}
