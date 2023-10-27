using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTooltip : MonoBehaviour
{
    public bool isShowingDes = false;
    public float rotateIndex = 0f;
    public GameObject skillDesButtonObj;
    public GameObject skillDesObj;

    public void ChangeDescriptionState()
    {
        isShowingDes = !isShowingDes;
        rotateIndex = isShowingDes ? 180f : 0f;
        skillDesObj.SetActive(isShowingDes);
        skillDesButtonObj.transform.rotation = Quaternion.Euler(0f,0f,rotateIndex);
    }
}
