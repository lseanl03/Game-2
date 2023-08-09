using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleCardFieldController : FieldBase
{
    public ScrollRect scrollR;
    public GameObject characterCardList;
    public GameObject actionCardList;
    private void Start()
    {
    }
    public void ChangeContent(CardSelectType cardSelectType)
    {
        if (cardSelectType == CardSelectType.CharacterCard)
        {
            scrollR.content = characterCardList.transform as RectTransform;
        }
        if (cardSelectType == CardSelectType.ActionCard)
        {
            scrollR.content = actionCardList.transform as RectTransform;
        }
        characterCardList.SetActive(cardSelectType == CardSelectType.CharacterCard);
        actionCardList.SetActive(cardSelectType == CardSelectType.ActionCard);
    }
}
