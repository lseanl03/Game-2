using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacterCardField : MonoBehaviour
{
    //public CharacterCardListData enemyCharacterCardListData;
   // public CharacterCardListData characterCardListData;
    private void Start()
    {
        List<int> randomCheck = new List<int>();   

        for(int i = 0;i<transform.childCount;i++)
        {
            //int randomIndex;
            do
            {
                //randomIndex = Random.Range(0, characterCardListData.characterCardList.Count);

            } while (randomCheck.Contains(1));
            randomCheck.Add(1);

            //string characterName = enemyCharacterCardListData.characterCardList[i].characterName
            //    = characterCardListData.characterCardList[randomIndex].characterName;

            //string description = enemyCharacterCardListData.characterCardList[i].description
            //    = characterCardListData.characterCardList[randomIndex].description;

            //int maxHealth = enemyCharacterCardListData.characterCardList[i].maxHealth
            //    = characterCardListData.characterCardList[randomIndex].maxHealth;

            //int skillPoint = enemyCharacterCardListData.characterCardList[i].skillPointMax
            //    = characterCardListData.characterCardList[randomIndex].skillPointMax;

            //Sprite cardSprite = enemyCharacterCardListData.characterCardList[i].characterCardSprite
            //    = characterCardListData.characterCardList[randomIndex].characterCardSprite;

            //if (transform.GetChild(i).GetComponent<CharacterCardDisplay>())
            //{
            //    CharacterCard card = new CharacterCard(characterName, description, maxHealth, skillPoint);
            //    //transform.GetChild(i).GetComponent<CharacterCardDisplay>().GetImage(cardSprite);
            //    //transform.GetChild(i).GetComponent<CharacterCardDisplay>().GetName(characterName);
            //    //transform.GetChild(i).GetComponent<CharacterCardDisplay>().GetHealth(maxHealth);
            //    //transform.GetChild(i).GetComponent<CharacterCardDisplay>().GetSkillPoint(skillPoint);
            //}
        }
    }
}
