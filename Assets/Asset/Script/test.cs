﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class test : MonoBehaviour
{
    public Button button;
    public void ChangeScene()
    {
        SceneManager.LoadScene("Start");
    }
}
