using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Test2 : SelectCardBase
{
    public int numberCharacterCard;
    public int numberActionCard;
    public GameObject slotPrefab;
    public GameObject characterCardPrefab;
    public GameObject actionCardPrefab;
    public CardSelectType thisCardType;
    public List<GameObject> slotList;
    public void Start()
    {
        //numberCharacterCard = cardListData.characterCardList.Count;
        //numberActionCard = cardListData.actionCardList.Count;
        SpawnCard(CardSelectType.ActionCard);
    }
    public void Update()
    {
        CheckSlot();
    }
    void SpawnCard(CardSelectType cardSelectType)
    {
        if (cardSelectType == CardSelectType.ActionCard)
        {
            for (int i = 0; i < numberActionCard; i++)
            {
                GameObject slot = Instantiate(slotPrefab, transform);
                GameObject actionCard = Instantiate(actionCardPrefab, slot.transform);
                //GameObject actionCard2 = Instantiate(actionCardPrefab, slot.transform);
                //actionCard.GetComponent<ActionCardDisplay>().GetOriginalCardInfo(cardListData.actionCardList[i]);
                //actionCard2.GetComponent<ActionCardDisplay>().GetCardInfo(cardListData.actionCardList[i]);
                slotList.Add(slot);
            }
        }
    }
    void CheckSlot()
    {
        for (int i = 0; i < slotList.Count; i++)
        {
            if (slotList[i].GetComponent<DropZone>())
            {
                if (slotList[i].GetComponentInChildren<Draggable>())
                {
                    slotList[i].GetComponent<DropZone>().enabled = false;
                }
                else
                {
                    slotList[i].GetComponent<DropZone>().enabled = true;
                }
            }
        }
    }
}
