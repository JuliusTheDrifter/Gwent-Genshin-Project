using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using System;
public class Lists : MonoBehaviour
{
    public List<Card> Cards{get;protected set;}
    public Lists(List<Card>cards)
    {
        Cards = new List<Card>(cards);
    }
    public List<Card> Find(Predicate<Card> predicate)
    {

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
        Card aux = Cards[Cards.Count-1];
        Cards.RemoveAt(Cards.Count-1);
        return aux;
    }
    public void Remove(Card card)
    {
        Cards.Remove(card);
    }
    public void Shuffle()
    {
        int n = Cards.Count;
        while (n > 0)
        {
            n--;
            int k = Random.Range(0, n + 1);
            Card aux = Cards[k];
            Cards[k] = Cards[n];
            Cards[n] = aux;
        }
    }
}
public class Deck : Lists
{
    public Deck(List<Card> cards) : base(cards){}
    /*public List<Card> GetCards()
    {
        return cards;
    }*/
}
public class Graveyard : Lists
{
    public Graveyard(List<Card> cards) : base(cards){}
}
public class Hand : Lists
{
    public Hand(List<Card> cards) : base(cards){}
}
public class Field : Lists
{
    public Field(List<Card> cards) : base(cards){}
}