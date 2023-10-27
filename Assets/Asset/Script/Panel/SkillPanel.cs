using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillPanel : PanelBase
{
    public bool isHighlightActive = false;
    public CharacterCardSkillType currentHighlightedSkill = CharacterCardSkillType.None;
    public GameObject SkillObj;

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
    public void SetActionPointCostText(int NACost, int ESCost, int EBCost)
    {
        NACostText.text = NACost.ToString();
        ESCostText.text = ESCost.ToString();
        EBCostText.text = EBCost.ToString();
    }
    public void NormalAttack()
    {
        HighlightActive(CharacterCardSkillType.NormalAttack, currentCharacterCard.currentNAActionPointCost);
    }
    public void ElementalSkill()
    {
        HighlightActive(CharacterCardSkillType.ElementalSkill, currentCharacterCard.currentESActionPointCost);
    }
    public void ElementalBurst()
    {
        HighlightActive(CharacterCardSkillType.ElementalBurst, currentCharacterCard.currentEBActionPointCost);
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
                    isShowSkill = playerManager.currentActionPoint >= currentCharacterCard.currentNAActionPointCost &&
                                  playerManager.currentSkillPoint >= characterSkill.skillPointCost;
                }
                else if (characterCardSkillType == CharacterCardSkillType.ElementalSkill)
                {
                    isShowSkill = playerManager.currentActionPoint >= currentCharacterCard.currentESActionPointCost &&
                                  playerManager.currentSkillPoint >= characterSkill.skillPointCost;
                }
                else if (characterCardSkillType == CharacterCardSkillType.ElementalBurst)
                {
                    isShowSkill = playerManager.currentActionPoint >= currentCharacterCard.currentEBActionPointCost &&
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
    public void HighlightActive(CharacterCardSkillType skillType, int actionCost)
    {
        if (gamePlayManager.playerAttacking) return;

        AudioManager.instance.PlayOnClickSkill();
        if (!uiManager.tutorialCanvas.isShowedAttackTutorial && gamePlayManager.actionPhase)
        {
            uiManager.tutorialCanvas.isShowedAttackTutorial = true;
            uiManager.tutorialCanvas.ActionTutorial(TutorialType.AttackTutorial);
        }
        if (gamePlayManager.actionPhase && gamePlayManager.currentTurn == TurnState.YourTurn &&
            !currentCharacterCard.characterStats.isDead && !currentCharacterCard.characterStats.isFreezing &&
            !currentCharacterCard.characterStats.isDetention)
        {
            if (isHighlightActive)
            {
                if (skillType != currentHighlightedSkill)
                {
                    SetCurrentHighlight(skillType);
                }
                else
                {
                    PerformSkill(currentHighlightedSkill, currentCharacterCardData.characterCard.characterSkillList, actionCost);
                }
            }
            else
            {
                isHighlightActive = true;
                SetCurrentHighlight(skillType);
            }
        }
        else if (currentCharacterCard.characterStats.isFreezing)
            notificationManager.SetNewNotification("The character is frozen");
        else if(currentCharacterCard .characterStats.isDetention)
            notificationManager.SetNewNotification("The character is currently detained");
    }
    public void SetCurrentHighlight(CharacterCardSkillType skillType)
    {
        if (gamePlayManager.playerCanSwitchCharacterDying || gamePlayManager.enemyCanSwitchCharacterDying) return;

        foreach (CharacterSkill characterSkill in currentCharacterCardData.characterCard.characterSkillList)
        {
            if (characterSkill.characterCardSkillType == skillType)
            {
                foreach (Skill skill in characterSkill.skillList)
                {
                    if(skillType == CharacterCardSkillType.NormalAttack)
                        gamePlayManager.HighlightCardTarget(skill.actionTargetType, currentCharacterCard.currentNAActionValue, characterSkill.weaknessBreakValue, currentCharacterCard);
                    else if(skillType == CharacterCardSkillType.ElementalSkill)
                        gamePlayManager.HighlightCardTarget(skill.actionTargetType, currentCharacterCard.currentESActionValue, characterSkill.weaknessBreakValue, currentCharacterCard);
                    else if(skillType == CharacterCardSkillType.ElementalBurst)
                        gamePlayManager.HighlightCardTarget(skill.actionTargetType, currentCharacterCard.currentEBActionValue, characterSkill.weaknessBreakValue, currentCharacterCard);

                }
                currentHighlightedSkill = skillType; // Cập nhật loại kỹ năng đang được highlight
            }
        }
    }
    public void PerformSkill(CharacterCardSkillType currentSkillType, List<CharacterSkill> characterSkillList, int actionCost)
    {
        foreach (CharacterSkill characterSkill in characterSkillList)
        {
            if (characterSkill.characterCardSkillType == currentSkillType)
            {
                if (playerManager.currentActionPoint >= actionCost &&
                    playerManager.currentSkillPoint >= characterSkill.skillPointCost &&
                    currentCharacterCard.currentBurstPoint >= characterSkill.burstPointCost)
                {
                    if(currentSkillType == CharacterCardSkillType.NormalAttack)
                        StartCoroutine(UseSkill(characterSkill, currentCharacterCard.currentNAActionPointCost, currentCharacterCard.currentNAActionValue));
                    else if (currentSkillType == CharacterCardSkillType.ElementalSkill)
                        StartCoroutine(UseSkill(characterSkill, currentCharacterCard.currentESActionPointCost, currentCharacterCard.currentESActionValue));
                    else if(currentSkillType == CharacterCardSkillType.ElementalBurst)
                        StartCoroutine(UseSkill(characterSkill, currentCharacterCard.currentEBActionPointCost, currentCharacterCard.currentEBActionValue));
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
                    else if (playerManager.currentActionPoint < currentCharacterCard.currentNAActionPointCost &&
                        playerManager.currentActionPoint < currentCharacterCard.currentESActionPointCost &&
                        playerManager.currentActionPoint < currentCharacterCard.currentEBActionPointCost)
                    {
                        notificationManager.SetNewNotification("Action point are not enough");
                        if (!uiManager.tutorialCanvas.isShowedEndRoundTutorial)
                        {
                            uiManager.tutorialCanvas.isShowedEndRoundTutorial = true;
                            uiManager.tutorialCanvas.ActionTutorial(TutorialType.EndRoundTutorial);
                        }
                        return;
                    }
                }
            }
        }
        gamePlayManager.HideHighlightsCard();
    }

    public IEnumerator UseSkill(CharacterSkill characterSkill, int actionCost, int actionValue)
    {
        while (currentCharacterCard.characterStats.isDetention || currentCharacterCard.characterStats.isFreezing) 
            yield return null;

        foreach (Skill skill in characterSkill.skillList)
        {
            gamePlayManager.DealDamageToTargets(skill.actionTargetType, actionValue, characterSkill.characterCardSkillType, currentCharacterCard);
        }
        while (gamePlayManager.playerAttacking)
        {
            yield return null;
        }
        currentCharacterCard.SetBurstPoint(characterSkill.burstPointCost);
        playerManager.ConsumeActionPoint(actionCost);
        playerManager.ConsumeSkillPoint(characterSkill.skillPointCost);
        if (gamePlayManager.currentTurn == TurnState.YourTurn)
        {
            if (!gamePlayManager.enemyEndingRound)
                gamePlayManager.UpdateTurnState(TurnState.EnemyTurn);
            else
                notificationManager.SetNewNotification("Your turn continues...");
        }
        ClearStatusUsed();
    }
    public void ClearStatusUsed()
    {
        if (currentCharacterCard.characterStats.isReducingSkillActionPoints)
        {
            currentCharacterCard.characterStats.ClearStatus(ActionCardActionSkillType.ReduceSkillActionPoints);
            uiManager.battleCanvas.skillPanel.SetActionPointCostText
                (currentCharacterCard.currentNAActionPointCost, currentCharacterCard.currentESActionPointCost, currentCharacterCard.currentEBActionPointCost);
        }
        if (currentCharacterCard.characterStats.isDoublingDamage)
        {
            currentCharacterCard.characterStats.ClearStatus(ActionCardActionSkillType.DoubleDamage);
        }
        if (currentCharacterCard.characterStats.isIncreasingAttack)
        {
            currentCharacterCard.characterStats.ClearStatus(ActionCardActionSkillType.IncreaseAttack);
        }
    }

}
