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
            Cards.Add(card.GetComponent<CardDisplay>().card);
        }
    }
    void Start()
    {
        textMeshPro.text = gameObjects.Count.ToString(); 
        Shuffle();  
    }

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
        int temp = int.Parse(aux);
        return temp;
    }
}
