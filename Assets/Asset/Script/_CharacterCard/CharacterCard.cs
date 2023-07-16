using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCard : MonoBehaviour
{

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI skillPointText;
    public TextMeshProUGUI quantityText;

    public Sprite cardSprite;
    public Image cardImage;
    [SerializeField] private GameObject[] hiddenObjects;

    public CharacterCardAndQuantity characterCardAndQuantity;
    public void GetOriginalCardInfo(CharacterCardAndQuantity characterCardAndQuantity)
    {
        this.characterCardAndQuantity = characterCardAndQuantity;
        nameText.text = characterCardAndQuantity.characterCard.characterName;
        descriptionText.text = characterCardAndQuantity.characterCard.description;
        healthText.text = characterCardAndQuantity.characterCard.maxHealth.ToString();
        cardSprite = characterCardAndQuantity.characterCard.cardSprite;
        cardImage.sprite = cardSprite;

        quantityText.text = characterCardAndQuantity.quantity.ToString();
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
