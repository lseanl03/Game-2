using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="WeaknessBreakingState", menuName ="TCG/WeaknessBreakingState")]
public class WeaknessStateData : ScriptableObject
{
    public List<WeaknessBreaking> weaknessBreakingList;
}

[Serializable]
public class WeaknessBreaking {
    public WeaknessType weaknessType;
    public string weaknessBreakingName;
    public Sprite weaknessBreakingSprite;
    [TextArea] public string weaknessBreakingDescription;
}
