using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseBurstPointAction : ActionCardSkillBase
{
    public override void DoAction(ActionCardData actionCardData, ActionCardSkill actionCardSkill, List<CharacterCard> targetList, List<Status> statusList)
    {
        int value = actionCardSkill.actionValue;
        foreach(var target in targetList)
        {
            target.characterStats.IncreaseBurstPoint(value);
        }
    }
}
