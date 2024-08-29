using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public abstract class CardList : MonoBehaviour
{
    public List<Card> Cards = new List<Card>();

    public List<Card> Find(Func<Card,bool> func)
    {
        return Cards.Where(func).ToList();
    }

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
