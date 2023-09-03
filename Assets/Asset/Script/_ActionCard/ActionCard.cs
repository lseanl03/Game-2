using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class ActionCard : CardBase
{
    [Header("Data")]
    public int quantityInDeck;
    public int quantityMaxInDeck;
    public int actionCost;
    [TextArea] public string description;
    public ActionCardData actionCardData;

    [Header("Image")]
    public Sprite cardSprite;
    public Image cardImage;
    public Image backImage;
    public Image cardBack;

    [Header("Text")]
    public TextMeshProUGUI actionCostText;
    public TextMeshProUGUI quantityText;
    public TextMeshProUGUI quantityInDeckText;
    public TextMeshProUGUI quantitySelectedText;


    [Header("GameObject")]
    [SerializeField] private GameObject quantityInDeckObj;
    [SerializeField] private GameObject quantitySelectedObj;
    [SerializeField] private GameObject quantityObj;
    [SerializeField] private GameObject manaObj;

    [Header("Component")]
    private ActionCardDragHover actionCardDragHover;

    private void Start()
    {
        actionCardDragHover = GetComponent<ActionCardDragHover>();
    }
    public void GetCardData(ActionCardData actionCardData)
    {
        this.actionCardData = actionCardData;
        quantityMaxInDeck = actionCardData.quantityMaxInDeck;
        description = actionCardData.cardDescription;

        SetCardImage(actionCardData.cardSprite);
        SetBackImage(actionCardData.actionCard.colorRarity);
        SetActionCost(actionCardData.actionCost);
    }
    public void SetActionCost(int value)
    {
        actionCost = value;
        actionCostText.text = actionCost.ToString();
    }
    public void SetCardImage(Sprite sprite)
    {
        cardSprite = sprite;
        cardImage.sprite = cardSprite;
    }
    public void SetBackImage(Color color)
    {
        backImage.color = color;
    }
    public void RecallCard(int quantity) 
    {
        quantityInDeck -= quantity;
        quantityMaxInDeck += quantity;
        quantitySelectedText.text = "Selected " + quantityInDeck.ToString() + " Card";
        quantityInDeckText.text = quantityInDeck.ToString();
        collectionManager.actionCardDataList.Remove(actionCardData);
    }
    public void AddCard(int quantity)
    {
        quantityInDeck += quantity;
        quantityMaxInDeck -= quantity;
        quantitySelectedText.text = "Selected " + quantityInDeck.ToString() + " Card";
        quantityInDeckText.text = quantityInDeck.ToString();
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
            DoAction(this.actionCardData, actionCardSkill, targetList, actionCardSkill.statusList);
        }
    }
    public void DoAction(ActionCardData actionCardData, ActionCardSkill actionCardSkill, List<CharacterCard> targetList, List<Status> statusList)
    {
        switch (actionCardSkill.actionCardTypeList)
        {
            case ActionCardActionSkillType.Healing:
                HealingAction healingAction = new HealingAction();
                healingAction.DoAction(actionCardData, actionCardSkill, targetList, statusList);
                break;
            case ActionCardActionSkillType.IncreaseAttack:
                IncreaseAttackAction increaseAttackAction = new IncreaseAttackAction();
                increaseAttackAction.DoAction(actionCardData, actionCardSkill, targetList, statusList);
                break;
            case ActionCardActionSkillType.SkillPointRecovery:
                playerManager.RecoverySkillPoint(actionCardSkill.actionValue);
                break;
            case ActionCardActionSkillType.IncreaseBurstPoint:
                IncreaseBurstPointAction increaseBurstPointAction = new IncreaseBurstPointAction();
                increaseBurstPointAction.DoAction(actionCardData, actionCardSkill, targetList, statusList);
                break;
            case ActionCardActionSkillType.CreateShield:
                CreateShiedAction createShiedAction = new CreateShiedAction();
                createShiedAction.DoAction(actionCardData,actionCardSkill, targetList, statusList);
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
                Debug.Log("ChooseAlly");
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
            List<CharacterCard> targetList = DetermineTarget(actionTargetType, playerCharacterList, enemyCharacterList);
            switch (actionCardSkill.actionCardTypeList)
            {
                case ActionCardActionSkillType.Healing:
                    foreach (CharacterCard characterCard in targetList)
                    {
                        if (characterCard.currentHealth >= characterCard.characterCardData.maxHealth ||
                            characterCard.characterStats.isUsingHealing)
                        {
                            actionCardDragHover.ReturnCard();
                            notificationManager.SetNewNotification("Card cannot be used at this time");
                        }
                    }
                    break;
                case ActionCardActionSkillType.IncreaseAttack:
                    foreach (CharacterCard characterCard in targetList)
                    {
                        if (characterCard.characterStats.isIncreasingAttack)
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
