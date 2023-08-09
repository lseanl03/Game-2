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
        SetHighlightCard(CharacterCardSkillType.NormalAttack);
    }
    public void ElementalSkill()
    {
        SetHighlightCard(CharacterCardSkillType.ElementalSkill);
    }
    public void ElementalBurst()
    {
        SetHighlightCard(CharacterCardSkillType.ElementalBurst);
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
                    image.color = colorShowSkill; // Sử dụng màu sắc từ Dictionary
                }
                else
                {
                    image.color = colorHideSkill;
                }
            }
        }
    }
    private void SetHighlightCard(CharacterCardSkillType skillType)
    {
        if (isHighlightActive && skillType != currentHighlightedSkill)
        {
            Debug.Log("b");
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
        else if(isHighlightActive && skillType == currentHighlightedSkill)
        {
            PerformSkill(currentHighlightedSkill, currentCharacterCardData.characterCard.characterSkillList);
        }
        else if (!isHighlightActive)
        {
            foreach (CharacterSkill characterSkill in currentCharacterCardData.characterCard.characterSkillList)
            {
                if (characterSkill.characterCardSkillType == skillType)
                {
                    foreach (Skill skill in characterSkill.actionSkillList)
                    {
                        gamePlayManager.HighlightCardTarget(skill.actionTargetType, skill.actionValue);
                    }
                    currentHighlightedSkill = skillType; // Lưu loại kỹ năng đang được hiển thị
                    isHighlightActive = true;
                    return;
                }
            }
        }
    }
    public void PerformSkill(CharacterCardSkillType currentSkillType, List<CharacterSkill> characterSkillList)
    {
        if(gamePlayManager.currentTurn == TurnState.YourTurn)
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
                        }
                        else if (playerManager.currentSkillPoint < characterSkill.skillPointCost)
                        {
                            notificationManager.SetNewNotification("Skill point are not enough");
                        }
                        else if (playerManager.currentActionPoint < characterSkill.actionPointCost)
                        {
                            notificationManager.SetNewNotification("Action point are not enough");
                        }
                    }
                }
            }

            isHighlightActive = false;
            gamePlayManager.HideHighlightsCard();
            currentHighlightedSkill = CharacterCardSkillType.None;
        }
        else if(gamePlayManager.currentTurn == TurnState.EnemyTurn)
        {
            notificationManager.SetNewNotification("Enemy Turn");
        }
    }

    public void UseSkill(CharacterSkill characterSkill)
    {
        foreach (Skill skill in characterSkill.actionSkillList)
        {
            currentCharacterCard.BurstPointConsumption(characterSkill.burstPointCost);
            gamePlayManager.DealDamageToTargets(skill.actionTargetType, skill.actionValue);
            playerManager.ConsumeActionPoint(characterSkill.actionPointCost);
            playerManager.ConsumeSkillPoint(characterSkill.skillPointCost);
        }
    }

}
