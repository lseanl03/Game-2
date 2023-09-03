using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public SelectCardCanvas selectCardCanvas;
    public BattleCanvas battleCanvas;
    protected PlayerManager deckManager => PlayerManager.instance;
    protected CollectionManager collectionManager => CollectionManager.instance;
    protected NotificationManager notificationManager => NotificationManager.instance;
    protected TooltipManager tooltipManager => TooltipManager.instance;
    protected GamePlayManager gamePlayManager => GamePlayManager.instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    public void HideTooltip()
    {
        tooltipManager.tooltipCanvas.CanvasState(false);
        if(tooltipManager.tooltipCanvas.characterCardTooltip != null)
        {
            CharacterCardTooltip characterCardTooltip = tooltipManager.tooltipCanvas.characterCardTooltip;
            if (characterCardTooltip.normalAtk.isShowing)
            {
                characterCardTooltip.normalAtk.DescriptionState();
            }
            if (characterCardTooltip.elementalSkill.isShowing)
            {
                characterCardTooltip.elementalSkill.DescriptionState();
            }
            if (characterCardTooltip.elementalBurst.isShowing)
            {
                characterCardTooltip.elementalBurst.DescriptionState();
            }

            characterCardTooltip.HideStatusDes();
        }
    }
    public void HideSwitchCardBattle()
    {
        battleCanvas.switchCardBattlePanel.PanelState(false);
    }
    public void ShowSkill()
    {
        battleCanvas.skillPanel.PanelState(true);
    }
}
