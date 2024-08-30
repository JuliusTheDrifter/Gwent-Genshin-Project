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

    public override List<GameObject> GetCards()
    {
        return Cards;
    }

    public override void Push(GameObject card)
    {
        Cards.Add(card);
        int aux = GetNumber();
        textMeshPro.text = aux+1.ToString();
    }

    public override void SendBottom(GameObject card)
    {
        Cards.Insert(0,card);
        int aux = GetNumber();
        textMeshPro.text = aux+1.ToString();
    }

    public override GameObject Pop()
    {
        GameObject card = Cards[Cards.Count-1];
        Cards.RemoveAt(Cards.Count-1);
        int aux = GetNumber();
        textMeshPro.text = aux--.ToString();
        return card;
    }

    public override void Remove(GameObject card)
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
