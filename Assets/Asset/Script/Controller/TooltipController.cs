using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TooltipController : MonoBehaviour
{
    [Header("CardName")]
    public TextMeshProUGUI cardNameText;

    [Header("CombatType")]
    public Image combatTypeImage;

    [Header("Weakness")]
    public Image weaknessImage1;
    public Image weaknessImage2;
    public Image weaknessImage3;

    [Header("NormalAttack")]
    public Image normalAttackImage;
    public TextMeshProUGUI normalAttackCostText;
    public TextMeshProUGUI normalAttackNameText;
    public Button normalAttackDesButton;
    public TextMeshProUGUI normalAttackDesText;

    [Header("ElementalSkill")]
    public Image elementalSkillImage;
    public TextMeshProUGUI elementalSkillCostText;
    public TextMeshProUGUI elementalSkillNameText;
    public Button elementalSkillDesButton;
    public TextMeshProUGUI elementalSkillDesText;

    [Header("ElementalBurst")]
    public Image elementalBurstImage;
    public TextMeshProUGUI elementalBurstCostText;
    public TextMeshProUGUI elementalBurstNameText;
    public Button elementalBurstDesButton;
    public TextMeshProUGUI elementalBurstDesText;


    public void StateObj(bool state)
    {
        gameObject.SetActive(state);
    }
}
