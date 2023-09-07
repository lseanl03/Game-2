using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipRoundAction : ActionCardSkillBase
{
    public override void DoAction(ActionCardData actionCardData, ActionCardSkill actionCardSkill, List<CharacterCard> targetList, List<Status> statusList)
    {
        foreach(var target in targetList)
        {
            target.characterStats.SkipRound();
            target.characterStats.ApplyStatus(ActionCardActionSkillType.SkipRound);
            foreach (Status status in statusList)
            {
                target.characterStats.statusList.Add(status);
            }
        }
    }
}
