using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class ActionCardDataBase : ScriptableObject
{
    public string cardName;
    public int actionCost;
    public int quantityMaxInDeck = 2;
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
    [Header("Action")]
    public ActionCardActionSkillType actionCardTypeList;
    public ActionTargetType actionTargetType;
    public int actionValue;

    [Header("Status")]
    public List<Status> statusList;
}
[Serializable]
public class Status
{
    public Sprite statusSprite;
    public string statusName;
    [TextArea] public string statusDescription;
    public ActionEndPhase actionEndPhase;
}
public enum ActionCardActionSkillType
{
    None,
    Healing, //hồi máu
    WeaknessRecovery, //hồi điểm yếu
    SkillPointRecovery, //hồi điểm kĩ năng
    IncreaseAttack, //tăng tấn công
    IncreaseBurstPoint, //tăng điểm nộ
    CreateShield, //Tạo khiên
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
    ChooseDeadAlly,
    ChooseDeadEnemy,
}
public enum RarityType
{
    None,
    Low,
    Normal,
    High,
    VeryHigh,
}
public enum ActionStartPhase
{
    None,
    Now, //hành động ngay
    StartRound, //sau khi bắt đầu hiệp
    EndRound, //sau khi kết thúc hiệp
}
public enum ActionEndPhase
{
    None,
    StartRound,
    EndRound,
    Infinite,
    OneActionOrEndRound,
}