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
    public int Owner;
    public string description;
    public string effectText;
    public int points;
    public bool golden;
    public string type;
    public string faction;
    public Context context;
    public string[] range = new string[3];
    public OnActivation effects;
    public GameObject prefab;
    public Sprite artwork;
    public Sprite posSprite;
    public Sprite aura;
}

