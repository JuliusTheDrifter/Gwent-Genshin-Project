using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using System;

public class Graveyard : CardList
{
    public TextMeshProUGUI textMeshPro;

    public new void Push(Card card)
    {
        Cards.Add(card);
        int aux = GetNumber();
        textMeshPro.text = aux+1.ToString();
    }

    public new void SendBottom(Card card)
    {
        Cards.Insert(0,card);
        int aux = GetNumber();
        textMeshPro.text = aux+1.ToString();
    }

    public new Card Pop()
    {
        Card card = Cards[Cards.Count-1];
        Cards.RemoveAt(Cards.Count-1);
        int aux = GetNumber();
        textMeshPro.text = aux--.ToString();
        return card;
    }

    public new void Remove(Card card)
    {
        int aux = GetNumber();
        textMeshPro.text = aux--.ToString();
        Cards.Remove(card);
    }

    public int GetNumber()
    {
        string aux = textMeshPro.text.ToString();
        int temp = Convert.ToInt32(aux);
        return temp;
    }
}
