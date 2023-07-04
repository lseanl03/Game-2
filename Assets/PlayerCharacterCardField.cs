using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterCardField : MonoBehaviour
{
    public CharacterCardListData playerCharacterCardListData;
    private void Start()
    {
        for(int i = 0; i<transform.childCount; i++)
        {

            if(transform.GetChild(i).GetComponent<CharacterCardDisplay>())
            {
                string characterName = playerCharacterCardListData.characterCardList[i].characterName;
                int maxHealth = playerCharacterCardListData.characterCardList[i].maxHealth;
                int skillPoint = playerCharacterCardListData.characterCardList[i].skillPointMax;
                Sprite cardSprite = playerCharacterCardListData.characterCardList[i].characterCardSprite;

                transform.GetChild(i).GetComponent<CharacterCardDisplay>().GetName(characterName);
                transform.GetChild(i).GetComponent<CharacterCardDisplay>().GetHealth(maxHealth);
                transform.GetChild(i).GetComponent<CharacterCardDisplay>().GetSkillPoint(skillPoint);
                transform.GetChild(i).GetComponent<CharacterCardDisplay>().GetImage(cardSprite);
            }
        }
    }
}
