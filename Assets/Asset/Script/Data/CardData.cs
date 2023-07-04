using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Card", menuName = "TCG/Card")]
public class CardData : ScriptableObject
{
    public int id;
    public string cardName;
    public int mana;
    public Sprite cardSprite;
    [TextArea] public string description;
}
