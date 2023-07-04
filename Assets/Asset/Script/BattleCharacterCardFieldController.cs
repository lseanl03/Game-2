using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCharacterCardFieldController : MonoBehaviour
{
    public bool fullCharacterBattle;
    public int battleCharacterCardMax = 3;
    public GameObject characterCardListBattle;
    public CharacterCardListData characterCardListBattleData;
    public void Start()
    {

    }
    public void Update()
    {
        if(characterCardListBattle.transform.childCount == battleCharacterCardMax)
        {
            fullCharacterBattle = true;

            if (characterCardListBattle.GetComponent<CharacterCardBattleController>())
            {
                characterCardListBattle.GetComponent<CharacterCardBattleController>().enabled = false;
            }
        }
        else if(characterCardListBattle.transform.childCount < battleCharacterCardMax 
            && characterCardListBattle.transform.childCount >= 0)
        {
            fullCharacterBattle = false;

            if (characterCardListBattle.GetComponent<CharacterCardBattleController>().enabled == false)
            {
                characterCardListBattle.GetComponent<CharacterCardBattleController>().enabled = true;
            }
        }

        for (int i = 0; i < characterCardListBattle.transform.childCount; i++)
        {
            if(characterCardListBattle.transform.childCount <= battleCharacterCardMax)
            {
                if (characterCardListBattle.transform.GetChild(i).GetComponent<CharacterCardDisplay>())
                {
                    characterCardListBattleData.characterCardList[i].characterName
                        = characterCardListBattle.transform.GetChild(i).GetComponent<CharacterCardDisplay>().cardName;

                    characterCardListBattleData.characterCardList[i].maxHealth
                        = characterCardListBattle.transform.GetChild(i).GetComponent<CharacterCardDisplay>().maxHealth;

                    characterCardListBattleData.characterCardList[i].skillPointMax
                        = characterCardListBattle.transform.GetChild(i).GetComponent<CharacterCardDisplay>().maxSkillPoint;

                    characterCardListBattleData.characterCardList[i].characterCardSprite
                        = characterCardListBattle.transform.GetChild(i).GetComponent<CharacterCardDisplay>().cardSprite;
                }
            }
        }
    }
}
