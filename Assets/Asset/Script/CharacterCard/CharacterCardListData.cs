using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "CharacterCardList", menuName = "TCG/CharacterCard")]
public class CharacterCardListData : ScriptableObject
{
    public List<CharacterCardListBase> characterCardList;
}
[Serializable]
public class CharacterCardListBase
{
    public string characterName;
    public int maxHealth;
    public int skillPointMax;
    public Sprite characterCardSprite;

    public DestinyType destinyType;
    public List<SkillType> skillTypeList;
}
