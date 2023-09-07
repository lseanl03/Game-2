using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class HealingAction : ActionCardSkillBase
{
    public override void DoAction(ActionCardData actionCardData,ActionCardSkill actionCardSkill, List<CharacterCard> targetList, List<Status> statusList)
    {
        int healingValue = actionCardSkill.actionValue;
        foreach (var target in targetList)
        {
            target.characterStats.Healing(healingValue);
            target.characterStats.ApplyStatus(ActionCardActionSkillType.Healing);
            foreach (Status status in statusList)
            {
                target.characterStats.statusList.Add(status);
            }
        }
    }
}
