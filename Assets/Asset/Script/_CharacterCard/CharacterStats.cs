using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStats : MonoBehaviour
{

    [Header("Status List")]
    public List<Status> statusList;
    [Header("Weakness List")]
    public List<Combat> weaknessList;
    [Header("Breaking List")]
    public List<WeaknessBreaking> breakingList;

    public bool isChoosing;
    public bool isActionCharacter;
    public bool isDead;
    public bool isHighlighting;
    public bool isApplyingStatus;
    public bool isApplyingWeakness;
    public bool isApplyBreaking;

    [Header("Status")]
    public bool isSkippingRound;
    public bool isIncreasingAttack;
    public bool isReducingSkillActionPoints;
    public bool isDoublingDamage;
    public bool isReviving;
    public bool isShield;
    public bool isSatiated;
    public bool isDeadFirst;
    public bool isBleeding;
    public bool isDetention;
    public bool isFreezing;

    [Header("Weakness")]
    public bool isFireWeakness;
    public bool isIceWeakness;
    public bool isImaginaryWeakness;
    public bool isWindWeakness;
    public bool isLightningWeakness;
    public bool isQuantumWeakness;
    public bool isPhysicalWeakness;

    [Header("Status Image")]
    public Image deadImage;
    public Image healingStatusImage;
    public Image increaseAttackStatusImage;
    public Image satiatedStatusImage;
    public Image shieldImage;
    public Image revivalImage;
    public Image skipRoundImage;
    public Image doubleDamageImage;
    public Image bleedImage;
    public Image detentionImage;
    public Image freezeImage;

    [Header("Weakness Image")]
    public Image fireImage;
    public Image iceImage;
    public Image imaginaryImage;
    public Image windImage;
    public Image lightningImage;
    public Image quantumImage;
    public Image physicalImage;

    [Header("Take Damage")]
    public GameObject takeDamageObj;
    public TextMeshProUGUI takeDamageText;

    [Header("Take Weakness")]
    public GameObject takeWeaknessObj;
    public TextMeshProUGUI takeWeaknessText;

    [Header("Data")]
    public WeaknessStateData weaknessBreakingStateData;

    [Header("Component")]
    public CharacterCard characterCard;

    private Coroutine takeDamageCoroutine;
    private Coroutine takeWeaknessCoroutine;
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
        if(statusList.Count > 0)
        {
            isApplyingStatus = true;
        }
        else
        {
            isApplyingStatus = false;
        }
        if (weaknessList.Count > 0)
        {
            isApplyingWeakness = true;
        }
        else
        {
            isApplyingWeakness = false;
        }
        if(breakingList.Count > 0)
        {
            isApplyBreaking = true;
        }
        else
        {
            isApplyBreaking = false;
        }
    }
    //state
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
    public void DoubleDamageState(bool state)
    {
        isDoublingDamage = state;
        doubleDamageImage.gameObject.SetActive(state);
    }
    public IEnumerator TakeDamageState(int value)
    {
        if (takeDamageCoroutine != null)
            StopCoroutine(takeDamageCoroutine);

        takeDamageText.text = "-" + value.ToString();
        takeDamageObj.SetActive(false);
        takeDamageObj.SetActive(true);
        yield return new WaitForSeconds(2f);
        takeDamageObj.SetActive(false);
    }
    public IEnumerator DeadState(bool state)
    {
        isDead = state;

        SetSwitchCharacterDying();
        SetCharacterDeadFirst();
        yield return new WaitForSeconds(2);
        ClearAllStatus();
        ClearAllWeakness();
        ClearAllBreaking();

        ClearStatusList();
        ClearWeaknessList();
        ClearBreakingList();
        deadImage.gameObject.SetActive(state);
        gamePlayManager.UpdateGameState(GamePlayState.SelectBattleCharacter);
        characterCard.characterCardDragHover.HandleCardSelected();
        CheckAllDead();
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
    public void ApplyStatus(ActionCardActionSkillType actionCardActionSkillType)
    {
        if (isDead) return;

        switch (actionCardActionSkillType)
        {
            case ActionCardActionSkillType.Healing: SatiatedState(true); 
                break;
            case ActionCardActionSkillType.IncreaseAttack: IncreaseAttackState(true); 
                break;
            case ActionCardActionSkillType.CreateShield: ShieldState(true); 
                break;
            case ActionCardActionSkillType.DoubleDamage: DoubleDamageState(true); 
                break;
            case ActionCardActionSkillType.SkipRound: SkipRoundState(true); 
                break;
            case ActionCardActionSkillType.Revival: RevivalState(true); 
                break;
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

            foreach (CharacterSkill characterSkill in characterCard.characterCardData.characterCard.characterSkillList)
            {
                foreach (Skill skill in characterSkill.actionSkillList)
                {
                    skill.actionValue /= 2;
                }
            }

            DoubleDamageState(false);
            RemoveStatus(StatusType.isDoublingDamage);
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
        isApplyingStatus = false;
        ClearStatus(ActionCardActionSkillType.Healing);
        ClearStatus(ActionCardActionSkillType.IncreaseAttack);
        ClearStatus(ActionCardActionSkillType.CreateShield);
        ClearStatus(ActionCardActionSkillType.SkipRound);
        ClearStatus(ActionCardActionSkillType.DoubleDamage);
        ClearStatus(ActionCardActionSkillType.Revival);   
    }
    public void ClearAllWeakness()
    {
        isApplyingStatus = false;
        ClearWeakness(WeaknessType.Lightning);
        ClearWeakness(WeaknessType.Ice);
        ClearWeakness(WeaknessType.Fire);
        ClearWeakness(WeaknessType.Wind);
        ClearWeakness(WeaknessType.Quantum);
        ClearWeakness(WeaknessType.Imaginary);
        ClearWeakness(WeaknessType.Physical);
    }
    public void ClearAllBreaking()
    {
        isApplyBreaking = false;
        ClearBreaking(WeaknessType.Ice);
        ClearBreaking(WeaknessType.Imaginary);
        ClearBreaking(WeaknessType.Fire);
        ClearBreaking(WeaknessType.Physical);
        ClearBreaking(WeaknessType.Wind);
        ClearBreaking(WeaknessType.Lightning);
        ClearBreaking(WeaknessType.Quantum);
    }
    public void ClearStatusList()
    {
        statusList.Clear();
    }
    public void ClearWeaknessList()
    {
        weaknessList.Clear();
    }
    public void ClearBreakingList()
    {
        breakingList.Clear();
    }

    //-------------------------weakness
    public IEnumerator Bleed(WeaknessType weaknessType ,int value)
    {
        TakeDamage(1);
        yield return new WaitForSeconds(1);
        TakeWeakness(weaknessType, 0);
    }
    public void FireWeaknessState(bool state)
    {
        isFireWeakness = state;
        fireImage.gameObject.SetActive(state);
    }
    public void IceWeaknessState(bool state)
    {
        isIceWeakness = state;
        iceImage.gameObject.SetActive(state);
    }
    public void QuantumWeaknessState(bool state)
    {
        isQuantumWeakness = state;
        quantumImage.gameObject.SetActive(state);
    }
    public void WindWeaknessState(bool state)
    {
        isWindWeakness = state;
        windImage.gameObject.SetActive(state);
    }
    public void ImaginaryWeaknessState(bool state)
    {
        isImaginaryWeakness = state;
        imaginaryImage.gameObject.SetActive(state);
    }
    public void PhysicalWeaknessState(bool state)
    {
        isPhysicalWeakness = state;
        physicalImage.gameObject.SetActive(state);
    }
    public void LightningWeaknessState(bool state)
    {
        isLightningWeakness = state;
        lightningImage.gameObject.SetActive(state);
    }
    public void BleedState(bool state)
    {
        isBleeding = state;
        bleedImage.gameObject.SetActive(state);
    }
    public void DetentionState(bool state)
    {
        isDetention = state;
        detentionImage.gameObject.SetActive(state);
    }
    public void FreezeState(bool state)
    {
        isFreezing = state;
        freezeImage.gameObject.SetActive(state);
    }
    public void TakeDamageWeakness(int value, WeaknessType weaknessType)
    {
        if (isDead || isApplyBreaking) return;
        characterCard.currentWeakness -= value;
        if(characterCard.currentWeakness <= 0)
        {
            characterCard.currentWeakness = 0;
            StartCoroutine(WeaknessBreakingState(true, weaknessType));
            AddBreaking(weaknessType);
        }
        characterCard.SetWeaknessText();

        takeWeaknessCoroutine = StartCoroutine(TakeWeaknessState(value));
    }
    public IEnumerator TakeWeaknessState(int value)
    {
        if (takeWeaknessCoroutine != null)
            StopCoroutine(takeWeaknessCoroutine);

        takeWeaknessText.text = "-" + value.ToString();
        takeWeaknessObj.SetActive(true);
        yield return new WaitForSeconds(2f);
        takeWeaknessObj.SetActive(false);
    }
    public IEnumerator WeaknessBreakingState(bool state, WeaknessType weaknessType)
    {
        switch (weaknessType)
        {
            case WeaknessType.Imaginary:
                DetentionState(state);
                break;
            case WeaknessType.Ice:
                FreezeState(state);
                break;
            default:
                BleedState(state);
                break;
        }
        yield return null;
    }
    public void TakeWeakness(WeaknessType weaknessType, int value)
    {
        if (isDead) return;

        switch (weaknessType)
        {
            case WeaknessType.Fire:
                if (isFireWeakness)
                {
                    TakeDamageWeakness(value, weaknessType);
                    ClearWeakness(weaknessType);
                    return;
                }
                FireWeaknessState(true);
                break;

            case WeaknessType.Ice:
                if (isIceWeakness)
                {
                    TakeDamageWeakness(value, weaknessType);
                    ClearWeakness(weaknessType);
                    return;
                }
                IceWeaknessState(true);
                break;

            case WeaknessType.Quantum:
                if (isQuantumWeakness)
                {
                    TakeDamageWeakness(value, weaknessType);
                    ClearWeakness(weaknessType);
                    return;
                }
                QuantumWeaknessState(true);
                break;

            case WeaknessType.Imaginary:
                if (isImaginaryWeakness)
                {
                    TakeDamageWeakness(value, weaknessType);
                    ClearWeakness(weaknessType);
                    return;
                }
                ImaginaryWeaknessState(true);
                break;

            case WeaknessType.Lightning:
                if (isLightningWeakness)
                {
                    TakeDamageWeakness(value, weaknessType);

                    ClearWeakness(weaknessType);
                    return;
                }
                LightningWeaknessState(true);
                break;

            case WeaknessType.Wind:
                if (isWindWeakness)
                {
                    TakeDamageWeakness(value, weaknessType);

                    ClearWeakness(weaknessType);
                    return;
                }
                WindWeaknessState(true);
                break;

            case WeaknessType.Physical:
                if (isPhysicalWeakness)
                {
                    TakeDamageWeakness(value, weaknessType);

                    ClearWeakness(weaknessType);
                    return;
                }
                PhysicalWeaknessState(true);
                break;
        }
    }
    public void ClearBreaking(WeaknessType weaknessType)
    {
        switch (weaknessType)
        {
            case WeaknessType.Ice:
                FreezeState(false);
                RemoveBreaking(weaknessType);
                break;
            case WeaknessType.Imaginary:
                DetentionState(false);
                RemoveBreaking(weaknessType);
                break;
            default:
                BleedState(false);
                RemoveBreaking(weaknessType);
                break;
        }
    }
    public void AddBreaking(WeaknessType weaknessType)
    {
        foreach (WeaknessBreaking weaknessBreaking in weaknessBreakingStateData.weaknessBreakingList)
        {
            if (weaknessType == weaknessBreaking.weaknessType)
            {
                breakingList.Add(weaknessBreaking);
            }
        }
    }
    public void RemoveBreaking(WeaknessType weaknessType)
    {
        if (breakingList.Count == 0) return; 
        foreach(WeaknessBreaking weaknessBreaking in breakingList)
        {
            if(weaknessType == weaknessBreaking.weaknessType)
            {
                breakingList.Remove(weaknessBreaking);
                break;
            }
        }
    }
    public void ClearWeakness(WeaknessType weaknessType)
    {
        switch (weaknessType)
        {
            case WeaknessType.Fire:
                FireWeaknessState(false);
                RemoveWeakness(weaknessType);
                break;
            case WeaknessType.Ice:
                IceWeaknessState(false);
                RemoveWeakness(weaknessType);
                break;
            case WeaknessType.Quantum:
                QuantumWeaknessState(false);
                RemoveWeakness(weaknessType);
                break;
            case WeaknessType.Imaginary:
                ImaginaryWeaknessState(false);
                RemoveWeakness(weaknessType);
                break;
            case WeaknessType.Lightning:
                LightningWeaknessState(false);
                RemoveWeakness(weaknessType);
                break;
            case WeaknessType.Wind:
                WindWeaknessState(false);
                RemoveWeakness(weaknessType);
                break;
            case WeaknessType.Physical:
                PhysicalWeaknessState(false);
                RemoveWeakness(weaknessType);
                break;
        }
    }
    public void AddWeakness(Combat weakness)
    {
        bool canAddWeakness = true;
        foreach (Combat w in weaknessList)
        {
            if (w == weakness)
            {
                canAddWeakness = false;
                break;
            }
        }
        if (canAddWeakness)
        {
            weaknessList.Add(weakness);
        }
    }
    public void RemoveWeakness(WeaknessType weaknessType)
    {
        if (weaknessList.Count == 0) return;
        foreach (Combat weakness in weaknessList)
        {
            if (weaknessType == weakness.combatType)
            {
                weaknessList.Remove(weakness);
                break;
            }
        }
    }
}
