using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public CharacterCard characterCard;

    public bool isChoosing;
    public bool isActionCharacter;
    public bool isDead;

    public int currentHealth;
    public int maxHealth;
    public void Awake()
    {
        characterCard = GetComponent<CharacterCard>();
    }
    public void Healing(int value)
    {
        Debug.Log("Healing");
        if (currentHealth + value > maxHealth)
            currentHealth = maxHealth;
        else
            currentHealth += value;
        SetHealthText();
    }
    public void TakeDamage(int value)
    {
        Debug.Log("Take Damage");  
        if (isDead)
        {
            DeadState(); 
            return;
        }
        currentHealth -= value;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
        }
        SetHealthText();
    }
    public void Damage(int value)
    {
        Debug.Log("Damage");
        if (isDead)
        {
            DeadState(); 
            return;
        }
        currentHealth -= value;
        if(currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
        }
    }
    public void SetHealthText()
    {
        characterCard.healthText.text = currentHealth.ToString();
    }
    public void DeadState()
    {

    }
}
