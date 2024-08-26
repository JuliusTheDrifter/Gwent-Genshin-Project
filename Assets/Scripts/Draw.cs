using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.XR;

public class Draw : MonoBehaviour //This script is to draw cards
{
    public Hand Hand1;
    public Hand Hand2;
    public Deck deck1;
    public Deck deck2;
    public void DrawCard(int player) //Draw a random card from the deck and removes it from the deck list
    {
        if(player == 1)
        {
            Hand1.Push(deck1.Pop());
        }
        else
        {
            Hand2.Push(deck2.Pop());
        }
    }
}