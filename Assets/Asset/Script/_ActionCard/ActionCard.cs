using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class ActionCard : CardBase
{
    [Header("Quantity")]
    public int quantitySelected = 0;
    public int remainingQuantity = 0;
    public TextMeshProUGUI quantity;
    public TextMeshProUGUI quantityInDeckText;
    public TextMeshProUGUI quantitySelectedText;

    [Header("Info")]
    public Sprite cardSprite;
    public Image cardImage;
    public Image backImage;
    public Image cardBack;
    public TextMeshProUGUI manaText;


    [Header("GameObject")]
    [SerializeField] private GameObject quantityInDeckObj;
    [SerializeField] private GameObject quantitySelectedObj;
    [SerializeField] private GameObject quantityObj;
    [SerializeField] private GameObject manaObj;

    [Header("Data")]
    public ActionCardData actionCardData;

    [Header("Component")]
    private ActionCardDragHover actionCardDragHover;

    private void Start()
    {
        quantitySelected = 0;
        actionCardDragHover = GetComponent<ActionCardDragHover>();
    }
    public void GetOriginalCardInfo(ActionCardData actionCardData) //nhận thông tin card ban đầu
    {
        this.actionCardData = actionCardData;
        manaText.text = actionCardData.cardCost.ToString();
        quantity.text = actionCardData.quantityMax.ToString();
        cardSprite = actionCardData.cardSprite;
        cardImage.sprite = cardSprite;
        backImage.color = actionCardData.actionCard.colorRarity;
        remainingQuantity = actionCardData.quantityMax;
    }
    public void RecallCard(int quantity) 
    {
        quantitySelected -= quantity;
        remainingQuantity += quantity;
        quantitySelectedText.text = "Selected " + quantitySelected.ToString() + " Card";
        quantityInDeckText.text = quantitySelected.ToString();
        collectionManager.actionCardDataList.Remove(actionCardData);
    }
    public void AddCard(int quantity)
    {
        quantitySelected += quantity;
        remainingQuantity -= quantity;
        quantitySelectedText.text = "Selected " + quantitySelected.ToString() + " Card";
        quantityInDeckText.text = quantitySelected.ToString();
        collectionManager.actionCardDataList.Add(actionCardData);
    }
    public void ManaState(bool state)
    {
        manaObj.gameObject.SetActive(state);
    }
    public void QuantityInDeckState(bool state)
    {
        quantityInDeckObj.SetActive(state);
    }
    public void QuantitySelectedState(bool state)
    {
        quantitySelectedObj.SetActive(state);
    }
    public void QuantityState(bool state)
    {
        quantityObj.SetActive(state);
    }
    public void CardBackState(bool state)
    {
        cardBack.gameObject.SetActive(state);
    }
    public void CardState(bool state)
    {
        gameObject.SetActive(state);
    }
    public void PlayCard()
    {
        List<CharacterCard> playerCharacterList = gamePlayManager.playerCharacterList;
        List<CharacterCard> enemyCharacterList = gamePlayManager.enemyCharacterList;
        for (int i = 0; i < actionCardData.actionCard.actionSkillList.Count; i++)
        {
            Debug.Log("play card");
            ActionCardSkill actionCardSkill = actionCardData.actionCard.actionSkillList[i];
            ActionTargetType actionTargetType = actionCardSkill.actionTargetType;
            List<CharacterCard> targetList = DetermineTarget(actionTargetType, playerCharacterList, enemyCharacterList);
            DoAction(this.actionCardData, actionCardSkill, targetList);
        }
    }
    public void DoAction(ActionCardData actionCardData, ActionCardSkill actionCardSkill, List<CharacterCard> targetList)
    {
        switch (actionCardSkill.actionCardTypeList)
        {
            case ActionCardActionSkillType.Healing:
                Debug.Log("Do Action Healing");
                HealingAction healingAction = new HealingAction();
                healingAction.DoAction(actionCardData, actionCardSkill, targetList);
                break;
        }
    }
    public List<CharacterCard> DetermineTarget(ActionTargetType actionTargetType, List<CharacterCard> playerCharacterList, List<CharacterCard> enemyCharacterList)
    {
        List<CharacterCard> targetList = new List<CharacterCard>();
        switch (actionTargetType)
        {
            case ActionTargetType.Ally:
                foreach (CharacterCard characterCard in playerCharacterList)
                {
                    if (characterCard.characterStats.isActionCharacter)
                        targetList.Add(characterCard);
                }
                break;

            case ActionTargetType.Enemy:
                foreach(CharacterCard characterCard in enemyCharacterList)
                {
                    if (characterCard.characterStats.isActionCharacter)
                        targetList.Add(characterCard);
                }
                break;

            case ActionTargetType.AllAllies:
                foreach (CharacterCard characterCard in playerCharacterList)
                {
                    targetList.Add(characterCard);
                }
                break;

            case ActionTargetType.AllEnemies: 
                foreach (CharacterCard characterCard in enemyCharacterList)
                {
                    targetList.Add(characterCard);
                }
                break;

            case ActionTargetType.ChooseAlly:
                foreach (CharacterCard characterCard in playerCharacterList)
                {
                    if (characterCard.characterStats.isChoosing)
                    {
                        targetList.Add(characterCard);
                    }
                }
                break;

            case ActionTargetType.ChooseEnemy:
                foreach (CharacterCard characterCard in enemyCharacterList)
                {
                    if (characterCard.characterStats.isChoosing)
                    {
                        targetList.Add(characterCard);
                    }
                }
                break;
        }
        return targetList;
    }
    public void CheckTarget()
    {
        actionCardDragHover.canPlayCard = true;
        List<CharacterCard> playerCharacterList = gamePlayManager.playerCharacterList;
        List<CharacterCard> enemyCharacterList = gamePlayManager.enemyCharacterList;
        for (int i = 0; i < actionCardData.actionCard.actionSkillList.Count; i++)
        {
            ActionCardSkill actionCardSkill = actionCardData.actionCard.actionSkillList[i];
            ActionTargetType actionTargetType = actionCardSkill.actionTargetType;
            ActionPhase actionPhase = actionCardSkill.actionPhase;
            ActionLimit actionLimit = actionCardSkill.actionLimit;
            List<CharacterCard> targetList = DetermineTarget(actionTargetType, playerCharacterList, enemyCharacterList);
            switch (actionCardSkill.actionCardTypeList)
            {
                case ActionCardActionSkillType.Healing:
                    foreach (CharacterCard characterCard in targetList)
                    {
                        if (characterCard.characterStats.currentHealth >= characterCard.characterStats.maxHealth)
                        {
                            actionCardDragHover.ReturnCard();
                            notificationManager.SetNewNotification("Card cannot be used at this time");
                        }
                    }
                    break;
            }
        }
    }
}
