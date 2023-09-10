using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleCanvas : CanvasBase
{
    public SkillPanel skillPanel;
    public InformationPanel informationPanel;
    public SelectTurnPanel selectTurnPanel;
    public SelectInitialActionCardPanel selectInitialActionCardPanel;
    public SwitchCharacterBattlePanel switchCardBattlePanel;
    public PlayCardPanel playCardPanel;
    public WinLosePanel winLosePanel;

    public void ReturnMainMenu()
    {
        winLosePanel.PanelState(false);
        sceneChanger.OpenMainMenuScene();
    }
}
