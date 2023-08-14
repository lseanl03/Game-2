using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{

    [Header("Stat")]
    public bool isChoosing;
    public bool isActionCharacter;
    public bool isDead;
    public bool isHighlighting;
    [Header("Take Damage")]
    public GameObject takeDamageObj;
    public TextMeshProUGUI takeDamageText;

    [Header("Health")]
    public int currentHealth;
    public int maxHealth;

    [Header("Component")]
    public CharacterCard characterCard;
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
        StartCoroutine(TakeDamageState(value));
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
    IEnumerator TakeDamageState(int value)
    {
        if(TakeDamageState(value) != null)
            StopCoroutine(TakeDamageState(value));

        if (value > 0)
            takeDamageText.text = "-" + value.ToString();
        else
            takeDamageText.text = "+" + value.ToString();
        takeDamageObj.SetActive(true);
        yield return new WaitForSeconds(2f);
        takeDamageObj.SetActive(false);
    }
}
