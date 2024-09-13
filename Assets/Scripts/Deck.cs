using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using Unity.VisualScripting;
using System;

public class Deck : CardList //This scrip is where the deck is formed
{
    public TextMeshProUGUI textMeshPro;
    public List<GameObject> gameObjects;

    void Awake()
    {
        foreach(var card in gameObjects)
        {
            Cards.Add(card);
        }
        Shuffle();
    }

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

    public override void Shuffle()
    {
        for(int i=0;i<Cards.Count;i++)
        {
            int randomIndex = UnityEngine.Random.Range(0,Cards.Count);
            GameObject temp = Cards[i];
            Cards[i] = Cards[randomIndex];
            Cards[randomIndex] = temp;
        }
    }

    public int GetNumber()
    {
        string aux = textMeshPro.text.ToString();
        int temp = int.Parse(aux);
        return temp;
    }
}
