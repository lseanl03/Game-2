using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public UIManager instance;
    private void Awake()
    {
        instance = this;
    }
}
