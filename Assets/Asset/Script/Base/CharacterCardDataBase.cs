using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CharacterCardDataBase : ScriptableObject
{
    public string characterName;
    public string description;
    public Sprite cardSprite;

    public int maxHealth = 20;
    public int burstPointMax = 3;
    public int quantityMax = 1;

    public CharacterCardBase characterCard;
    //public DestinyType destinyType; //lo?i v?n m?nh
    //public List<SkillType> skillTypeList; //lo?i k? n?ng
}

[Serializable]
public class CharacterCardBase
{
    [Header("Skill")]
    public List<CharacterSkill> characterSkillList;

    [Header("Destiny")]
    public DestinyType destinyType;
    [TextArea] public string descriptionDestiny;

    [Header("CombatType")]
    public CombatType combatType;

    [Header("Weakness")]
    public List<Weakness> weakness;
}

[Serializable]
public class CharacterSkill
{
    public CharacterCardSkillType characterCardSkillType;
    public Sprite skillSprite;
    public int manaCost;
    [TextArea] public string descriptionSkill;
}

[Serializable]

public class CombatType
{
    public WeaknessType combatType;
    public Sprite combatTypeSprite;
}

[Serializable]
public  class Weakness
{
    public WeaknessType weaknessType;
    public Sprite weaknessTypeSprite;
}
public enum CharacterCardSkillType
{
    NormalAttack, //tấn công thường
    ElementalSkill, //kỹ năng nguyên tố
    ElementalBurst, //kỹ năng nộ
}
public enum DestinyType
{
    Nihility, //Hư Vô
    Destruction, //Hủy Diệt
    TheHunt, //Săn Bắt
    Erudition, //Tri Thức
    Harmony, //Hòa Hợp
    Preservation, //Bảo Hộ
    Abundance, //Trù Phú
}
public enum WeaknessType
{
    Lightning, //lôi
    Physical, //vật lý
    Fire, //hỏa
    Ice, //băng
    Wind, //phong
    Quantum, //lượng tử
    Imaginary, //số ảo
}
