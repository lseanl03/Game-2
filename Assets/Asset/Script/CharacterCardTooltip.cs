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

    [Header("NormalAttack")]
    public Image normalAttackImage;
    public TextMeshProUGUI normalAttackCostText;
    public TextMeshProUGUI normalAttackNameText;
    public Button normalAttackDesButton;
    public TextMeshProUGUI normalAttackDesText;

    [Header("ElementalSkill")]
    public Image elementalSkillImage;
    public TextMeshProUGUI elementalSkillCostText;
    public TextMeshProUGUI elementalSkillNameText;
    public Button elementalSkillDesButton;
    public TextMeshProUGUI elementalSkillDesText;

    [Header("ElementalBurst")]
    public Image elementalBurstImage;
    public TextMeshProUGUI elementalBurstCostText;
    public TextMeshProUGUI elementalBurstNameText;
    public Button elementalBurstDesButton;
    public TextMeshProUGUI elementalBurstDesText;

    [Header("CharacterStatus")]
    public GameObject statusDescriptionObj;
    public List<StatusTooltip> statusTooltipList;
    public List<StatusDesTooltip> statusDesTooltipList;

    [Header("Data")]
    public CharacterCardData currentCharacterCardData;

    public void Start()
    {
        gameObject.SetActive(false);
        StatusDescriptionObjState(false);
    }
    public void StatusDescriptionObjState(bool state)
    {
        statusDescriptionObj.SetActive(state);
    }
    public void HideStatusDes()
    {
    }
    public void GetStatusInfo(Sprite sprite, string name, string description, int count)
    {
        for(int i=0;i<statusTooltipList.Count;i++)
        {
            if (!statusTooltipList[i].gameObject.activeSelf && i < count)
            {
                statusTooltipList[i].gameObject.SetActive(true);
                statusTooltipList[i].statusImage.sprite = sprite;
                statusTooltipList[i].statusNameText.text = name;
                break;
            }
        }
        for(int i=0; i<statusDesTooltipList.Count; i++)
        {
            if (statusDesTooltipList[i].statusDesText.text == string.Empty && i< count)
            {
                statusDesTooltipList[i].statusDesText.text = description;
                break;
            }
        }
    }
    public void GetCharacterCardInfo(CharacterCardData characterCardData)
    {
        currentCharacterCardData = characterCardData;

        characterCardImage.sprite = currentCharacterCardData.cardSprite;
        healthText.text = currentCharacterCardData.maxHealth.ToString();
        cardNameText.text = currentCharacterCardData.characterName;
        combatTypeImage.sprite = currentCharacterCardData.characterCard.combatType.combatTypeSprite;

        //weakness
        Weakness[] weakness = currentCharacterCardData.characterCard.weakness.ToArray();
        if(weakness.Length >= 2)
        {
            weaknessImage1.sprite = weakness[0].weaknessTypeSprite;
            weaknessImage2.sprite = weakness[1].weaknessTypeSprite;
            weaknessImage3.sprite = weakness[2].weaknessTypeSprite;
        }

        //skill
        for (int i = 0; i < currentCharacterCardData.characterCard.characterSkillList.Count; i++)
        {
            CharacterSkill characterSkill = currentCharacterCardData.characterCard.characterSkillList[i];
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
