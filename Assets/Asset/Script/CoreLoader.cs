using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CoreLoader : MonoBehaviour
{
    private void Awake()
    {
        try
        {
            if(GameManager.instance == null)
            {
                SceneManager.LoadScene("Core", LoadSceneMode.Additive);
            }
            Destroy(gameObject);
        }
        catch
        {
            Debug.LogError("You need add Core scene to build settings");
            throw;
        }
    }
}
