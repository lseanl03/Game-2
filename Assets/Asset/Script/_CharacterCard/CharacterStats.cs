using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStats : MonoBehaviour
{

    [Header("Status")]
    public List<Status> statusList;

    public bool isChoosing;
    public bool isActionCharacter;
    public bool isDead;
    public bool isHighlighting;
    public bool isApplyingStatus;

    [Header("Apply Status")]
    public bool isSkippingRound;
    public bool isIncreasingAttack;
    public bool isReducingSkillActionPoints;
    public bool isDoublingDamage;
    public bool isReviving;
    public bool isShield;
    public bool isSatiated;
    public bool isDeadFirst;

    [Header("Image")]
    public Image deadImage;
    public Image healingStatusImage;
    public Image increaseAttackStatusImage;
    public Image satiatedStatusImage;
    public Image shieldImage;
    public Image revivalImage;
    public Image skipRoundImage;
    public Image doubleDamageImage;


    [Header("Color")]
    public Color colorDeadStatus;

    [Header("Take Damage")]
    public GameObject takeDamageObj;
    public TextMeshProUGUI takeDamageText;

    [Header("Component")]
    public CharacterCard characterCard;

    private Coroutine takeDamageCoroutine;
    protected TooltipManager tooltipManager => TooltipManager.instance;
    protected UIManager uiManager => UIManager.instance;
    protected GamePlayManager gamePlayManager => GamePlayManager.instance;
    public void Awake()
    {
        characterCard = GetComponent<CharacterCard>();
    }
    public void Update()
    {
        CheckApplyStatus();
    }
    public void CheckApplyStatus()
    {
        if(isIncreasingAttack || isReviving || isIncreasingAttack || isShield || 
            isSatiated || isSkippingRound || isDoublingDamage)
        {
            isApplyingStatus = true;
        }
        else
        {
            isApplyingStatus = false;
        }
    }
    public void GetStatusInfo(Status status)
    {
        statusList.Add(status);
    }
    public void ClearStatusInfo(Status status)
    {
        statusList.Remove(status);
    }
    IEnumerator TakeDamageState(int value)
    {
        if (takeDamageCoroutine != null)
            StopCoroutine(takeDamageCoroutine);

        takeDamageText.text = "-" + value.ToString();
        takeDamageObj.SetActive(false);
        takeDamageObj.SetActive(true);
        yield return new WaitForSeconds(2f);
        takeDamageObj.SetActive(false);
    }
    public void IncreaseAttackState(bool state)
    {
        isIncreasingAttack = state;
        increaseAttackStatusImage.gameObject.SetActive(state);
    }
    public void SatiatedState(bool state)
    {
        isSatiated = state;
        satiatedStatusImage.gameObject.SetActive(state);
    }
    public void RevivalState(bool state)
    {
        isReviving = state;
        revivalImage.gameObject.SetActive(state);
    }
    public void ShieldState(bool state)
    {
        isShield = state;
        shieldImage.gameObject.SetActive(state);
    }
    public void SkipRoundState(bool state)
    {
        isSkippingRound = state;
        skipRoundImage.gameObject.SetActive(state);
    }
    private void DoubleDamageState(bool state)
    {
        isDoublingDamage = state;
        doubleDamageImage.gameObject.SetActive(state);
    }
    public void SetCharacterDeadFirst()
    {
        if (!gamePlayManager.playerHaveCharacterDeadFirst)
        {
            foreach (CharacterCard characterCard in gamePlayManager.playerCharacterList)
            {
                if(characterCard == this.characterCard)
                {
                    if (characterCard.characterStats.isDead)
                    {
                        characterCard.characterStats.isDeadFirst = true;
                        gamePlayManager.playerHaveCharacterDeadFirst = true;
                        break;
                    }
                }
            }
        }
        if(!gamePlayManager.enemyHaveCharacterDeadFirst)
        {
            foreach (CharacterCard characterCard in gamePlayManager.enemyCharacterList)
            {
                if (characterCard == this.characterCard)
                {
                    if (characterCard.characterStats.isDead)
                    {
                        characterCard.characterStats.isDeadFirst = true;
                        gamePlayManager.enemyHaveCharacterDeadFirst = true;
                        break;
                    }
                }
            }
        }

    }
    public void SetSwitchCharacterDying()
    {
        if (transform.parent == gamePlayManager.gamePlayCanvas.playerCharacterCardField.transform)
        {
            gamePlayManager.playerCanSwitchCharacterDying = true;
        }
        else if (transform.parent == gamePlayManager.gamePlayCanvas.enemyCharacterCardField.transform)
        {
            gamePlayManager.enemyCanSwitchCharacterDying = true;
        }
    }
    public void CheckAllDead()
    {
        bool allAlliesDead = true;
        bool allEnemiesDead = true;
        if (allAlliesDead && allEnemiesDead)
        {
            foreach (CharacterCard characterCard in gamePlayManager.playerCharacterList)
            {
                if (!characterCard.characterStats.isDead)
                {
                    allAlliesDead = false;
                    break;
                }
            }
            foreach (CharacterCard characterCard in gamePlayManager.enemyCharacterList)
            {
                if (!characterCard.characterStats.isDead)
                {
                    allEnemiesDead = false;
                    break;
                }
            }
        }
        if (allAlliesDead)
        {
            uiManager.battleCanvas.winLosePanel.SetWinLoseText("You Lose");
            gamePlayManager.UpdateGameState(GamePlayState.Lose);
        }
        else if (allEnemiesDead)
        {
            uiManager.battleCanvas.winLosePanel.SetWinLoseText("You Win");
            gamePlayManager.UpdateGameState(GamePlayState.Victory);
        }
    }
    public IEnumerator DeadState(bool state)
    {
        isDead = state;

        SetSwitchCharacterDying();
        SetCharacterDeadFirst();
        yield return new WaitForSeconds(2);
        ClearAllStatus();
        RemoveStatusList();
        deadImage.gameObject.SetActive(state);
        gamePlayManager.UpdateGameState(GamePlayState.SelectBattleCharacter);
        characterCard.characterCardDragHover.HandleCardSelected();
        CheckAllDead();
    }
    public void Revival(int value)
    {
        if (!isDead) return;

        //Debug.Log("Revival");
        isDead = false;
        deadImage.gameObject.SetActive(false);

        characterCard.currentHealth += value;
        characterCard.SetHealthText();
    }
    public void SkipRound()
    {
        if (isDead) return;
        //Debug.Log("Skip Round");

        if(gamePlayManager.currentTurn == TurnState.YourTurn)
        StartCoroutine(gamePlayManager.PlayerEndRound());
        else if(gamePlayManager.currentTurn == TurnState.EnemyTurn)
        StartCoroutine(gamePlayManager.EnemyEndRound());
    }
    public void DoubleDamage()
    {
        if (isDead) return;
        //Debug.Log("Double Damage");

        foreach (CharacterSkill characterSkill in characterCard.characterCardData.characterCard.characterSkillList)
        {
            foreach (Skill skill in characterSkill.actionSkillList)
            {
                skill.actionValue *= 2;
            }
        }
    }
    public void Healing(int value)
    {
        if (isDead) return;
        //Debug.Log("Healing");

        if (characterCard.currentHealth + value > characterCard.characterCardData.maxHealth)
            characterCard.currentHealth = characterCard.characterCardData.maxHealth;
        else
            characterCard.currentHealth += value;

        characterCard.SetHealthText();
    }
    public void TakeDamage(int value)
    {
        if (isDead) return;
        //Debug.Log("Take Damage");

        if (isShield)
        {
            int shieldValue = characterCard.currentShield;
            Shield(-value);
            value -= shieldValue;
        }
        if (isSkippingRound)
        {
            value = 0;
        }
        characterCard.currentHealth -= value;

        if (characterCard.currentHealth <= 0)
        {
            characterCard.currentHealth = 0;
            StartCoroutine(DeadState(true));
        }

        characterCard.SetHealthText();
        characterCard.SetShieldValueText();

        takeDamageCoroutine = StartCoroutine(TakeDamageState(value));

    }
    public void Shield(int value)
    {
        if (isDead) return;
        //Debug.Log("Shield");

        characterCard.currentShield += value;
        if(characterCard.currentShield <= 0)
        {
            characterCard.currentShield = 0;
            shieldImage.gameObject.SetActive(false);
            ClearStatus(ActionCardActionSkillType.CreateShield);
        }
        characterCard.SetShieldValueText();
    }
    public void IncreaseAttack(int value)
    {
        if (isDead) return;
        //Debug.Log("IncreaseAttack");

        characterCard.currentIncreaseAttack = value;
        foreach (CharacterSkill characterSkill in characterCard.characterCardData.characterCard.characterSkillList)
        { //8
            foreach (Skill skill in characterSkill.actionSkillList)
            {
                skill.actionValue += characterCard.currentIncreaseAttack;
            }
        }
        characterCard.SetIncreaseValueText();
    }
    public void IncreaseBurstPoint(int value)
    {
        if (isDead) return;
        //Debug.Log("IncreaseBurstPoint");

        characterCard.SetBurstPoint(-value);
    }
    public void ReduceSkillActionPoints(int value)
    {
        if (isDead) return;
        //Debug.Log("ReduceSkillActionPoints");

        isReducingSkillActionPoints = true;
        characterCard.currentReduceSkillActionPoints = value;

        foreach (CharacterSkill characterSkill in characterCard.characterCardData.characterCard.characterSkillList)
            characterSkill.actionPointCost -= characterCard.currentReduceSkillActionPoints;

        if(gamePlayManager.currentTurn == TurnState.YourTurn)
        {
            CharacterSkill[] skill = characterCard.characterCardData.characterCard.characterSkillList.ToArray();
            uiManager.battleCanvas.skillPanel.
                SetActionPointCostText(skill[0].actionPointCost, skill[1].actionPointCost, skill[2].actionPointCost);
        }
    }
    public void ApplyStatus(ActionCardActionSkillType actionCardActionSkillType)
    {
        if (isDead) return;

        if (actionCardActionSkillType == ActionCardActionSkillType.Healing)
        {
            SatiatedState(true);
        }
        if(actionCardActionSkillType == ActionCardActionSkillType.IncreaseAttack)
        {
            IncreaseAttackState(true);
        }
        if(actionCardActionSkillType == ActionCardActionSkillType.CreateShield)
        {
            ShieldState(true);
        }
        if(actionCardActionSkillType == ActionCardActionSkillType.DoubleDamage)
        {
            DoubleDamageState(true);
        }
        if(actionCardActionSkillType == ActionCardActionSkillType.SkipRound)
        {
            SkipRoundState(true);
        }
        if(actionCardActionSkillType == ActionCardActionSkillType.Revival)
        {
            RevivalState(true);
        }
    }

    public void ClearStatus(ActionCardActionSkillType actionCardActionSkillType)
    {
        if(actionCardActionSkillType == ActionCardActionSkillType.Healing)
        {
            if (!isSatiated) return;

            SatiatedState(false);
            RemoveStatus(StatusType.isSatiated);
        }
        if (actionCardActionSkillType == ActionCardActionSkillType.IncreaseAttack)
        {
            if (!isIncreasingAttack) return;

            int temp = characterCard.currentIncreaseAttack;
            IncreaseAttack(-temp);
            characterCard.currentIncreaseAttack = 0;
            IncreaseAttackState(false);
            RemoveStatus(StatusType.isIncreasingAttack);
        }
        if (actionCardActionSkillType == ActionCardActionSkillType.CreateShield)
        {
            if(!isShield) return;

            characterCard.currentShield = 0;
            ShieldState(false);
            RemoveStatus(StatusType.isShield);
        }
        if(actionCardActionSkillType == ActionCardActionSkillType.ReduceSkillActionPoints)
        {
            if (!isReducingSkillActionPoints) return;

            int temp = characterCard.currentReduceSkillActionPoints;
            ReduceSkillActionPoints(-temp);
            characterCard.currentReduceSkillActionPoints = 0;
            isReducingSkillActionPoints = false;
        }
        if(actionCardActionSkillType == ActionCardActionSkillType.DoubleDamage)
        {
            if (!isDoublingDamage) return;

            DoubleDamageState(false);
            RemoveStatus(StatusType.isDoublingDamage);

            foreach (CharacterSkill characterSkill in characterCard.characterCardData.characterCard.characterSkillList)
            {
                foreach (Skill skill in characterSkill.actionSkillList)
                {
                    skill.actionValue /= 2;
                }
            }
        }
        if(actionCardActionSkillType == ActionCardActionSkillType.SkipRound)
        {
            if(!isSkippingRound) return;

            SkipRoundState(false);
            RemoveStatus(StatusType.isSkippingRound);
        }
        if(actionCardActionSkillType == ActionCardActionSkillType.Revival)
        {
            if (!isReviving) return;

            RevivalState(false);
            RemoveStatus(StatusType.isReviving);
        }
    }
    public void RemoveStatus(StatusType statusType)
    {
        foreach(Status status in statusList)
        {
            if(statusType == status.statusType)
            {
                statusList.Remove(status);
                break;
            }
        }
    }
    public void ClearAllStatus()
    {
        ClearStatus(ActionCardActionSkillType.Healing);
        ClearStatus(ActionCardActionSkillType.IncreaseAttack);
        ClearStatus(ActionCardActionSkillType.CreateShield);
        ClearStatus(ActionCardActionSkillType.SkipRound);
        ClearStatus(ActionCardActionSkillType.DoubleDamage);
        ClearStatus(ActionCardActionSkillType.Revival);
    }
    public void RemoveStatusList()
    {
        statusList.Clear();
    }
}
