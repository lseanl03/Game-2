using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickCharacterCardFieldController : MonoBehaviour
{
    public Transform characterCardListPick;
    public GameObject characterCardPrefab;
    public CharacterCardListData characterCardListData;
    private void Start()
    {
        for(int i = 0; i < characterCardListData.characterCardList.Count; i++)
        {
            string characterName = characterCardListData.characterCardList[i].characterName;
            int maxHealth = characterCardListData.characterCardList[i].maxHealth;
            int skillPoint = characterCardListData.characterCardList[i].skillPointMax;
            Sprite cardSprite = characterCardListData.characterCardList[i].characterCardSprite;
            GameObject characterCard = Instantiate(characterCardPrefab, characterCardListPick);
            if (characterCard.GetComponent<CharacterCardDisplay>())
            {
                characterCard.GetComponent<CharacterCardDisplay>().GetImage(cardSprite);
                characterCard.GetComponent<CharacterCardDisplay>().GetName(characterName);
                characterCard.GetComponent<CharacterCardDisplay>().GetHealth(maxHealth);
                characterCard.GetComponent<CharacterCardDisplay>().GetSkillPoint(skillPoint);
            }
        }
    }
    
}
