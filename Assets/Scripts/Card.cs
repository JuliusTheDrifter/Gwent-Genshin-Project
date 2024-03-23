using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEngine;
[System.Serializable]
[CreateAssetMenu(fileName="New Card",menuName="Card")]
public class Card: ScriptableObject
{
    public new string name;
    public int id;
    public string position;
    public string description;
    public string effect;
    public int points;
    public Sprite artwork;
    public Sprite type;
    public Sprite aura;

    public Card(string name,int id,string position,string description,string effect,int points,Sprite artwork,Sprite type,Sprite aura)
{
    this.name=name;
    this.id=id;
    this.position=position;
    this.description=description;
    this.effect=effect;
    this.points=points;
    this.artwork=artwork;
    this.type=type;
    this.aura=aura;
}
}
