using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class HealingAction : ActionCardSkillBase
{
    public override void DoAction(ActionCardData actionCardData,ActionCardSkill actionCardSkill, List<CharacterCard> targetList)
    {
        int healingValue = actionCardSkill.actionValue; // Assuming the actionCardSkill has a value
        foreach (var target in targetList)
        {
            target.characterStats.Healing(healingValue);
        }
    }
}
