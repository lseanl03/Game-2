using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldBase : MonoBehaviour
{
    protected CollectionManager collectionManager => CollectionManager.instance;
    protected UIManager uIManager => UIManager.instance;
    protected GameManager gameManager => GameManager.instance;

}
