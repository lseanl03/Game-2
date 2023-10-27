using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusTooltip : MonoBehaviour
{
    public Image statusImage;
    public TextMeshProUGUI statusNameText;
    public GameObject statusDesButtonObj; 

    public bool isShowingDes = false;
    public float rotateIndex = 0f;
    public GameObject statusDesObj;

    public void ChangeDescriptionState()
    {
        isShowingDes = !isShowingDes;
        rotateIndex = isShowingDes ? 180f : 0f;
        statusDesObj.SetActive(isShowingDes);
        statusDesButtonObj.transform.rotation = Quaternion.Euler(0f,0f,rotateIndex);
    }
}
