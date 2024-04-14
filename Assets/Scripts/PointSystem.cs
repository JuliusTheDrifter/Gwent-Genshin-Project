using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Collections;

public class PointSystem : MonoBehaviour
{
    public TMP_Text player1Points;
    public TMP_Text player2Points;
    public GameObject player1Hand;
    public GameObject player2Hand;
    void Start()
    {
        CollectP1Points();
    }
    void Update()
    {
        //CollectP1Points();
    }
    void CollectP1Points()
    {
        CardDisplay[] cards = player1Hand.GetComponentsInChildren<CardDisplay>();
        int[] pa = new int[20];
        int x = 0;
        int y = 0;
        foreach(var card in cards)
        {
           pa[x++] = card.card.points;
           Debug.Log(pa[--x]);
        }
        for(int i=0;i<20;i++)
        {
            y += pa[i];
        }
        Debug.Log(y);
        player1Points.text = y.ToString();
    }
    void CollectP2Points()
    {

    }
}
