using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateShiedAction : ActionCardSkillBase
{
    public override void DoAction(ActionCardData actionCardData, ActionCardSkill actionCardSkill, List<CharacterCard> targetList, List<Status> statusList)
    {
        int shieldValue = actionCardSkill.actionValue;

        foreach (var target in targetList)
        {
            target.characterStats.Shield(shieldValue);
            target.characterStats.ApplyStatus(ActionCardActionSkillType.CreateShield);
            foreach (var status in statusList)
            {
                target.characterStats.statusList.Add(status);
            }
        }
    }
}
