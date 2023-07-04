using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCardDisplay : MonoBehaviour
{
    public int maxHealth;
    public int maxSkillPoint;
    public string cardName;
    public string cardDescription;

    public Sprite cardSprite;
    public Image cardImage;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI nameText;
    public void GetImage(Sprite sprite)
    {
        cardSprite = sprite;
        cardImage.sprite = cardSprite;
    }
    public void GetName(string name)
    {
        cardName = name;
        nameText.text = "" + cardName;
    }
    public void GetHealth(int maxHealth)
    {
        this.maxHealth = maxHealth;
        healthText.text = "" + this.maxHealth;
    }
    public void GetSkillPoint(int skillPoint)
    {
        this.maxSkillPoint = skillPoint;
    }
}
