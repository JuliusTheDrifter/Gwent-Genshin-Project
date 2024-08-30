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

    public override List<GameObject> GetCards()
    {
        List<GameObject> cards = new List<GameObject>();
        foreach(Transform transform in hand.transform)
        {
            cards.Add(transform.gameObject);
        }
        return cards;
    }

    public override void Push(GameObject card)
    {
        GameObject auxCard = Instantiate(card);
        auxCard.transform.SetParent(hand.transform);
    }

    public override void SendBottom(GameObject card)
    {
        GameObject auxCard = Instantiate(card);
        auxCard.transform.SetParent(hand.transform);
    }

    public override GameObject Pop()
    {
        if(Cards.Count == 0)
        {
            return null;
        }
        GameObject card = GetCards()[Cards.Count-1];
        Remove(card);
        return card;
    }

    public override void Remove(GameObject card)
    {
        CardDisplay[] cardDisplays = hand.GetComponentsInChildren<CardDisplay>();
        foreach(var cardDisplay in cardDisplays)
        {
            if(cardDisplay.Equals(card.GetComponent<CardDisplay>()))
            {
                Destroy(card.gameObject);
                break;
            }
        }
        Cards.Remove(card);
    }
}
