using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseAttackAction : ActionCardSkillBase
{
    public override void DoAction(ActionCardSkill actionCardSkill, List<CharacterCard> targetList, List<Status> statusList)
    {
        int attackIncreaseValue = actionCardSkill.actionValue;

        foreach (var target in targetList)
        {
            target.characterStats.IncreaseAttack(attackIncreaseValue);
            target.characterStats.ApplyStatus(ActionCardActionSkillType.IncreaseAttack);
            foreach (Status status in statusList)
            {
                target.characterStats.statusList.Add(status);
            }
        }
    }
}
