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
    [TextArea] public string descriptionSkill;
    public int actionPointCost;
    public int skillPointCost;
    public int burstPointCost;

    public List<Skill> actionSkillList;
}

[Serializable]
public class Skill
{
    public CharacterCardActionSkillType skillActionType;
    public ActionTargetType actionTargetType;
    public int actionValue;
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
    None,
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
public enum CharacterCardActionSkillType
{
    None,
    Attack,
    Healing, //hồi máu
    WeaknessRecovery, //hồi điểm yếu
    SkillPointRecovery, //hồi điểm
    IncreaseAttack, //tăng tấn công
    CreateArmor, //Tạo giáp
    Revival, //hồi sinh
    ReduceHealth, //giảm máu
    SkipRound, //bỏ qua lượt
    DamageFree, //miễn sát thương
    DoubleDamage, //nhân đôi sát thương
    IncreaseElementalBurstPoint, //tăng điểm kỹ năng nộ
}
