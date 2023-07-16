using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CardList : MonoBehaviour
{
    public int actionCardDeckSize;
    public int characterCardDeckSize;

    public List<CharacterCardAndQuantity> characterCardList;
    public List<ActionCardAndQuantity> actionCardList;
}
