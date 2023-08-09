using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTooltip : MonoBehaviour
{
    public bool isShowing = false;
    public float rotateIndex = 0f;
    public GameObject skillDesButtonObj;
    public GameObject skillDesObj;

    public void DescriptionState()
    {
        isShowing = !isShowing;
        rotateIndex = isShowing ? 180f : 0f;
        skillDesObj.SetActive(isShowing);
        skillDesButtonObj.transform.rotation = Quaternion.Euler(0f,0f,rotateIndex);
    }
}
