using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionCard : MonoBehaviour
{
    public int quantitySelected = 0;
    public int remainingQuantity = 0;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI manaText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI quantity;
    public TextMeshProUGUI quantityInDeckText;
    public TextMeshProUGUI quantitySelectedText;

    public Sprite cardSprite;
    public Image cardImage;

    [SerializeField] private GameObject[] hiddenObjects;
    [SerializeField] private GameObject quantityInDeckObj;
    [SerializeField] private GameObject quantitySelectedObj;
    [SerializeField] private GameObject quantityObj;

    public ActionCardAndQuantity actionCardAndQuantity;
    private void Start()
    {
        if(actionCardAndQuantity.actionCard)
        {
            remainingQuantity = actionCardAndQuantity.quantity;
            quantitySelected = 0;
        }
    }
    public void GetOriginalCardInfo(ActionCardAndQuantity actionCardAndQuantity) //nhận thông tin card ban đầu
    {
        this.actionCardAndQuantity = actionCardAndQuantity;
        nameText.text = actionCardAndQuantity.actionCard.cardName;
        descriptionText.text = actionCardAndQuantity.actionCard.description;
        manaText.text = actionCardAndQuantity.actionCard.mana.ToString();
        cardSprite = actionCardAndQuantity.actionCard.cardSprite;
        cardImage.sprite = cardSprite;
        quantity.text = actionCardAndQuantity.quantity.ToString();
    }
    public void CardRecall(int quantity)
    {
        quantitySelected -= quantity;
        remainingQuantity += quantity;
        quantitySelectedText.text = "Selected " + quantitySelected.ToString() + " Card";
        quantityInDeckText.text = quantitySelected.ToString();
    }
    public void AddCard(int quantity)
    {
        quantitySelected += quantity;
        remainingQuantity -= quantity;
        quantitySelectedText.text = "Selected " + quantitySelected.ToString() + " Card";
        quantityInDeckText.text = quantitySelected.ToString();
    }
    public void HideObjects()
    {
        for(int i=0;i<hiddenObjects.Length;i++)
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
    public void ShowHideQuantityInDeck(bool hidden)
    {
        quantityInDeckObj.SetActive(hidden);
    }
    public void ShowHideQuantitySelected(bool hidden)
    {
        quantitySelectedObj.SetActive(hidden);
    }
    public void ShowHideQuantity(bool hidden)
    {
        quantityObj.SetActive(hidden);
    }

}
