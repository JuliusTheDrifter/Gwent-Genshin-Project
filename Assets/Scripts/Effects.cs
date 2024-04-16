using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Effects : MonoBehaviour
{
    public GameObject zone1;
    public GameObject zone2;
    public GameObject zone3;
    public GameObject zone4;
    public GameObject zone5;
    public GameObject zone6;
    public bool inspireLoop;
    public bool weatherLoop;
    void Update()
    {

    }
    public void PlayCardEffect(string effect, GameObject card)
    {
        if(effect == "OwnFileIncrease")
        {
            OwnFileIncrease(card);
        }
        if(effect == "Weather")
        {
            Weather(card);
        }
        if(effect == "Cleanse")
        {
            Cleanse(card);
        }
        if(effect == "Inspire")
        {
            Inspire(card);
        }
        if(effect == "Average")
        {
            Average(card);
        }
        if(effect == "Multiply")
        {
            Multiply(card);
        }
        /*if(effect == "Highest")
        {
            AnnihilateHighest();
        }
        if(effect == "Lowest")
        {
            AnnihilateLowest(card);
        }*/
    }
    void OwnFileIncrease(GameObject cardplayed)
    {
        string cardPosition = cardplayed.GetComponent<CardDisplay>().position;
        zone1 = GameObject.Find(cardPosition);
        CardDisplay[] cards = zone1.GetComponentsInChildren<CardDisplay>();
        foreach(var card in cards)
        {
            card.points = card.card.points;
            card.points += 5;
            card.pointsText.text = card.points.ToString();
        }
    }
    void Weather(GameObject cardplayed)
    {
        string weatherType = cardplayed.GetComponent<CardDisplay>().card.name;
        if(weatherType == "Frost")
        {
            zone1 = GameObject.Find("Siege1");
            zone2 = GameObject.Find("Siege2");
        }
        if(weatherType == "Winds")
        {
            zone1 = GameObject.Find("Ranged1");
            zone2 = GameObject.Find("Ranged2");
        }
        if(weatherType == "Earthquake")
        {
            zone1 = GameObject.Find("Melee1");
            zone2 = GameObject.Find("Melee2");
        }
        CardDisplay[] cards = zone1.GetComponentsInChildren<CardDisplay>();
        foreach(var card in cards)
        {
            card.points = 1;
            card.pointsText.text = card.points.ToString();
        }
        CardDisplay[] cards2 = zone2.GetComponentsInChildren<CardDisplay>();
        foreach(var card in cards2)
        {
            card.points = 1;
            card.pointsText.text = card.points.ToString();
        }
        weatherLoop = true;
    }
    void Cleanse(GameObject cardplayed)
    {
        zone1 = GameObject.Find("Weather");
        int[] weatherCheck = new int[3];
        foreach(Transform card in zone1.transform)
        {
            if(card.gameObject.GetComponent<CardDisplay>().card.name == "Frost")
            {
                weatherCheck[0]=1;
            } 
            if(card.gameObject.GetComponent<CardDisplay>().card.name == "Winds")
            {
                weatherCheck[1]=1;
            }
            if(card.gameObject.GetComponent<CardDisplay>().card.name == "Earthquake")
            {
                weatherCheck[2]=1;
            }
            Destroy(card.gameObject);
        }
        if(weatherCheck[0]!=0)
        {
            zone2 = GameObject.Find("Siege1");
            zone3 = GameObject.Find("Siege2");
            CardDisplay[] siege1 = zone2.GetComponentsInChildren<CardDisplay>();
            foreach(var card in siege1)
            {
                card.points = card.card.points;
                card.pointsText.text = card.points.ToString();
            }
            CardDisplay[] siege2 = zone3.GetComponentsInChildren<CardDisplay>();
            foreach(var card in siege2)
            {
                card.points = card.card.points;
                card.pointsText.text = card.points.ToString();
            }
        }
        if(weatherCheck[1]!=0)
        {
            zone2 = GameObject.Find("Ranged1");
            zone3 = GameObject.Find("Ranged2");
            CardDisplay[] ranged1 = zone2.GetComponentsInChildren<CardDisplay>();
            foreach(var card in ranged1)
            {
                card.points = card.card.points;
                card.pointsText.text = card.points.ToString();
            }
            CardDisplay[] ranged2 = zone3.GetComponentsInChildren<CardDisplay>();
            foreach(var card in ranged2)
            {
                card.points = card.card.points;
                card.pointsText.text = card.points.ToString();
            }
        }
        if(weatherCheck[2]!=0)
        {
            zone2 = GameObject.Find("Melee1");
            zone3 = GameObject.Find("Melee2");
            CardDisplay[] melee1 = zone2.GetComponentsInChildren<CardDisplay>();
            foreach(var card in melee1)
            {
                card.points = card.card.points;
                card.pointsText.text = card.points.ToString();
            }
            CardDisplay[] melee2 = zone3.GetComponentsInChildren<CardDisplay>();
            foreach(var card in melee2)
            {
                card.points = card.card.points;
                card.pointsText.text = card.points.ToString();
            }
        }
        Destroy(cardplayed);
        weatherLoop = false;
    }
    void Inspire(GameObject cardplayed)
    {
        string zoneInspired = cardplayed.GetComponent<CardDisplay>().position;
        if(zoneInspired == "InspireM1") zone1 = GameObject.Find("Melee1");
        else if(zoneInspired == "InspireR1") zone1 = GameObject.Find("Ranged1");
        else if(zoneInspired == "InspireS1") zone1 = GameObject.Find("Siege1");
        else if(zoneInspired == "InspireM2") zone1 = GameObject.Find("Melee2");
        else if(zoneInspired == "InspireR2") zone1 = GameObject.Find("Ranged2");
        else if(zoneInspired == "InspireS2") zone1 = GameObject.Find("Siege2");
        
        CardDisplay[] cards = zone1.GetComponentsInChildren<CardDisplay>();
        foreach(var card in cards)
        {
            if(!card.card.isEnhanced)
            {
                card.points = card.card.points;
                card.points += 5;
                card.pointsText.text = card.points.ToString();
                card.card.isEnhanced = true;
            }
        }
        inspireLoop = true;
    }
    void Average(GameObject cardplayed)
    {
        if(cardplayed.GetComponent<CardDisplay>().team == 1)
        {
            zone1 = GameObject.Find("Melee1");
            zone2 = GameObject.Find("Ranged1");
            zone3 = GameObject.Find("Siege1");
        }
        else
        {
            zone1 = GameObject.Find("Melee2");
            zone2 = GameObject.Find("Ranged2");
            zone3 = GameObject.Find("Siege2");
        }
        int sum = 0;
        int div = 0;
        
        CardDisplay[] cards1 = zone1.GetComponentsInChildren<CardDisplay>();
        foreach(var card in cards1)
        {
            sum += card.points;
            div++;
        }
            
        CardDisplay[] cards2 = zone2.GetComponentsInChildren<CardDisplay>();
        foreach(var card in cards2)
        {
            sum += card.points;
            div++;
        }
            
        CardDisplay[] cards3 = zone3.GetComponentsInChildren<CardDisplay>();
        foreach(var card in cards3)
        {
            sum += card.points;
            div++;
        }
            
        sum = sum/div;
        foreach(var card in cards1)
        {
            card.points = sum;
        }
        foreach(var card in cards2)
        {
            card.points = sum;
        }
        foreach(var card in cards3)
        {
            card.points = sum;
        }
    }
    void Multiply(GameObject cardplayed)
    {
        string zone = cardplayed.GetComponent<CardDisplay>().position;
        int id = cardplayed.GetComponent<CardDisplay>().card.id;
        int counter = 0;
        zone1 = GameObject.Find(zone);
        CardDisplay[] cards = zone1.GetComponentsInChildren<CardDisplay>();
        foreach(var card in cards)
        {
            if(id == card.card.id) counter++;
        }
        foreach(var card in cards)
        {
            if(id == card.card.id)
            {
                card.points *= counter;
            }
        }
    }
    /*void AnnihilateHighest()
    {
        
    }
    void AnnihilateLowest(GameObject cardplayed)
    {
        int team = cardplayed.GetComponent<CardDisplay>().team;
        int max1 = int.MaxValue;
        int max2 = int.MaxValue;
        int max3 = int.MaxValue;
        int cardid1 = 0;
        int cardid2 = 0;
        int cardid3 = 0;
        if(team == 1)
        {
            zone1 = GameObject.Find("Melee 2");
            zone2 = GameObject.Find("Ranged 2");
            zone3 = GameObject.Find("Siege 2");
            CardDisplay[] cards1 = zone1.GetComponentsInChildren<CardDisplay>();
            if(cards1 != null)
            {
                foreach(var card in cards1)
                {
                    if(card.card.points < max1)
                    {
                        max1 = card.card.points;
                        cardid1 = card.card.id;
                    }
                }
            }
            CardDisplay[] cards2 = zone2.GetComponentsInChildren<CardDisplay>();
            if(cards2 != null)
            {
                foreach(var card in cards2)
                {
                    if(card.card.points < max2)
                    {
                        max2 = card.card.points;
                        cardid2 = card.card.id;
                    }
                }
            }
            CardDisplay[] cards3 = zone3.GetComponentsInChildren<CardDisplay>();
            if(cards3 != null)
            {
                foreach(var card in cards3)
                {
                    if(card.card.points < max3)
                    {
                        max3 = card.card.points;
                        cardid3 = card.card.id;
                    }
                }
            }
            if(Math.Min(max1,Math.Min(max2,max3)) == max1)
            {

            }
            else if(Math.Min(max1,Math.Min(max2,max3)) == max2)
            {
                cardid1 = cardid2;
            }
            else if(Math.Min(max1,Math.Min(max2,max3)) == max3)
            {
                cardid1 = cardid3;
            }
            GameObject cardToEliminate = FindCardByID(cardid1);
            Destroy(cardToEliminate);
        }
        else
        {

        }
    }*/
}