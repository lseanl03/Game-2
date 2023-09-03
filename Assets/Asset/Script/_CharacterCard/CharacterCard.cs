using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCard : MonoBehaviour
{
    [Header("Info")]
    public CharacterCardData characterCardData;
    public int currentHealth;
    public int currentShield;
    public int currentBurstPoint;
    public int currentIncreaseAttack;
    public int currentQuantitySelected;
    public Sprite cardSprite;
    public Image cardImage;

    [Header("Text")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI quantityText;
    public TextMeshProUGUI quantitySelectedText;
    public TextMeshProUGUI valueReceivedText;
    public TextMeshProUGUI shieldText;
    [Header("GameObject")]
    [SerializeField] private GameObject burstPointObj;
    [SerializeField] private GameObject highLightObj;
    [SerializeField] private GameObject[] hiddenObjects;
    [SerializeField] private GameObject[] burstPointIconObjects;

    [Header("Component")]
    public CharacterStats characterStats;
    public CharacterCardDragHover characterCardDragHover;
    protected NotificationManager notificationManager => NotificationManager.instance;
    public void Awake()
    {
        characterStats = GetComponent<CharacterStats>();
        characterCardDragHover = GetComponent<CharacterCardDragHover>();
    }
    public void Start()
    {
        if(highLightObj != null) SetHighlight(false);
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
        SetDescriptionText(characterCardData.description);
        SetCardImage(characterCardData.cardSprite);
        SetHealthText();
    }
    public void SetDescriptionText(string text)
    {
        descriptionText.text = text;
    }
    public void SetShieldText()
    {
        shieldText.text = currentShield.ToString();
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
    public void SetHighlight(bool state)
    {
        highLightObj.SetActive(state);
        characterStats.isHighlighting = state;
    }
    public void BurstPointObjState(bool state)
    {
        burstPointObj.SetActive(state);
    }
    public void SetValueReceived(int value)
    {
        if(value <= 0)
        valueReceivedText.text =" + " + value.ToString();
        else
            valueReceivedText.text = " - " + value.ToString();
    }
    public void SetQuantitySelectedText()
    {
        quantitySelectedText.text = currentQuantitySelected.ToString();
    }
    public void SetBurstPoint(int value)
    {
        if (currentBurstPoint - value < 0)
        {
            notificationManager.SetNewNotification("Burst Point are not enough");
            return;
        }
        else
        {
            currentBurstPoint -= value;
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
}
