using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "tutorialData", menuName ="TCG/TutorialData")]
public class TutorialData : ScriptableObject
{
    public List<TutorialBase> tutorialList;
}

[Serializable]
public class TutorialBase
{
    public TutorialType tutorialType;
    public List<TutorialText> textList;
}
public enum TutorialType
{
    None,
    SelectTurnInitial,
    SelectActionCardInitial,
    SelectCharacterBattleInitial,
    uiTutorial,
    CharacterTutorial,
    ActionCardTutorial,
    AttackTutorial,
    SwitchCharacterTutorial,
    WeaknessTutorial,
    EndRoundTutorial,
}

[Serializable]
public class TutorialText
{
    public bool isUsed = false;
    public bool arrowTop = false;
    public bool arrowBottom = false;
    [TextArea] public string text;
}
