using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCardTooltip : MonoBehaviour
{

    [Header("SkillScript")]
    public SkillTooltip normalAtk;
    public SkillTooltip elementalSkill;
    public SkillTooltip elementalBurst;

    [Header("Character Card")]
    public Image characterCardImage;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI cardNameText;
    public Image combatTypeImage;

    [Header("Weakness")]
    public Image weaknessImage1;
    public Image weaknessImage2;
    public Image weaknessImage3;

    [Header("Normal Attack")]
    public Image normalAttackImage;
    public TextMeshProUGUI normalAttackCostText;
    public TextMeshProUGUI normalAttackNameText;
    public Button normalAttackDesButton;
    public TextMeshProUGUI normalAttackDesText;

    [Header("Elemental Skill")]
    public Image elementalSkillImage;
    public TextMeshProUGUI elementalSkillCostText;
    public TextMeshProUGUI elementalSkillNameText;
    public Button elementalSkillDesButton;
    public TextMeshProUGUI elementalSkillDesText;

    [Header("Elemental Burst")]
    public Image elementalBurstImage;
    public TextMeshProUGUI elementalBurstCostText;
    public TextMeshProUGUI elementalBurstNameText;
    public Button elementalBurstDesButton;
    public TextMeshProUGUI elementalBurstDesText;

    public GameObject statusDescriptionObj;

    [Header("Status")]
    public List<StatusTooltip> statusTooltipList;
    public List<StatusDesTooltip> statusDesTooltipList;

    [Header("Breaking Status")]
    public List <BreakingTooltip> breakingTooltipList;
    public List <BreakingDesTooltip> breakingDesTooltipList;

    [Header("Data")]
    public CharacterCard currentCharacterCard;

    public void Start()
    {
        gameObject.SetActive(false);
        StatusDescriptionObjState(false);
    }
    public void StatusDescriptionObjState(bool state)
    {
        statusDescriptionObj.SetActive(state);
    }
    public void HideSkillDes()
    {
        if (normalAtk.isShowing)
        {
            normalAtk.DescriptionState();
        }
        if (elementalSkill.isShowing)
        {
            elementalSkill.DescriptionState();
        }
        if (elementalBurst.isShowing)
        {
            elementalBurst.DescriptionState();
        }
    }
    public void HideStatusDes()
    {
        foreach(StatusTooltip statusTooltip in statusTooltipList)
        {
            if (statusTooltip.isShowing)
            {
                statusTooltip.ChangeDescriptionState();
            }
        }
    }
    public void GetStatusInfo(List<Status> statusList)
    {
        for(int i = 0; i < statusList.Count; i++)
        {
            statusTooltipList[i].gameObject.SetActive(true);
            statusTooltipList[i].statusImage.sprite = statusList[i].statusSprite;
            statusTooltipList[i].statusNameText.text = statusList[i].statusName;

            statusDesTooltipList[i].statusDesText.text = statusList[i].statusDescription;
        }
        for (int i = 0; i < statusTooltipList.Count; i++)
        {
            if (i >= statusList.Count)
            {
                statusTooltipList[i].gameObject.SetActive(false);
            }
        }
        for(int i=0; i<statusDesTooltipList.Count; i++)
        {
            if (i >= statusList.Count)
            {
                statusDesTooltipList[i].statusDesText.text = string.Empty;
            }
        }
    }
    public void GetBreakingStatusInfo(List<WeaknessBreaking> breakingList)
    {
        for (int i = 0; i < breakingList.Count; i++)
        {
            breakingTooltipList[i].gameObject.SetActive(true);
            breakingTooltipList[i].statusImage.sprite = breakingList[i].weaknessBreakingSprite;
            breakingTooltipList[i].statusNameText.text = breakingList[i].weaknessBreakingName;

            breakingDesTooltipList[i].statusDesText.text = breakingList[i].weaknessBreakingDescription;
        }
        for (int i = 0; i < breakingTooltipList.Count; i++)
        {
            if (i >= breakingList.Count)
            {
                breakingTooltipList[i].gameObject.SetActive(false);
            }
        }
        for (int i = 0; i < breakingDesTooltipList.Count; i++)
        {
            if (i >= breakingList.Count)
            {
                breakingDesTooltipList[i].statusDesText.text = string.Empty;
            }
        }
    }
    public void GetCharacterCardInfo(CharacterCard characterCard)
    {
        currentCharacterCard = characterCard;

        characterCardImage.sprite = currentCharacterCard.characterCardData.cardSprite;
        cardNameText.text = currentCharacterCard.characterCardData.characterName;
        combatTypeImage.sprite = currentCharacterCard.characterCardData.characterCard.combat.combatTypeSprite;
        healthText.text = currentCharacterCard.currentHealth.ToString();

        //weakness
        weaknessImage1.gameObject.SetActive(false);
        weaknessImage2.gameObject.SetActive(false);
        weaknessImage3.gameObject.SetActive(false);

        for(int i = 0; i < currentCharacterCard.characterStats.weaknessList.Count; i++)
        {
            if (i == 0)
            {
                weaknessImage1.gameObject.SetActive(true);
                weaknessImage1.sprite = currentCharacterCard.characterStats.weaknessList[i].combatTypeSprite;
            }
            if(i == 1)
            {
                weaknessImage2.gameObject.SetActive(true);
                weaknessImage2.sprite = currentCharacterCard.characterStats.weaknessList[i].combatTypeSprite;
            }
            if(i == 2)
            {
                weaknessImage3.gameObject.SetActive(true);
                weaknessImage3.sprite = currentCharacterCard.characterStats.weaknessList[i].combatTypeSprite;
            }
        }

        //skill
        for (int i = 0; i < currentCharacterCard.characterCardData.characterCard.characterSkillList.Count; i++)
        {
            CharacterSkill characterSkill = currentCharacterCard.characterCardData.characterCard.characterSkillList[i];
            CharacterCardSkillType skillType = characterSkill.characterCardSkillType;

            //normal atk
            if (skillType == CharacterCardSkillType.NormalAttack)
            {
                normalAttackImage.sprite = characterSkill.skillSprite;

                normalAttackNameText.text = "Normal Attack";
                normalAttackCostText.text = characterSkill.actionPointCost.ToString();
                normalAttackDesText.text = characterSkill.descriptionSkill;
            }

            //elemental skill
            else if(skillType == CharacterCardSkillType.ElementalSkill)
            {
                elementalSkillImage.sprite = characterSkill.skillSprite;

                elementalSkillNameText.text = "Elemental Skill";
                elementalSkillCostText.text = characterSkill.actionPointCost.ToString();
                elementalSkillDesText.text = characterSkill.descriptionSkill;
            }

            //elemental burst
            else if(skillType == CharacterCardSkillType.ElementalBurst)
            {
                elementalBurstImage.sprite = characterSkill.skillSprite;

                elementalBurstNameText.text = "Element Burst";
                elementalBurstCostText.text = characterSkill.actionPointCost.ToString();
                elementalBurstDesText.text = characterSkill.descriptionSkill;
            }
        }

    }
}
