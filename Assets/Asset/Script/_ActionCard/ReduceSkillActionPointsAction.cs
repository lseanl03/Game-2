using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReduceSkillActionPointsAction : ActionCardSkillBase
{
    public override void DoAction(ActionCardData actionCardData, ActionCardSkill actionCardSkill, List<CharacterCard> targetList, List<Status> statusList)
    {
        int reducedValue = actionCardSkill.actionValue;
        foreach (var target in targetList)
        {
            target.characterStats.ReduceSkillActionPoints(reducedValue);
        }
    }
}
