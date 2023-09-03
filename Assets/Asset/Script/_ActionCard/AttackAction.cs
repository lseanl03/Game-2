using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : ActionCardSkillBase
{
    public override void DoAction(ActionCardData actionCardData, ActionCardSkill actionCardSkill, List<CharacterCard> targetList, List<Status> statusList)
    {
        int damageValue = actionCardSkill.actionValue; // Assuming the actionCardSkill has a value
        foreach (var target in targetList)
        {
            target.characterStats.Damage(damageValue);
        }
    }
}
