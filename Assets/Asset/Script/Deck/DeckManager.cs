using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class DeckManager : MonoBehaviour
{
    public static DeckManager instance;

    public int characterCardMaxSize = 3;
    public int actionCardMaxSize = 30;
    public List<CharacterCardAndQuantity> characterCardDeck;
    public List<ActionCardAndQuantity> actionCardDeck;
    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
}
