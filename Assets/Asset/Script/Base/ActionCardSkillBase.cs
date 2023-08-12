using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionCardSkillBase
{
    public abstract void DoAction(ActionCardData actionCardData, ActionCardSkill actionCardSkill, List<CharacterCard> targetList);
    protected GamePlayManager gamePlayManager => GamePlayManager.instance;
}
