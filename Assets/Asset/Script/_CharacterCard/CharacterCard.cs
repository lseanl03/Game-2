using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCard : MonoBehaviour
{
    [Header("Quantity Card")]
    public int quantitySelected = 0;
    public int remainingQuantity = 1;

    [Header("Burst Point")]
    public int burstPointMax;
    public int currentBurstPoint = 0;

    [Header("Info")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI quantityText;
    public TextMeshProUGUI quantitySelectedText;
    public TextMeshProUGUI valueReceivedText;
    public Sprite cardSprite;
    public Image cardImage;

    [Header("GameObject")]
    [SerializeField] private GameObject burstPointObj;
    [SerializeField] private GameObject highLightObj;
    [SerializeField] private GameObject[] hiddenObjects;
    [SerializeField] private GameObject[] burstPointIconObjects;

    [Header("Component")]
    public CharacterCardData characterCardData;
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
        remainingQuantity = characterCardData.quantityMax;
        burstPointMax = characterCardData.burstPointMax;
        if(highLightObj != null) SetHighlight(false);
        if(burstPointObj != null) BurstPointObjState(true);
    }
    public void GetOriginalCardInfo(CharacterCardData characterCardData)
    {
        this.characterCardData = characterCardData;

        nameText.text = characterCardData.characterName;
        descriptionText.text = characterCardData.description;
        cardSprite = characterCardData.cardSprite;
        cardImage.sprite = cardSprite;
        quantityText.text = characterCardData.quantityMax.ToString();
        if (characterStats != null)
        {
            characterStats.maxHealth = characterCardData.maxHealth;
            characterStats.currentHealth = characterStats.maxHealth;
            characterStats.SetHealthText();
        }
    }
    public void AddCard(int quantity)
    {
        quantitySelected += quantity;
        remainingQuantity -= quantity;
        quantitySelectedText.text = "Selected " + quantitySelected.ToString() + " Card";
        CollectionManager.instance.characterCardDataList.Add(characterCardData);
    }
    public void RecallCard(int quantity)
    {
        quantitySelected -= quantity;
        remainingQuantity += quantity;
        quantitySelectedText.text = "Selected " + quantitySelected.ToString() + " Card";
        CollectionManager.instance.characterCardDataList.Remove(characterCardData);
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
    public void BurstPointConsumption(int value)
    {
        if (currentBurstPoint - value < 0)
        {
            notificationManager.SetNewNotification("Burst Point are not enough");
            return;
        }
        else
        {
            currentBurstPoint -= value;
            for (int i=0; i<currentBurstPoint; i++)
            {
                burstPointIconObjects[i].SetActive(true);
            }
        }
    }
}
