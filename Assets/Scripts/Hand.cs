using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Unity.VisualScripting;
using System;

public class Hand : MonoBehaviour
{
    public List<Card> Cards = new List<Card>();
    public GameObject hand;

    public List<Card> Find(Func<Card,bool> func)
    {
        return Cards.Where(func).ToList();
    }

    public void Push(Card card)
    {
        Cards.Add(card);
        GameObject auxCard = Instantiate(card.prefab);
        auxCard.transform.SetParent(hand.transform);
    }

    public void SendBottom(Card card)
    {
        Cards.Insert(0,card);
        GameObject auxCard = Instantiate(card.prefab);
        auxCard.transform.SetParent(hand.transform);
    }

    public Card Pop()
    {
        Card card = Cards[Cards.Count-1];
        Remove(card);
        return card;
    }

    public void Remove(Card card)
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

    public void Shuffle()
    {
        Cards = Cards.OrderBy(x => UnityEngine.Random.value).ToList();
    }
}
