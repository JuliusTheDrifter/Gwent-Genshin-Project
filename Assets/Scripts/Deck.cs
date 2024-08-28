using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using Unity.VisualScripting;
using System;

public class Deck : MonoBehaviour //This scrip is where the deck is formed
{
    public TextMeshProUGUI textMeshPro;
    public List<GameObject> gameObjects;
    public List<Card> Cards;

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

    public List<Card> Find(Func<Card,bool> func)
    {
        return Cards.Where(func).ToList();
    }

    public void Push(Card card)
    {
        Cards.Add(card);
        int aux = GetNumber();
        textMeshPro.text = aux+1.ToString();
    }

    public void SendBottom(Card card)
    {
        Cards.Insert(0,card);
        int aux = GetNumber();
        textMeshPro.text = aux+1.ToString();
    }

    public Card Pop()
    {
        Card card = Cards[Cards.Count-1];
        Cards.RemoveAt(Cards.Count-1);
        int aux = GetNumber();
        textMeshPro.text = aux--.ToString();
        return card;
    }

    public void Remove(Card card)
    {
        int aux = GetNumber();
        textMeshPro.text = aux--.ToString();
        Cards.Remove(card);
    }

    public void Shuffle()
    {
        Cards = Cards.OrderBy(x => UnityEngine.Random.value).ToList();
    }

    public int GetNumber()
    {
        string aux = textMeshPro.text.ToString();
        int temp = int.Parse(aux);
        return temp;
    }
}
