using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.XR;

public class Draw : MonoBehaviour
{
    public GameObject Card;
    public GameObject Hand1;
    public GameObject Hand2;
    public Deck deck1;
    public Deck deck2;
    public TMP_Text deck1Size;
    public TMP_Text deck2Size;
    void Start()
    {
        Hand1 = GameObject.Find("Hand1");
        Hand2 = GameObject.Find("Hand2");
        deck1 = GameObject.Find("DeckManager1").GetComponent<Deck>();
        deck2 = GameObject.Find("DeckManager2").GetComponent<Deck>();
        deck1Size.text = deck1.GetCards().Count.ToString();
        deck2Size.text = deck1.GetCards().Count.ToString();
        for(int i=0;i<10;i++)
        {
            Draw1();
            Draw2();
        }
    }
    public void Draw1()
    {
        List<GameObject> deckCards = deck1.GetCards();
        int randomIndex = Random.Range(0, deckCards.Count);
        GameObject selectedCard = deckCards[randomIndex];
        GameObject playerCard = Instantiate(selectedCard, new Vector3(0, 0, 0), Quaternion.identity);
        playerCard.transform.SetParent(Hand1.transform, false);
        deckCards.RemoveAt(randomIndex);
        deck1Size.text = deck1.GetCards().Count.ToString();
    }
    public void Draw2()
    {
        List<GameObject> deckCards = deck2.GetCards();
        int randomIndex = Random.Range(0, deckCards.Count);
        GameObject selectedCard = deckCards[randomIndex];
        GameObject playerCard = Instantiate(selectedCard, new Vector3(0, 0, 0), Quaternion.identity);
        playerCard.transform.SetParent(Hand2.transform, false);
        deckCards.RemoveAt(randomIndex);
        deck2Size.text = deck2.GetCards().Count.ToString();
    }
}