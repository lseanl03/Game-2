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
    public void ReturnMainMenu()
    {
        PanelState(false);
        sceneChanger.OpenMainMenuScene();
        gamePlayManager.ClearStatusApplying();
        gamePlayManager.gamePlayCanvas.ResetActionCard();
    }
    public void SetWinLoseText(string text)
    {
        winLoseText.text = text;
    }
    public IEnumerator ShowButton()
    {
        notificationManager.ResetText();
        returnButton.gameObject.SetActive(false);
        yield return new WaitForSeconds(2);
        returnButton.gameObject.SetActive(true);
    }
    public void SetWinState()
    {
        SetWinLoseText("You Win");
        gamePlayManager.UpdateGameState(GamePlayState.Victory);
    }
    public void SetLoseState()
    {
        SetWinLoseText("You Lose");
        gamePlayManager.UpdateGameState(GamePlayState.Lose);
    }
}
