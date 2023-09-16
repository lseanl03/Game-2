using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleDamageAction : ActionCardSkillBase
{
    public override void DoAction(ActionCardSkill actionCardSkill, List<CharacterCard> targetList, List<Status> statusList)
    {
        foreach(var target in targetList)
        {
            target.characterStats.DoubleDamage();
            target.characterStats.ApplyStatus(ActionCardActionSkillType.DoubleDamage);
            foreach (Status status in statusList)
            {
                target.characterStats.statusList.Add(status);
            }
        }
    }
}
