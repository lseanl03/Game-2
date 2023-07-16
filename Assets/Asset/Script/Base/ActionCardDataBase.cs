using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ActionCardAndQuantity
{
    public int quantity;
    public ActionCardDataBase actionCard;
}

public class ActionCardDataBase : ScriptableObject
{
    public int id;
    public string cardName;
    [TextArea] public string description;
    public int mana = 10;
    public Sprite cardSprite;
}
