using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCard : MonoBehaviour
{
    public int quantitySelected = 0;
    public int remainingQuantity = 1;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI skillPointText;
    public TextMeshProUGUI quantityText;
    public TextMeshProUGUI quantitySelectedText;

    public Sprite cardSprite;
    public Image cardImage;
    [SerializeField] private GameObject[] hiddenObjects;

    public CharacterCardData characterCardData;
    public void Start()
    {
        remainingQuantity = characterCardData.quantityMax;
    }
    public void GetOriginalCardInfo(CharacterCardData characterCardData)
    {
        this.characterCardData = characterCardData;
        nameText.text = characterCardData.characterName;
        descriptionText.text = characterCardData.description;
        healthText.text = characterCardData.maxHealth.ToString();
        cardSprite = characterCardData.cardSprite;
        cardImage.sprite = cardSprite;

        quantityText.text = characterCardData.quantityMax.ToString();
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
}
