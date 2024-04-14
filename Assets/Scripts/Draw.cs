using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class Draw : MonoBehaviour
{
    public GameObject Card;
    public GameObject Hand1;
    public GameObject Hand2;
    public Deck deck1;
    public Deck deck2;
    void Start()
    {
        
        for(int i=0;i<10;i++)
        {
            OnClick1();
            OnClick2();
        }
    }
    public void OnClick1()
    {
        List<GameObject> deckCards = deck1.GetCards();
        Debug.Log(deckCards.Count);
        int randomIndex = Random.Range(0, deckCards.Count);
        GameObject selectedCard = deckCards[randomIndex];
        GameObject playerCard = Instantiate(selectedCard, new Vector3(0, 0, 0), Quaternion.identity);
        playerCard.transform.SetParent(Hand1.transform, false);
        deckCards.RemoveAt(randomIndex);
    }
    public void OnClick2()
    {
        List<GameObject> deckCards = deck2.GetCards();
        int randomIndex = Random.Range(0, deckCards.Count);
        GameObject selectedCard = deckCards[randomIndex];
        GameObject playerCard = Instantiate(selectedCard, new Vector3(0, 0, 0), Quaternion.identity);
        playerCard.transform.SetParent(Hand2.transform, false);
        deckCards.RemoveAt(randomIndex);
    }
}