using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionalToolCharacterCardDescription : MonoBehaviour
{
    public TextMeshProUGUI cardNameText;
    public Image combatTypeImage;

    [Header("Normal Attack")]
    public Image nAImage;
    public TextMeshProUGUI nANameText;
    public TextMeshProUGUI nADesText;
    public TextMeshProUGUI nAActionPointCostText;

    [Header("Elemental Skill")]
    public Image eSImage;
    public TextMeshProUGUI eSNameText;
    public TextMeshProUGUI eSDesText;
    public TextMeshProUGUI eSActionPointCostText;

    [Header("Elemental Burst")]
    public Image eBImage;
    public TextMeshProUGUI eBNameText;
    public TextMeshProUGUI eBDesText;
    public TextMeshProUGUI eBActionPointCostText;
    public TextMeshProUGUI eBBurstPointCostText;

    [Header("Weakness")]
    public Image weaknessImage1;
    public Image weaknessImage2;
    public Image weaknessImage3;
}
