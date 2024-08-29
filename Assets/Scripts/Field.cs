using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Field : CardList
{
    public GameObject Units;

    public void Push(Card card)
    {
        Cards.Add(card);
    }

    public void SendBottom(Card card)
    {
        Cards.Insert(0,card);
    }

    public Card Pop()
    {
        if(Cards.Count == 0)
        {

        }
        Card card = Cards[Cards.Count-1];
        Cards.RemoveAt(Cards.Count-1);
        return card;
    }

    public void Remove(Card card)
    {
        Cards.Remove(card);
    }

    public void Shuffle()
    {
        Cards = Cards.OrderBy(x => UnityEngine.Random.value).ToList();
    }
}
