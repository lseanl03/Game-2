using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCharacterCardField : MonoBehaviour
{
   // public CharacterCardListData playerCharacterCardListData;
    private void Start()
    {
        for(int i = 0; i<transform.childCount; i++)
        {

            //if(transform.GetChild(i).GetComponent<CharacterCardDisplay>())
            //{
            //    string characterName = playerCharacterCardListData.characterCardList[i].characterName;
            //    string description = playerCharacterCardListData.characterCardList[i].description;
            //    int maxHealth = playerCharacterCardListData.characterCardList[i].maxHealth;
            //    int skillPoint = playerCharacterCardListData.characterCardList[i].skillPointMax;
            //    Sprite cardSprite = playerCharacterCardListData.characterCardList[i].characterCardSprite;

            //    CharacterCard card = new CharacterCard(characterName, description, maxHealth, skillPoint);

            //}
        }
    }
}
