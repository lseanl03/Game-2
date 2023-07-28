using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeckManager : DeckManagerBase
{
    public static EnemyDeckManager instance;
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
    public void Start()
    {
        ShuffleList(actionCardDeckData);
    }
}
