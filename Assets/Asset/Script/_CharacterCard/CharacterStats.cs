using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStats : MonoBehaviour
{

    [Header("Status")]
    public List<Status> statusList;

    [Header("Apply Status")]
    public bool isChoosing;
    public bool isActionCharacter;
    public bool isDead;
    public bool isIncreasingAttack;
    public bool isReviving;
    public bool isShield;
    public bool isSatiated;
    public bool isUsingHealing;
    public bool isHighlighting;
    public bool isApplyingStatus;

    [Header("Image")]
    public Image healingStatusImage;
    public Image increaseAttackStatusImage;
    public Image satiatedStatusImage;
    public Image shieldImage;
    public Image revivalImage;

    [Header("Take Damage")]
    public GameObject takeDamageObj;
    public TextMeshProUGUI takeDamageText;

    [Header("Component")]
    public CharacterCard characterCard;

    private Coroutine takeDamageCoroutine;
    protected TooltipManager tooltipManager => TooltipManager.instance;
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
        if(isIncreasingAttack || isReviving || isIncreasingAttack || isShield || isSatiated)
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

        if (value >= 0)
            takeDamageText.text = "-" + value.ToString();
        else
            takeDamageText.text = "+" + value.ToString();
        takeDamageObj.SetActive(false);
        takeDamageObj.SetActive(true);
        yield return new WaitForSeconds(2f);
        takeDamageObj.SetActive(false);
    }
    public void HealingState(bool state)
    {
        isUsingHealing = state;
        healingStatusImage.gameObject.SetActive(state);
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
    public void DeadState(bool state)
    {
        isDead = state;
    }
    public void Healing(int value)
    {
        Debug.Log("Healing");

        if (characterCard.currentHealth + value > characterCard.characterCardData.maxHealth)
            characterCard.currentHealth = characterCard.characterCardData.maxHealth;
        else
            characterCard.currentHealth += value;

        characterCard.SetHealthText();
    }
    public void TakeDamage(int value)
    {
        Debug.Log("Take Damage");
        if (isShield)
        {
            int shieldValue = characterCard.currentShield;
            Shield(-value);
            value -= shieldValue;
        }
        characterCard.currentHealth -= value;

        if (characterCard.currentHealth <= 0)
        {
            characterCard.currentHealth = 0;
            DeadState(true);
        }

        characterCard.SetHealthText();
        characterCard.SetShieldText();
        takeDamageCoroutine = StartCoroutine(TakeDamageState(value));

    }
    public void Shield(int value)
    {
        Debug.Log("Shield");
        isShield = true;
        characterCard.currentShield += value;
        if(characterCard.currentShield <= 0)
        {
            isShield = false;
            characterCard.currentShield = 0;
            shieldImage.gameObject.SetActive(false);
        }
        characterCard.SetShieldText();
    }
    public void Damage(int value)
    {
        Debug.Log("Damage");
        characterCard.currentHealth -= value;
        if(characterCard.currentHealth <= 0)
        {
            characterCard.currentHealth = 0;
            isDead = true;
        }
    }
    public void IncreaseAttack(int value)
    {
        Debug.Log("Increase Attack");
        characterCard.currentIncreaseAttack += value;
        //foreach (CharacterSkill characterSkill in characterCard.characterCardData.characterCard.characterSkillList)
        //{
        //    foreach (Skill skill in characterSkill.actionSkillList)
        //    {
        //        skill.actionValue += value;
        //    }
        //}
    }
    public void IncreaseBurstPoint(int value)
    {
        Debug.Log("Increase Burst Point");
        characterCard.SetBurstPoint(-value);
    }
    public void ApplyStatus(ActionCardActionSkillType actionCardActionSkillType)
    {
        if(actionCardActionSkillType == ActionCardActionSkillType.Healing)
        {
            HealingState(true);
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
    }
    public void ClearStatus(ActionCardActionSkillType actionCardActionSkillType)
    {
        if (actionCardActionSkillType == ActionCardActionSkillType.IncreaseAttack)
        {
            IncreaseAttackState(false);
        }
    }
}
