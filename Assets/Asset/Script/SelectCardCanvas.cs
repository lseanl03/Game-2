using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectCardCanvas : SelectCardBase
{
    public Button notificationButton;
    public Button characterCardButton;
    public Button actionCardButton;
    public TextMeshProUGUI quantityCharacterCardText;
    public TextMeshProUGUI quantityActionCardText;
    private void Start()
    {
        OpenCharacterCardPanel();
    }
    private void Update()
    {

    }
    public void OpenCharacterCardPanel()
    {
        collectionManager.collectionCanvas.ChangeContent(CardSelectType.CharacterCard);
        characterCardButton.GetComponent<Image>().enabled = true;
        actionCardButton.GetComponent<Image>().enabled = false;
    }
    public void OpenActionCardPanel()
    {
        collectionManager.collectionCanvas.ChangeContent(CardSelectType.ActionCard);
        actionCardButton.GetComponent<Image>().enabled = true;
        characterCardButton.GetComponent<Image>().enabled = false;
    }
    public void QuantityCharacterCard(int available ,int total)
    {
        quantityCharacterCardText.text = available + " / " + total;
    }
    public void QuantityActionCard(int available, int total)
    {
        quantityActionCardText.text = available + " / " + total;
    }
    public void Confirm()
    {

    }
}
