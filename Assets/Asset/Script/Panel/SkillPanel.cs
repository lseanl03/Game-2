using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class SkillPanel : PanelBase
{
    public bool isHighlightActive = false;
    public CharacterCardSkillType currentHighlightedSkill = CharacterCardSkillType.None;

    [Header("NormalAttack")]
    public Image normalAttackImage;
    public TextMeshProUGUI NACostText;
    [Header("ElementalSkill")]
    public Image elementalSkillImage;
    public TextMeshProUGUI ESCostText;
    [Header("ElementalBurst")]
    public Image elementalBurstImage;
    public TextMeshProUGUI EBCostText;
    [Header("Image Color")]
    public Color colorShowSkill;
    public Color colorHideSkill;
    [Header("Data")]
    public CharacterCardData currentCharacterCardData;
    public CharacterCard currentCharacterCard;
    public void Update()
    {
        SetNormalAttackColor();
        SetElementalSkillColor();
        SetElementalBurstColor();
    }
    public void GetCharacterCard(CharacterCardData characterCardData, CharacterCard characterCard)
    {
        currentCharacterCardData = characterCardData;
        currentCharacterCard = characterCard;
    }
    public void SetSkillImage(Sprite NA, Sprite ES, Sprite eB)
    {
        normalAttackImage.sprite = NA;
        elementalSkillImage.sprite = ES;
        elementalBurstImage.sprite = eB;
    }
    public void SetCostText(int NACost, int ESCost, int EBCost)
    {
        NACostText.text = NACost.ToString();
        ESCostText.text = ESCost.ToString();
        EBCostText.text = EBCost.ToString();
    }
    public void NormalAttack()
    {
        HighlightActive(CharacterCardSkillType.NormalAttack);
    }
    public void ElementalSkill()
    {
        HighlightActive(CharacterCardSkillType.ElementalSkill);
    }
    public void ElementalBurst()
    {
        HighlightActive(CharacterCardSkillType.ElementalBurst);
    }

    public void SetNormalAttackColor()
    {
        SetSkillColor(CharacterCardSkillType.NormalAttack, normalAttackImage);
    }
    public void SetElementalSkillColor()
    {
        SetSkillColor(CharacterCardSkillType.ElementalSkill, elementalSkillImage);
    }
    public void SetElementalBurstColor()
    {
        SetSkillColor(CharacterCardSkillType.ElementalBurst, elementalBurstImage);
    }
    public void HideHighLight()
    {
        isHighlightActive = false;
        currentHighlightedSkill = CharacterCardSkillType.None;
    }
    public void SetSkillColor(CharacterCardSkillType characterCardSkillType, Image image)
    {
        foreach (CharacterSkill characterSkill in currentCharacterCardData.characterCard.characterSkillList)
        {
            if (characterSkill.characterCardSkillType == characterCardSkillType)
            {
                bool isShowSkill = false;

                if (characterCardSkillType == CharacterCardSkillType.NormalAttack)
                {
                    isShowSkill = playerManager.currentActionPoint >= characterSkill.actionPointCost &&
                                  playerManager.currentSkillPoint >= characterSkill.skillPointCost;
                }
                else if (characterCardSkillType == CharacterCardSkillType.ElementalSkill)
                {
                    isShowSkill = playerManager.currentActionPoint >= characterSkill.actionPointCost &&
                                  playerManager.currentSkillPoint >= characterSkill.skillPointCost;
                }
                else if (characterCardSkillType == CharacterCardSkillType.ElementalBurst)
                {
                    isShowSkill = playerManager.currentActionPoint >= characterSkill.actionPointCost &&
                                  playerManager.currentSkillPoint >= characterSkill.skillPointCost &&
                                  currentCharacterCard.currentBurstPoint == currentCharacterCardData.burstPointMax;
                }

                if (isShowSkill)
                {
                    image.color = colorShowSkill;
                }
                else
                {
                    image.color = colorHideSkill;
                }
            }
        }
    }
    public void HighlightActive(CharacterCardSkillType skillType)
    {
        if (gamePlayManager.actionPhase && gamePlayManager.currentTurn == TurnState.YourTurn)
        {
            if (isHighlightActive)
            {
                if(skillType != currentHighlightedSkill)
                {
                    SetCurrentHighlight(skillType);
                }
                else
                {
                    PerformSkill(currentHighlightedSkill, currentCharacterCardData.characterCard.characterSkillList);
                }
            }
            else
            {
                isHighlightActive = true;
                SetCurrentHighlight(skillType);
            }
        }
        else if (gamePlayManager.actionPhase && gamePlayManager.currentTurn == TurnState.EnemyTurn)
        {
            notificationManager.SetNewNotification("Enemy Turn");
        }
    }
    public void SetCurrentHighlight(CharacterCardSkillType skillType)
    {
        foreach (CharacterSkill characterSkill in currentCharacterCardData.characterCard.characterSkillList)
        {
            if (characterSkill.characterCardSkillType == skillType)
            {
                foreach (Skill skill in characterSkill.actionSkillList)
                {
                    gamePlayManager.HighlightCardTarget(skill.actionTargetType, skill.actionValue);
                }
                currentHighlightedSkill = skillType; // Cập nhật loại kỹ năng đang được highlight
            }
        }
    }
    public void PerformSkill(CharacterCardSkillType currentSkillType, List<CharacterSkill> characterSkillList)
    {
        foreach (CharacterSkill characterSkill in characterSkillList)
        {
            if (characterSkill.characterCardSkillType == currentSkillType)
            {
                if (playerManager.currentActionPoint >= characterSkill.actionPointCost &&
                    playerManager.currentSkillPoint >= characterSkill.skillPointCost &&
                    currentCharacterCard.currentBurstPoint >= characterSkill.burstPointCost)
                {
                    UseSkill(characterSkill); // Thực hiện kỹ năng
                }
                else
                {
                    if (currentCharacterCard.currentBurstPoint < characterSkill.burstPointCost)
                    {
                        notificationManager.SetNewNotification("Burst point are not enough");
                        return;
                    }
                    else if (playerManager.currentSkillPoint < characterSkill.skillPointCost)
                    {
                        notificationManager.SetNewNotification("Skill point are not enough");
                        return;
                    }
                    else if (playerManager.currentActionPoint < characterSkill.actionPointCost)
                    {
                        notificationManager.SetNewNotification("Action point are not enough");
                        return;
                    }
                }
            }
        }
        gamePlayManager.HideHighlightsCard();
    }

    public void UseSkill(CharacterSkill characterSkill)
    {
        foreach (Skill skill in characterSkill.actionSkillList)
        {
            currentCharacterCard.BurstPointConsumption(characterSkill.burstPointCost);
            playerManager.ConsumeActionPoint(characterSkill.actionPointCost);
            playerManager.ConsumeSkillPoint(characterSkill.skillPointCost);
            gamePlayManager.DealDamageToTargets(skill.actionTargetType, skill.actionValue);
        }
        if(gamePlayManager.currentTurn == TurnState.YourTurn)
        {
            gamePlayManager.UpdateTurnState(TurnState.EnemyTurn);
        }
    }

}
