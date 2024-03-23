using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class Deck : MonoBehaviour
{
        public List<CardDisplay> deck = new List<CardDisplay>();
        //lista con 
                
        public Transform[] cardSlots;
        public bool[] availableCardSlots;

        public TMP_Text deckSizeText;

        /*public void DrawCard()
        {
            if(deck.Count >= 1)
            {
                Card rdmCard = deck[Random.Range(0,deck.Count)];

                for(int i=0;i<availableCardSlots.Length;i++)
                {
                    if(availableCardSlots[i]==true)
                    {
                        rdmCard.gameObject.SetActive(true);
                        rdmCard.transform.position = cardSlots[i].position;
                        availableCardSlots[i] = false;
                        deck.Remove(rdmCard);
                        return;
                    }
                }
            }
        }
    private void Update() 
    {
        deckSizeText.text = deck.Count.ToString();    
    }*/
}
