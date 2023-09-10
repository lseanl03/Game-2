using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusTooltip : MonoBehaviour
{
    public Image statusImage; //icon tăng tấn công
    public TextMeshProUGUI statusNameText; //tên actioncard
    public Button statusDesButton; 

    public bool isShowing = false;
    public float rotateIndex = 0f;
    public GameObject statusDesObj;

    public void ChangeDescriptionState()
    {
        isShowing = !isShowing;
        rotateIndex = isShowing ? 180f : 0f;
        statusDesObj.SetActive(isShowing);
        statusDesButton.transform.rotation = Quaternion.Euler(0f,0f,rotateIndex);
    }
}
