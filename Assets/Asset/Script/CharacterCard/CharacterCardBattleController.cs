using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCardBattleController : MonoBehaviour
{
    public int cardCharacterSlotMax = 3;
    public GameObject characterCardSlotPrefab;
    public CharacterCardListData characterCardListBattleData;

    public List<GameObject> slotList;
    private void Start()
    {
        for(int i = 0; i < cardCharacterSlotMax; i++)
        {
            GameObject slot = Instantiate(characterCardSlotPrefab, transform);
            slotList.Add(slot);
        }
    }
    private void Update()
    {
        AddInfo();
        CheckSlot();
    }
    void CheckSlot()
    {
        for(int i = 0; i < slotList.Count; i++)
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
    void AddInfo()
    {
        for (int i = 0; i < slotList.Count; i++)
        {
            if (slotList[i] != null)
            {
                CharacterCardDisplay characterCardDisplay = slotList[i].GetComponentInChildren<CharacterCardDisplay>();
                if (characterCardDisplay != null)
                {
                    characterCardListBattleData.characterCardList[i].characterName = characterCardDisplay.cardName;
                    characterCardListBattleData.characterCardList[i].maxHealth = characterCardDisplay.maxHealth;
                    characterCardListBattleData.characterCardList[i].skillPointMax = characterCardDisplay.maxSkillPoint;
                    characterCardListBattleData.characterCardList[i].characterCardSprite = characterCardDisplay.cardSprite;
                }
                else
                {
                    characterCardListBattleData.characterCardList[i].characterName = null;
                    characterCardListBattleData.characterCardList[i].maxHealth = 0;
                    characterCardListBattleData.characterCardList[i].skillPointMax = 0;
                    characterCardListBattleData.characterCardList[i].characterCardSprite = null;
                }
            }
        }
    }
}
