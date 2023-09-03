using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CollectionCanvas : CanvasBase
{
    public BattleCardFieldController battleCardFieldController;
    public SelectCardFieldController selectCardFieldController;
    public void Start()
    {

    }
    public void ChangeContent(CardSelectType cardSelectType)
    {
        battleCardFieldController.ChangeContent(cardSelectType);
        selectCardFieldController.ChangeContent(cardSelectType);
    }
}
