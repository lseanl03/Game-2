using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct CharacterCardAndQuantity
{
    public int quantity;
    public CharacterCardDataBase characterCard;
}
public class CharacterCardDataBase : ScriptableObject
{
    public string characterName;
    public string description;
    public int maxHealth = 20;
    public int skillPointMax = 3;
    public Sprite cardSprite;

    //public DestinyType destinyType; //lo?i v?n m?nh
    //public List<SkillType> skillTypeList; //lo?i k? n?ng
}