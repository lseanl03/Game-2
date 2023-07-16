using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BackgroundClickBase : MonoBehaviour
{
    public TooltipManager tooltipManager;
    public void Start()
    {
        tooltipManager = FindObjectOfType<TooltipManager>();
    }
}
