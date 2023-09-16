using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionCardSkillBase
{
    public abstract void DoAction(ActionCardSkill actionCardSkill, List<CharacterCard> targetList, List<Status> statusList);
    protected GamePlayManager gamePlayManager => GamePlayManager.instance;
    protected UIManager uiManager => UIManager.instance;
}
