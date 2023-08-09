using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : CharacterBase
{
    public static EnemyManager instance;
    public void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    public override void Start()
    {
        base.Start();
    }
}
