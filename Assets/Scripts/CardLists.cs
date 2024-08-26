using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class CardLists : MonoBehaviour
{
    public List<Card> Cards = new List<Card>();
    public GameObject CardList;

    /*public List<Card> Find(Predicate predicate)
    {
        return Cards.Where().ToList();
    }*/

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
        Cards = Cards.OrderBy(x => Random.value).ToList();
    }

}