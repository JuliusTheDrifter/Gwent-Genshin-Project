using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Unity.VisualScripting;

public class Field : CardList
{
    public GameObject Units;
    public GameObject Inspires;

    public override List<GameObject> GetCards()
    {
        List<GameObject> cards = new List<GameObject>();
        foreach(Transform transform in Units.transform)
        {
            foreach(Transform trans in transform)
            {
                cards.Add(trans.gameObject);
            }
        }
        foreach(Transform transform in Inspires.transform)
        {
            foreach(Transform trans in transform)
            {
                cards.Add(trans.gameObject);
            }
        }
        return cards;
    }

    public override void Push(GameObject card)
    {
        Cards.Add(card);
    }

    public override void SendBottom(GameObject card)
    {
        Cards.Insert(0,card);
    }

    public override GameObject Pop()
    {
        if(Cards.Count == 0)
        {
            return null;
        }
        else
        {
            GameObject card = Cards[Cards.Count-1];
            Cards.RemoveAt(Cards.Count-1);
            DeleteCard(card);
            return card;
        }
    }

    public override void Remove(GameObject card)
    {
        DeleteCard(card);
        Cards.Remove(card);
    }

    void DeleteCard(GameObject card)
    {
        foreach(Transform transform in Units.transform)
        {
            foreach(Transform trans in transform)
            {
                if(trans.gameObject == card)
                {
                    Destroy(trans.gameObject);
                    break;
                }
            }
        }
        foreach(Transform transform in Inspires.transform)
        {
            foreach(Transform trans in transform)
            {
                if(trans.gameObject == card)
                {
                    Destroy(trans.gameObject);
                    break;
                }
            }
        }
    }
}
