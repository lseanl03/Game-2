using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCard : MonoBehaviour
{
    public float moveDuration = 0.75f;
    public float shakeDuration = 0.25f;
    public float shakeStrength = 10;
    public int vibrato = 10;
    public GameObject placeHolderPrefab;
    private GameObject placeHolder;
    public List<CharacterSound> characterSoundList;
    public AudioSource characterAudioSource;


    [Header("NA Skill")]
    public int currentNAActionPointCost;
    public int currentNAActionValue;
    [Header("ES Skill")]
    public int currentESActionPointCost;
    public int currentESActionValue;
    [Header("EB Skill")]
    public int currentEBActionPointCost;
    public int currentEBActionValue;

    [Header("Info")]
    public CharacterCardData characterCardData;
    public int currentHealth;
    public int currentWeakness;
    public int currentShield;
    public int currentBurstPoint;
    public int currentIncreaseAttack;
    public int currentQuantitySelected;
    public int currentReduceSkillActionPoints;
    public int currentNormalAttackDoubleDamage;
    public int currentElementalSkillDoubleDamage;
    public int currentElementalBurstDoubleDamage;
    public Sprite cardSprite;
    public Image cardImage;
    public WeaknessType combatType;

    [Header("Text")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI weaknessText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI quantityText;
    public TextMeshProUGUI quantitySelectedText;
    public TextMeshProUGUI takeDamageValueText;
    public TextMeshProUGUI takeWeaknessValueText;
    public TextMeshProUGUI shieldValueText;
    public TextMeshProUGUI increaseAttackValueText;

    [Header("GameObject")]
    [SerializeField] private GameObject takeWeaknessValueObj;
    [SerializeField] private GameObject burstPointObj;
    [SerializeField] private GameObject highLightObj;
    [SerializeField] private GameObject quantitySelectedObj;
    [SerializeField] private GameObject[] hiddenObjects;
    [SerializeField] private GameObject[] burstPointIconObjects;

    [Header("Component")]
    public CharacterStats characterStats;
    public CharacterCardDragHover characterCardDragHover;
    public RectTransform rectTransform;
    protected NotificationManager notificationManager => NotificationManager.instance;
    protected GamePlayManager gamePlayManager => GamePlayManager.instance;
    public void Awake()
    {
        characterStats = GetComponent<CharacterStats>();
        characterCardDragHover = GetComponent<CharacterCardDragHover>();
        rectTransform = GetComponent<RectTransform>();
    }
    public void Start()
    {
        if(highLightObj != null) SetTakeDamageHighlight(false);
        if(burstPointObj != null) BurstPointObjState(true);
    }
    public void GetOriginalCardInfo(CharacterCardData characterCardData)
    {
        this.characterCardData = characterCardData;
        currentQuantitySelected = 0;
        currentBurstPoint = 0;
        nameText.text = characterCardData.characterName;
        quantityText.text = characterCardData.quantityMax.ToString();
        currentHealth = characterCardData.maxHealth;
        currentWeakness = characterCardData.maxWeakness;
        combatType = characterCardData.characterCard.combat.combatType;
        SetDescriptionText(characterCardData.description);
        SetCardImage(characterCardData.cardSprite);
        SetWeaknessText();
        SetHealthText();
        GetActionPointCost();
        GetActionValue();

        characterSoundList = characterCardData.characterCard.characterSoundList;
    }
    public void PlaySound(SoundType soundType)
    {
        foreach(CharacterSound characterSound in characterSoundList)
        {
            if(characterSound.audioClip != null)
            {
                if (characterSound.soundType == soundType && !SFXMuting())
                {
                    characterAudioSource.clip = characterSound.audioClip;
                    characterAudioSource.PlayOneShot(characterSound.audioClip);
                }
            }
        }
    }
    public float SoundLength()
    {
        return characterAudioSource.clip.length;
    } 
    public bool SFXMuting()
    {
        return AudioManager.instance.isMutingSFX;
    }
    public void SetWeaknessText()
    {
        weaknessText.text = currentWeakness.ToString();
    }
    public void SetDescriptionText(string text)
    {
        descriptionText.text = text;
    }
    public void SetShieldValueText()
    {
        shieldValueText.text = currentShield.ToString();
    }
    public void SetIncreaseValueText()
    {
        increaseAttackValueText.text = currentIncreaseAttack.ToString();
    }
    public void SetHealthText()
    {
        healthText.text = currentHealth.ToString();
    }
    public void SetCardImage(Sprite sprite)
    {
        cardSprite = sprite;
        cardImage.sprite = cardSprite;
    }
    public void GetActionPointCost()
    {
        foreach(CharacterSkill characterSkill in characterCardData.characterCard.characterSkillList)
        {
            if(characterSkill.characterCardSkillType == CharacterCardSkillType.NormalAttack)
            {
                currentNAActionPointCost = characterSkill.actionPointCost;
            }
            else if(characterSkill.characterCardSkillType == CharacterCardSkillType.ElementalSkill)
            {
                currentESActionPointCost = characterSkill.actionPointCost;
            }
            else if(characterSkill.characterCardSkillType == CharacterCardSkillType.ElementalBurst)
            {
                currentEBActionPointCost = characterSkill.actionPointCost;
            }
        }
    }
    public void GetActionValue()
    {
        foreach (CharacterSkill characterSkill in characterCardData.characterCard.characterSkillList)
        {
            foreach(Skill skill in characterSkill.skillList)
            {
                if (characterSkill.characterCardSkillType == CharacterCardSkillType.NormalAttack)
                {
                    currentNAActionValue = skill.actionValue;
                }
                else if (characterSkill.characterCardSkillType == CharacterCardSkillType.ElementalSkill)
                {
                    currentESActionValue = skill.actionValue;
                }
                else if (characterSkill.characterCardSkillType == CharacterCardSkillType.ElementalBurst)
                {
                    currentEBActionValue = skill.actionValue;
                }
            }
        }
    }
    public void AddCard(int quantity)
    {
        if(currentQuantitySelected < characterCardData.quantityMax)
        {
            currentQuantitySelected += quantity;
            quantitySelectedText.text = "Selected " + currentQuantitySelected.ToString() + " Card";
            CollectionManager.instance.characterCardDataList.Add(characterCardData);
        }
    }
    public void RecallCard(int quantity)
    {
        if(currentQuantitySelected == characterCardData.quantityMax)
        {
            currentQuantitySelected -= quantity;
            quantitySelectedText.text = "Selected " + currentQuantitySelected.ToString() + " Card";
            CollectionManager.instance.characterCardDataList.Remove(characterCardData);
        }
    }
    public void QuantitySelectedObjState(bool state)
    {
        quantitySelectedObj.SetActive(state);
    }
    public void HideObjects()
    {
        for (int i = 0; i < hiddenObjects.Length; i++)
        {
            hiddenObjects[i].gameObject.SetActive(false);
        }
    }
    public void ShowObjects()
    {
        for (int i = 0; i < hiddenObjects.Length; i++)
        {
            hiddenObjects[i].gameObject.SetActive(true);
        }
    }
    public void SetTakeDamageHighlight(bool state)
    {
        highLightObj.SetActive(state);
        characterStats.isHighlighting = state;
    }
    public void SetTakeWeaknessHighlight(bool state)
    {
        takeWeaknessValueObj.SetActive(state);
    }
    public void BurstPointObjState(bool state)
    {
        burstPointObj.SetActive(state);
    }
    public void SetBurstPoint(int value)
    {
        if (currentBurstPoint - value < 0)
        {
            notificationManager.SetNewNotification("Burst Point are not enough");
            Debug.Log("a");
            return;
        }
        else            
        {
            currentBurstPoint -= value;

            if(currentBurstPoint <= characterCardData.burstPointMax) AudioManager.instance.PlayGetBurstPoint();

            if (currentBurstPoint >= characterCardData.burstPointMax)
            {
                currentBurstPoint = characterCardData.burstPointMax;
            }
            for (int i=0; i<burstPointIconObjects.Length; i++)
            {
                burstPointIconObjects[i].SetActive(i < currentBurstPoint);
            }
        }
    }
    public void SetTakeDamageValue(int value)
    {
        if (characterStats.isShield)
        {
            value -= currentShield;
            if (value <= 0) value = 0;
        }
        if (characterStats.isSkippingRound)
        {
            value = 0;
        }

        if (value < 0)
            takeDamageValueText.text = " + " + value.ToString();
        else
            takeDamageValueText.text = " - " + value.ToString();
    }
    public void SetTakeWeaknessValue(int value)
    {
        if (value < 0)
            takeWeaknessValueText.text = " + " + value.ToString();
        else
            takeWeaknessValueText.text = " - " + value.ToString();
    }

    public void AttackToTarget(Transform target)
    {
        if (placeHolder != null) return;

        Transform parent = transform.parent;

        placeHolder = Instantiate(placeHolderPrefab, parent);
        placeHolder.transform.SetSiblingIndex(transform.GetSiblingIndex());
        placeHolder.transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y);
        transform.SetParent(parent.parent);
        transform.DOMove(target.transform.position, moveDuration).SetEase(Ease.InBack).SetLoops(2, LoopType.Yoyo).
            OnComplete(() =>
            {
                transform.SetParent(parent);
                transform.SetSiblingIndex(placeHolder.transform.GetSiblingIndex());
                Destroy(placeHolder);

                if(gamePlayManager.currentTurn == TurnState.YourTurn)
                    gamePlayManager.playerAttacking = false;
                else if(gamePlayManager.currentTurn == TurnState.EnemyTurn)
                    gamePlayManager.enemyAttacking = false;

            });
        target.transform.DOShakeRotation(shakeDuration, shakeStrength, vibrato, 0, true).SetDelay(moveDuration);
        target.transform.DOShakePosition(shakeDuration, shakeStrength, vibrato, 0, true).SetDelay(moveDuration);
    }
}
