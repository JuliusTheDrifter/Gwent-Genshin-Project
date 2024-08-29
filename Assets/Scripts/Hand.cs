using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Unity.VisualScripting;
using System;

public class Hand : CardList
{
    public GameObject hand;

    public new void Push(Card card)
    {
        Cards.Add(card);
        GameObject auxCard = Instantiate(card.prefab);
        auxCard.transform.SetParent(hand.transform);
    }

    public new void SendBottom(Card card)
    {
        Cards.Insert(0,card);
        GameObject auxCard = Instantiate(card.prefab);
        auxCard.transform.SetParent(hand.transform);
    }

    public new Card Pop()
    {
        Card card = Cards[Cards.Count-1];
        Remove(card);
        return card;
    }

    public new void Remove(Card card)
    {
        foreach(Transform transform in hand.transform)
        {
            if(transform.gameObject.GetComponent<Card>()==card)
            {
                Destroy(transform.gameObject);
                break;
            }
        }
        Cards.Remove(card);
    }
}
