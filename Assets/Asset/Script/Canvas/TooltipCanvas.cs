using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TooltipCanvas : MonoBehaviour
{
    public Image cardImage;

    public TextMeshProUGUI headerText;
    public TextMeshProUGUI contentText;

    private void Start()
    {
    }
    public void SetText(string headerText = null, string contentText = null)
    {
        if(string.IsNullOrEmpty(headerText) && string.IsNullOrEmpty(contentText))
        {
            this.headerText.gameObject.SetActive(false);
            this.contentText.gameObject.SetActive(false);
        }
        else
        {
            this.headerText.gameObject.SetActive(true);
            this.contentText.gameObject.SetActive(true);
            this.headerText.text = headerText;
            this.contentText.text = contentText;
        }
    }
    public void SetCardImage(Image cardImage = null)
    {
        if(cardImage == null)
        {
            cardImage.gameObject.SetActive(false);
        }
        else
        {
            cardImage.gameObject.SetActive(true);
            this.cardImage = cardImage;
        }
    }
}
