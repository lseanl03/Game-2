using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionCard : MonoBehaviour
{
    public int quantitySelected = 0;
    public int remainingQuantity = 0;

    public ActionCardData actionCardData;
    public TextMeshProUGUI manaText;

    public TextMeshProUGUI quantity;
    public TextMeshProUGUI quantityInDeckText;
    public TextMeshProUGUI quantitySelectedText;

    public Sprite cardSprite;
    public Image cardImage;
    public Image backImage;

    [SerializeField] private GameObject quantityInDeckObj;
    [SerializeField] private GameObject quantitySelectedObj;
    [SerializeField] private GameObject quantityObj;
    [SerializeField] private GameObject[] hiddenObjects;

    private void Start()
    {
        remainingQuantity = actionCardData.quantityMax;
        quantitySelected = 0;
    }
    public void GetOriginalCardInfo(ActionCardData actionCardData) //nhận thông tin card ban đầu
    {
        this.actionCardData = actionCardData;
        manaText.text = actionCardData.cardCost.ToString();
        quantity.text = actionCardData.quantityMax.ToString();
        cardSprite = actionCardData.cardSprite;
        cardImage.sprite = cardSprite;
        backImage.color = actionCardData.actionCard.colorRarity;
    }
    public void RecallCard(int quantity)
    {
        quantitySelected -= quantity;
        remainingQuantity += quantity;
        quantitySelectedText.text = "Selected " + quantitySelected.ToString() + " Card";
        quantityInDeckText.text = quantitySelected.ToString();
        CollectionManager.instance.actionCardDataList.Remove(actionCardData);
    }
    public void AddCard(int quantity)
    {
        quantitySelected += quantity;
        remainingQuantity -= quantity;
        quantitySelectedText.text = "Selected " + quantitySelected.ToString() + " Card";
        quantityInDeckText.text = quantitySelected.ToString();
        CollectionManager.instance.actionCardDataList.Add(actionCardData);
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
