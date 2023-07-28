using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipController : MonoBehaviour
{

    public void StateObj(bool state)
    {
        gameObject.SetActive(state);
    }
}
