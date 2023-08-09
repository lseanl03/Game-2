using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class ActionCardDataBase : ScriptableObject
{
    public string cardName;
    public int cardCost;
    public int quantityMax = 2;
    [TextArea] public string cardDescription;
    public Sprite cardSprite;
    public ActionCardBase actionCard;
}

[Serializable]
public class ActionCardBase
{
    [Header("Skill")]
    public List<ActionCardSkill> actionSkillList;

    [Header("Rarity")]
    public RarityType rarityType;
    public Color colorRarity;
}

[Serializable]
public class ActionCardSkill
{
    public ActionCardActionSkillType actionCardTypeList;
    public ActionTargetType actionTargetType;
    public int actionValue;
    public ActionPhase actionPhase;
    public ActionLimit actionLimit;
}
public enum ActionCardActionSkillType
{
    None,
    Healing, //hồi máu
    WeaknessRecovery, //hồi điểm yếu
    SkillPointRecovery, //hồi điểm
    IncreaseAttack, //tăng tấn công
    CreateArmor, //Tạo giáp
    Weapon, //Vũ khí
    Artifact, //Thánh di vật
    Revival, //hồi sinh
    ReduceHealth, //giảm máu
    SkipRound, //bỏ qua lượt
    DamageFree, //miễn sát thương
    DoubleDamage, //nhân đôi sát thương
    IncreaseElementalBurstPoint, //tăng điểm kỹ năng nộ
}
public enum ActionTargetType
{
    None,
    Enemy,
    Ally,
    AllEnemies,
    AllAllies,
    ChooseAlly,
    ChooseEnemy,
}
public enum ActionPhase
{
    None,
    Now, //hành động ngay
    EndRound, //sau khi kết thúc hiệp
    StartRound, //sau khi bắt đầu hiệp
}
public enum RarityType
{
    None,
    Low,
    Normal,
    High,
    VeryHigh,
}
public enum ActionLimit
{
    None,
    OneAction,
    TwoAction,
    ThreeAction,
    OneRound,
    TwoRound,
    ThreeRound,
    Infinite
}