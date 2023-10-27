using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class OptionalToolCanvas : CanvasBase
{
    [Header("Action Card")]
    public OptionalToolActionCardImage actionCardImage;
    public OptionalToolActionCardDescription actionCardDescription;

    [Header("Character Card")]
    public OptionalToolCharacterCardImage characterImage;
    public OptionalToolCharacterCardDescription characterDescription;
    public CharacterCard currentCharacterCard;
    public ActionCard currentActionCard;
    public void GetCharacterCardInfo(CharacterCard characterCard)
    {
        currentCharacterCard = characterCard;

        OptionalToolCharacterCardState(true);
        OptionalToolActionCardState(false);

        characterImage.cardImage.sprite = characterCard.cardImage.sprite;
        characterImage.healthText.text = characterCard.healthText.text;

        characterDescription.cardNameText.text = characterCard.nameText.text;
        Sprite combatTypeSprite = characterCard.characterCardData.characterCard.combat.combatTypeSprite;
        characterDescription.combatTypeImage.sprite = combatTypeSprite;

        //weakness

        //skill
        foreach (CharacterSkill characterSkill in characterCard.characterCardData.characterCard.characterSkillList)
        {
            CharacterCardSkillType skillType = characterSkill.characterCardSkillType;

            //normal atk
            if (skillType == CharacterCardSkillType.NormalAttack)
            {
                characterDescription.nAImage.sprite = characterSkill.skillSprite;
                characterDescription.nANameText.text = "Normal Attack";
                characterDescription.nAActionPointCostText.text = currentCharacterCard.currentNAActionPointCost.ToString();
                SetDes(characterSkill, characterDescription.nADesText);
            }

            //elemental skill
            else if (skillType == CharacterCardSkillType.ElementalSkill)
            {
                characterDescription.eSImage.sprite = characterSkill.skillSprite;
                characterDescription.eSNameText.text = "Normal Attack";
                characterDescription.eSActionPointCostText.text = currentCharacterCard.currentESActionPointCost.ToString();
                SetDes(characterSkill, characterDescription.eSDesText);
            }

            //elemental burst
            else if (skillType == CharacterCardSkillType.ElementalBurst)
            {
                characterDescription.eBImage.sprite = characterSkill.skillSprite;
                characterDescription.eBNameText.text = "Normal Attack";
                characterDescription.eBActionPointCostText.text = currentCharacterCard.currentESActionPointCost.ToString();
                characterDescription.eBBurstPointCostText.text = characterSkill.burstPointCost.ToString();
                SetDes(characterSkill, characterDescription.eBDesText);
            }
        }

    }
    public void SetDes(CharacterSkill characterSkill, TextMeshProUGUI text)
    {
        var str = new StringBuilder();
        List<SkillDescription> skillDesList = characterSkill.skillDescriptionList;
        foreach (SkillDescription skillDes in skillDesList)
        {
            if (skillDes.canModified)
            {
                foreach (Skill skill in characterSkill.skillList)
                {
                    int value = GetValueForSkill(characterSkill);
                    if (skill.skillActionType == CharacterCardActionSkillType.Attack)
                    {
                        CharacterStats stat = currentCharacterCard.characterStats;
                        if(stat.isDoublingDamage || stat.isIncreasingAttack)
                        {
                            str.Append("<color=yellow>" + value + "</color> ");
                        }
                        else
                        {
                            str.Append(value + " ");
                        }
                    }
                }
            }
            else
            {
                str.Append(skillDes.description + " ");
            }
        }
        text.text = str.ToString();
    }
    private int GetValueForSkill(CharacterSkill characterSkill)
    {
        if (characterSkill.characterCardSkillType == CharacterCardSkillType.NormalAttack)
        {
            return currentCharacterCard.currentNAActionValue;
        }
        else if (characterSkill.characterCardSkillType == CharacterCardSkillType.ElementalSkill)
        {
            return currentCharacterCard.currentESActionValue;
        }
        else if (characterSkill.characterCardSkillType == CharacterCardSkillType.ElementalBurst)
        {
            return currentCharacterCard.currentEBActionValue;
        }
        return 0;
    }
    public void GetActionCardInfo(ActionCard actionCard)
    {
        currentActionCard = actionCard;

        OptionalToolActionCardState(true);
        OptionalToolCharacterCardState(false);

        actionCardImage.cardImage.sprite = actionCard.cardImage.sprite;
        actionCardImage.backImage.color = actionCard.backImage.color;
        actionCardImage.actionCardCostText.text = actionCard.actionCostText.text;

        actionCardDescription.cardDescriptionText.text = actionCard.actionCardData.cardDescription;
        actionCardDescription.cardNameText.text = actionCard.actionCardData.cardName;
    }
    public void OptionalToolActionCardState(bool state)
    {
        actionCardImage.gameObject.SetActive(state);
        actionCardDescription.gameObject.SetActive(state);
    }
    public void OptionalToolCharacterCardState(bool state)
    {
        characterImage.gameObject.SetActive(state);
        characterDescription.gameObject.SetActive(state);   
    }
}
