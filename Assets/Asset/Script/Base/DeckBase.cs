using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckBase : MonoBehaviour
{
    public int quantityRemainingCharacterCard;
    public int quantityRemainingActionCard;

    public List<CharacterCardData> characterCardList;
    public List<ActionCardData> actionCardList;

    public PlayerManager playerDeckManager => PlayerManager.instance;
    public EnemyManager enemyDeckManager => EnemyManager.instance;
    public CardListManager cardListManager => CardListManager.instance;
    public virtual void Start()
    {
        if(playerDeckManager != null)
        {
            quantityRemainingCharacterCard = characterCardList.Count;
            quantityRemainingActionCard = actionCardList.Count;
        }
    }
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
}
