using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinLosePanel : PanelBase
{
    public TextMeshProUGUI winLoseText;
    public Button returnButton;

    public void OnEnable()
    {
        StartCoroutine(ShowButton());
    }
    public void SetWinLoseText(string text)
    {
        winLoseText.text = text;
    }
    public IEnumerator ShowButton()
    {
        notificationManager.Reset();
        returnButton.gameObject.SetActive(false);
        yield return new WaitForSeconds(2);
        returnButton.gameObject.SetActive(true);
    }
}
