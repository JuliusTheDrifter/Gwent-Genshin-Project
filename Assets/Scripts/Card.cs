using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Unity.VisualScripting;
using UnityEngine;
[System.Serializable]
[CreateAssetMenu(fileName="New Card",menuName="Card")] //This allows me to create cards in Unity
public class Card: ScriptableObject //In this script I create the class card as an ScriptableObject
{
    public new string name;
    public int id;

    //card.Owner missing
    public string description;
    public string effectText;
    public int points;
    public bool golden; //Eliminar
    public string type;
    public Effect effect;
    public GameObject prefab; //Ponerlo a cada carta
    public Sprite artwork;
    public Sprite posSprite;
    public Sprite aura;
}

