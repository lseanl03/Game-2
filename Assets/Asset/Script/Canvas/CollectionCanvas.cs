using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CollectionCanvas : CanvasBase
{
    public BattleCardFieldController battleCardFieldController;
    public SelectCardFieldController selectCardFieldController;

    private void OnEnable()
    {
        uiManager.selectCardCanvas.OpenCharacterCardPanel();
    }
    public void ChangeContent(CardSelectType cardSelectType)
    {
        battleCardFieldController.ChangeContent(cardSelectType);
        selectCardFieldController.ChangeContent(cardSelectType);
    }
}
