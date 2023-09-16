using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevivalAction : ActionCardSkillBase
{
    public override void DoAction(ActionCardSkill actionCardSkill, List<CharacterCard> targetList, List<Status> statusList)
    {
        int healthValueRevival = actionCardSkill.actionValue;
        foreach (var target in targetList)
        {
            target.characterStats.Revival(healthValueRevival);
            target.characterStats.ApplyStatus(ActionCardActionSkillType.Revival);
            foreach (Status status in statusList)
            {
                target.characterStats.statusList.Add(status);
            }
        }
    }
}
