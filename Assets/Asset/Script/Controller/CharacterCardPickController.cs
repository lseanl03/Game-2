using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterCardPickController : MonoBehaviour
{
    public int numberOfCard;
    public GameObject slotPrefab;
    public GameObject characterCardPrefab;
    public CardListData selectCardData;

    public List<GameObject> slotList;
    private void Start()
    {
    }
    private void Update()
    {
        CheckSlot();    
    }
    void CharacterCardSpawn()
    {
        for (int i = 0; i < numberOfCard; i++)
        {
            GameObject slot = Instantiate(slotPrefab, transform);

            GameObject characterCard = Instantiate(characterCardPrefab, slot.transform);

            //string characterName = characterCardListData.characterCardList[i].characterName;
            //string description = characterCardListData.characterCardList[i].description;
            //int maxHealth = characterCardListData.characterCardList[i].maxHealth;
            //int skillPoint = characterCardListData.characterCardList[i].skillPointMax;
            //Sprite cardSprite = characterCardListData.characterCardList[i].characterCardSprite;

            //if (characterCard.GetComponent<CharacterCardDisplay>())
            //{
            //    CharacterCard card = new CharacterCard(characterName, description, maxHealth, skillPoint);
            //    //characterCard.GetComponent<CharacterCardDisplay>().GetImage(cardSprite);
            //    //characterCard.GetComponent<CharacterCardDisplay>().GetName(characterName);
            //    //characterCard.GetComponent<CharacterCardDisplay>().GetHealth(maxHealth);
            //    //characterCard.GetComponent<CharacterCardDisplay>().GetSkillPoint(skillPoint);
            //}

            slotList.Add(slot);
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
