using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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

    [Header("Take Healing")]
    public GameObject takeHealingObj;
    public TextMeshProUGUI takeHealingText;

    [Header("Data")]
    public WeaknessStateData weaknessBreakingStateData;

    [Header("Component")]
    public CharacterCard characterCard;

    private Coroutine takeDamageCoroutine;
    private Coroutine takeWeaknessCoroutine;
    private Coroutine takeHealingCoroutine;
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
        isApplyingStatus = statusList.Count > 0;
        isApplyingWeakness = weaknessList.Count > 0;
        isApplyBreaking = breakingList.Count > 0;
    }
    #region Status State
    public void IncreaseAttackState(bool state)
    {
        isIncreasingAttack = state;
        increaseAttackStatusImage.gameObject.SetActive(state);
        increaseAttackStatusImage.transform.DOScale(1.5f, 0.25f).SetLoops(2, LoopType.Yoyo);
    }
    public void SatiatedState(bool state)
    {
        isSatiated = state;
        satiatedStatusImage.gameObject.SetActive(state);
        satiatedStatusImage.transform.DOScale(1.5f, 0.25f).SetLoops(2, LoopType.Yoyo);
    }
    public void RevivalState(bool state)
    {
        isReviving = state;
        revivalImage.gameObject.SetActive(state);
        revivalImage.transform.DOScale(1.5f, 0.25f).SetLoops(2, LoopType.Yoyo);
    }
    public void ShieldState(bool state)
    {
        isShield = state;
        shieldImage.gameObject.SetActive(state);
        shieldImage.transform.DOScale(1.5f, 0.25f).SetLoops(2, LoopType.Yoyo);
    }
    public void SkipRoundState(bool state)
    {
        isSkippingRound = state;
        skipRoundImage.gameObject.SetActive(state);
        skipRoundImage.transform.DOScale(1.5f, 0.25f).SetLoops(2, LoopType.Yoyo);
    }
    public void DoubleDamageState(bool state)
    {
        isDoublingDamage = state;
        doubleDamageImage.gameObject.SetActive(state);
        doubleDamageImage.transform.DOScale(1.5f, 0.25f).SetLoops(2, LoopType.Yoyo);
    }
    #endregion
    public IEnumerator TakeHealingState(int value)
    {
        if (takeHealingCoroutine != null)
            StopCoroutine(takeHealingCoroutine);

        AudioManager.instance.PlayHealing();

        takeHealingText.text = "+" + value.ToString();
        takeHealingObj.SetActive(false);
        takeHealingObj.SetActive(true);
        yield return new WaitForSeconds(2f);
        takeHealingObj.SetActive(false);
    }
    public IEnumerator TakeDamageState(int value)
    {
        if (takeDamageCoroutine != null)
            StopCoroutine(takeDamageCoroutine);
        takeDamageText.text = "-" + value.ToString();
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
        characterCard.characterCardDragHover.HandleCardSelected();
        characterCard.PlaySound(SoundType.Die);
        CheckAllDead();
        gamePlayManager.UpdateGameState(GamePlayState.SelectBattleCharacter);
    }
    public void SetCharacterDeadFirst()
    {
        if (!gamePlayManager.playerHaveCharacterDeadFirst)
        {
            foreach (CharacterCard characterCard in gamePlayManager.playerCharacterList)
            {
                if (characterCard == this.characterCard)
                {
                    isDeadFirst = true;
                    gamePlayManager.playerHaveCharacterDeadFirst = true;
                    break;
                }
            }
        }
        if (!gamePlayManager.enemyHaveCharacterDeadFirst)
        {
            foreach (CharacterCard characterCard in gamePlayManager.enemyCharacterList)
            {
                if (characterCard == this.characterCard)
                {
                    characterCard.characterStats.isDeadFirst = true;
                    gamePlayManager.enemyHaveCharacterDeadFirst = true;
                    break;
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
        if (value <= 0) value = 0;
        characterCard.currentHealth -= value;

        if (characterCard.currentHealth <= 0)
        {
            characterCard.currentHealth = 0;
            StartCoroutine(DeadState(true));
        }

        characterCard.SetHealthText();
        characterCard.healthText.transform.DOScale(1.5f, 0.25f).SetLoops(2, LoopType.Yoyo);
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

        if (gamePlayManager.currentTurn == TurnState.YourTurn)
            StartCoroutine(gamePlayManager.PlayerEndRound());
        else if (gamePlayManager.currentTurn == TurnState.EnemyTurn)
            StartCoroutine(gamePlayManager.EnemyEndRound());
    }
    public void DoubleDamage()
    {
        if (isDead) return;
        //Debug.Log("Double Damage");

        foreach (CharacterSkill characterSkill in characterCard.characterCardData.characterCard.characterSkillList)
        {
            foreach (Skill skill in characterSkill.skillList)
            {

                if(characterSkill.characterCardSkillType == CharacterCardSkillType.NormalAttack)
                {
                    characterCard.currentNAActionValue *= 2;
                    characterCard.currentNormalAttackDoubleDamage = characterCard.currentNAActionValue / 2;
                }

                else if(characterSkill.characterCardSkillType == CharacterCardSkillType.ElementalSkill)
                {
                    characterCard.currentESActionValue *= 2;
                    characterCard.currentElementalSkillDoubleDamage = characterCard.currentESActionValue / 2;

                }

                else if(characterSkill.characterCardSkillType == CharacterCardSkillType.ElementalBurst)
                {
                    characterCard.currentEBActionValue *= 2;
                    characterCard.currentElementalBurstDoubleDamage = characterCard.currentEBActionValue / 2;

                }
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
        characterCard.healthText.transform.DOScale(1.5f, 0.25f).SetLoops(2, LoopType.Yoyo);
        takeHealingCoroutine = StartCoroutine(TakeHealingState(value));

    }
    public void Shield(int value)
    {
        if (isDead) return;
        //Debug.Log("Shield");

        characterCard.currentShield += value;
        if (characterCard.currentShield <= 0)
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
        {
            foreach (Skill skill in characterSkill.skillList)
            {
                if (isDoublingDamage)
                {
                    if (characterSkill.characterCardSkillType == CharacterCardSkillType.NormalAttack)
                    {
                        characterCard.currentNAActionValue -= characterCard.currentNormalAttackDoubleDamage;
                        characterCard.currentNAActionValue += characterCard.currentIncreaseAttack;
                        characterCard.currentNAActionValue *= 2;

                        characterCard.currentNormalAttackDoubleDamage += characterCard.currentIncreaseAttack;
                    }

                    else if (characterSkill.characterCardSkillType == CharacterCardSkillType.ElementalSkill)
                    {
                        characterCard.currentESActionValue -= characterCard.currentElementalSkillDoubleDamage;
                        characterCard.currentESActionValue += characterCard.currentIncreaseAttack;
                        characterCard.currentESActionValue *= 2;

                        characterCard.currentElementalSkillDoubleDamage += characterCard.currentIncreaseAttack;
                    }

                    else if (characterSkill.characterCardSkillType == CharacterCardSkillType.ElementalBurst)
                    {
                        characterCard.currentEBActionValue -= characterCard.currentElementalBurstDoubleDamage;
                        characterCard.currentEBActionValue += characterCard.currentIncreaseAttack;
                        characterCard.currentEBActionValue *= 2;

                        characterCard.currentElementalBurstDoubleDamage += characterCard.currentIncreaseAttack;
                    }
                }
                else
                {
                    if (characterSkill.characterCardSkillType == CharacterCardSkillType.NormalAttack)
                    {
                        characterCard.currentNAActionValue += characterCard.currentIncreaseAttack;
                    }

                    else if (characterSkill.characterCardSkillType == CharacterCardSkillType.ElementalSkill)
                    {
                        characterCard.currentESActionValue += characterCard.currentIncreaseAttack;
                    }

                    else if (characterSkill.characterCardSkillType == CharacterCardSkillType.ElementalBurst)
                    {
                        characterCard.currentEBActionValue += characterCard.currentIncreaseAttack;
                    }
                }
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
        characterCard.currentNAActionPointCost -= characterCard.currentReduceSkillActionPoints;
        characterCard.currentESActionPointCost -= characterCard.currentReduceSkillActionPoints;
        characterCard.currentEBActionPointCost -= characterCard.currentReduceSkillActionPoints;

        foreach(CharacterCard characterCard in gamePlayManager.playerCharacterList)
        {
            if (characterCard.characterStats.isActionCharacter)
            {
                uiManager.battleCanvas.skillPanel.SetActionPointCostText
                    (characterCard.currentNAActionPointCost, characterCard.currentESActionPointCost, characterCard.currentEBActionPointCost);
            }
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
            uiManager.battleCanvas.winLosePanel.SetLoseState();
        }
        else if (allEnemiesDead)
        {
            uiManager.battleCanvas.winLosePanel.SetWinState();
        }
    }
    public void ApplyStatus(ActionCardActionSkillType actionCardActionSkillType)
    {
        if (isDead) return;

        switch (actionCardActionSkillType)
        {
            case ActionCardActionSkillType.Healing:
                SatiatedState(true);
                break;
            case ActionCardActionSkillType.IncreaseAttack:
                IncreaseAttackState(true);
                break;
            case ActionCardActionSkillType.CreateShield:
                ShieldState(true);
                break;
            case ActionCardActionSkillType.DoubleDamage:
                DoubleDamageState(true);
                break;
            case ActionCardActionSkillType.SkipRound:
                SkipRoundState(true);
                break;
            case ActionCardActionSkillType.Revival:
                RevivalState(true);
                break;
        }
    }
    public void ClearStatus(ActionCardActionSkillType actionCardActionSkillType)
    {
        if (actionCardActionSkillType == ActionCardActionSkillType.Healing)
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
            if (!isShield) return;

            characterCard.currentShield = 0;

            ShieldState(false);
            RemoveStatus(StatusType.isShield);
        }
        if (actionCardActionSkillType == ActionCardActionSkillType.ReduceSkillActionPoints)
        {
            if (!isReducingSkillActionPoints) return;

            int temp = characterCard.currentReduceSkillActionPoints;
            ReduceSkillActionPoints(-temp);
            characterCard.currentReduceSkillActionPoints = 0;
            isReducingSkillActionPoints = false;

            foreach (CharacterCard characterCard in gamePlayManager.playerCharacterList)
            {
                if (characterCard.characterStats.isActionCharacter)
                {
                    uiManager.battleCanvas.skillPanel.SetActionPointCostText
                        (characterCard.currentNAActionPointCost, characterCard.currentESActionPointCost, characterCard.currentEBActionPointCost);
                }
            }
        }
        if (actionCardActionSkillType == ActionCardActionSkillType.DoubleDamage)
        {
            if (!isDoublingDamage) return;

            foreach (CharacterSkill characterSkill in characterCard.characterCardData.characterCard.characterSkillList)
            {
                foreach (Skill skill in characterSkill.skillList)
                {
                    if (characterSkill.characterCardSkillType == CharacterCardSkillType.NormalAttack)
                    {
                        characterCard.currentNAActionValue -= characterCard.currentNormalAttackDoubleDamage;
                        characterCard.currentNormalAttackDoubleDamage = 0;
                    }

                    else if (characterSkill.characterCardSkillType == CharacterCardSkillType.ElementalSkill)
                    {
                        characterCard.currentESActionValue -= characterCard.currentElementalSkillDoubleDamage;
                        characterCard.currentElementalSkillDoubleDamage = 0;
                    }

                    else if (characterSkill.characterCardSkillType == CharacterCardSkillType.ElementalBurst)
                    {
                        characterCard.currentEBActionValue -= characterCard.currentElementalBurstDoubleDamage;
                        characterCard.currentElementalBurstDoubleDamage = 0;
                    }
                }
            }

            DoubleDamageState(false);
            RemoveStatus(StatusType.isDoublingDamage);
        }
        if (actionCardActionSkillType == ActionCardActionSkillType.SkipRound)
        {
            if (!isSkippingRound) return;

            SkipRoundState(false);
            RemoveStatus(StatusType.isSkippingRound);
        }
        if (actionCardActionSkillType == ActionCardActionSkillType.Revival)
        {
            if (!isReviving) return;

            RevivalState(false);
            RemoveStatus(StatusType.isReviving);
        }
    }
    public void RemoveStatus(StatusType statusType)
    {
        foreach (Status status in statusList)
        {
            if (statusType == status.statusType)
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
    public void ClearAllWeakness()
    {
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
    #region Weakness State
    public void FireWeaknessState(bool state)
    {
        isFireWeakness = state;
        fireImage.gameObject.SetActive(state);
        fireImage.transform.DOScale(1.5f, 0.25f).SetLoops(2, LoopType.Yoyo);
    }
    public void IceWeaknessState(bool state)
    {
        isIceWeakness = state;
        iceImage.gameObject.SetActive(state);
        iceImage.transform.DOScale(1.5f, 0.25f).SetLoops(2, LoopType.Yoyo);
    }
    public void QuantumWeaknessState(bool state)
    {
        isQuantumWeakness = state;
        quantumImage.gameObject.SetActive(state);
        quantumImage.transform.DOScale(1.5f, 0.25f).SetLoops(2, LoopType.Yoyo);
    }
    public void WindWeaknessState(bool state)
    {
        isWindWeakness = state;
        windImage.gameObject.SetActive(state);
        windImage.transform.DOScale(1.5f, 0.25f).SetLoops(2, LoopType.Yoyo);
    }
    public void ImaginaryWeaknessState(bool state)
    {
        isImaginaryWeakness = state;
        imaginaryImage.gameObject.SetActive(state);
        imaginaryImage.transform.DOScale(1.5f, 0.25f).SetLoops(2, LoopType.Yoyo);
    }
    public void PhysicalWeaknessState(bool state)
    {
        isPhysicalWeakness = state;
        physicalImage.gameObject.SetActive(state);
        physicalImage.transform.DOScale(1.5f, 0.25f).SetLoops(2, LoopType.Yoyo);
    }
    public void LightningWeaknessState(bool state)
    {
        isLightningWeakness = state;
        lightningImage.gameObject.SetActive(state);
        lightningImage.transform.DOScale(1.5f, 0.25f).SetLoops(2, LoopType.Yoyo);
    }
    #endregion
    #region Breaking State
    public void BleedState(bool state)
    {
        isBleeding = state;
        bleedImage.gameObject.SetActive(state);
        bleedImage.transform.DOScale(1.5f, 0.25f).SetLoops(2, LoopType.Yoyo);
    }
    public void DetentionState(bool state)
    {
        isDetention = state;
        detentionImage.gameObject.SetActive(state);
        detentionImage.transform.DOScale(1.5f, 0.25f).SetLoops(2, LoopType.Yoyo);
    }
    public void FreezeState(bool state)
    {
        isFreezing = state;
        freezeImage.gameObject.SetActive(state);
        freezeImage.transform.DOScale(1.5f, 0.25f).SetLoops(2, LoopType.Yoyo);
    }
    #endregion
    public IEnumerator Bleed(WeaknessType weaknessType, int value)
    {
        TakeDamage(value);
        yield return new WaitForSeconds(1);
        TakeWeakness(weaknessType, 0);
    }
    public void TakeDamageWeakness(int value, WeaknessType weaknessType)
    {
        if (isDead || isApplyBreaking) return;
        characterCard.currentWeakness -= value;
        if (characterCard.currentWeakness <= 0)
        {
            characterCard.currentWeakness = 0;
            StartCoroutine(WeaknessBreakingState(true, weaknessType));
            AddBreaking(weaknessType);
        }
        characterCard.SetWeaknessText();
        characterCard.weaknessText.transform.DOScale(1.5f, 0.25f).SetLoops(2, LoopType.Yoyo);

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
        foreach (WeaknessBreaking weaknessBreaking in breakingList)
        {
            if (weaknessType == weaknessBreaking.weaknessType)
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
    public void ClearStatusAfterAttack()
    {
        if (isReducingSkillActionPoints)
        {
            ClearStatus(ActionCardActionSkillType.ReduceSkillActionPoints);
        }
        if (isDoublingDamage)
        {
            ClearStatus(ActionCardActionSkillType.DoubleDamage);
        }
        if (isIncreasingAttack)
        {
            ClearStatus(ActionCardActionSkillType.IncreaseAttack);
        }
    }
}
