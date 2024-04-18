using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Effects : MonoBehaviour
{
    public GameObject zone1;
    public GameObject zone2;
    public GameObject zone3;
    public bool inspireLoop;
    public bool weatherLoop;
    public BattleBehaviour decoy;
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
        if(effect == "Highest")
        {
            AnnihilateHighest();
        }
        if(effect == "Lowest")
        {
            AnnihilateLowest(card);
        }
        if(effect == "EraseFile")
        {
            EraseFile(card);
        }
        if(effect == "Decoy")
        {
            decoy = GameObject.Find("BattleSystem").GetComponent<BattleBehaviour>();
            decoy.decoyTime = true;
            if(card.GetComponent<CardDisplay>().team == 1)
            {
                decoy.team1 = true;
            }
            else
            {
                decoy.team2 = true;
            }
        }
    }
    void OwnFileIncrease(GameObject cardplayed)
    {
        string cardPosition = cardplayed.GetComponent<CardDisplay>().position;
        zone1 = GameObject.Find(cardPosition);
        CardDisplay[] cards = zone1.GetComponentsInChildren<CardDisplay>();
        foreach(var card in cards)
        {
            if(card.card.golden)
            {
                continue;
            }
            card.points = card.card.points;
            card.points += 5;
            card.pointsText.text = card.points.ToString();
            card.pointsText.color = Color.green;
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
            if(card.card.golden)
            {
                continue;
            }
            card.points = 1;
            card.pointsText.text = card.points.ToString();
            card.pointsText.color = Color.red;
        }
        CardDisplay[] cards2 = zone2.GetComponentsInChildren<CardDisplay>();
        foreach(var card in cards2)
        {
            if(card.card.golden)
            {
                continue;
            }
            card.points = 1;
            card.pointsText.text = card.points.ToString();
            card.pointsText.color = Color.red;
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
                card.pointsText.color = Color.black;
                card.card.isEnhanced = false;
            }
            CardDisplay[] siege2 = zone3.GetComponentsInChildren<CardDisplay>();
            foreach(var card in siege2)
            {
                card.points = card.card.points;
                card.pointsText.text = card.points.ToString();
                card.pointsText.color = Color.black;
                card.card.isEnhanced = false;
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
                card.pointsText.color = Color.black;
                card.card.isEnhanced = false;
            }
            CardDisplay[] ranged2 = zone3.GetComponentsInChildren<CardDisplay>();
            foreach(var card in ranged2)
            {
                card.points = card.card.points;
                card.pointsText.text = card.points.ToString();
                card.pointsText.color = Color.black;
                card.card.isEnhanced = false;
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
                card.pointsText.color = Color.black;
                card.card.isEnhanced = false;
            }
            CardDisplay[] melee2 = zone3.GetComponentsInChildren<CardDisplay>();
            foreach(var card in melee2)
            {
                card.points = card.card.points;
                card.pointsText.text = card.points.ToString();
                card.pointsText.color = Color.black;
                card.card.isEnhanced = false;
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
                if(card.card.golden)
                {
                    continue;
                }
                card.points = card.card.points;
                card.points += 5;
                card.pointsText.text = card.points.ToString();
                card.pointsText.color = Color.green;
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
            if(card.card.golden)
            {
                continue;
            }
            sum += card.points;
            div++;
        }
            
        CardDisplay[] cards2 = zone2.GetComponentsInChildren<CardDisplay>();
        foreach(var card in cards2)
        {
            if(card.card.golden)
            {
                continue;
            }
            sum += card.points;
            div++;
        }
            
        CardDisplay[] cards3 = zone3.GetComponentsInChildren<CardDisplay>();
        foreach(var card in cards3)
        {
            if(card.card.golden)
            {
                continue;
            }
            sum += card.points;
            div++;
        }
            
        sum /= div;
        foreach(var card in cards1)
        {
            if(card.card.golden)
            {
                continue;
            }
            card.points = sum;
            card.pointsText.text = card.points.ToString();
            card.pointsText.color = card.pointsText.color = new Color(1, 0.5f, 0);
        }
        foreach(var card in cards2)
        {
            if(card.card.golden)
            {
                continue;
            }
            card.points = sum;
            card.pointsText.text = card.points.ToString();
            card.pointsText.color = card.pointsText.color = new Color(1, 0.5f, 0);
        }
        foreach(var card in cards3)
        {
            if(card.card.golden)
            {
                continue;
            }
            card.points = sum;
            card.pointsText.text = card.points.ToString();
            card.pointsText.color = card.pointsText.color = new Color(1, 0.5f, 0);
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
                card.pointsText.text = card.points.ToString();
                card.pointsText.color = Color.blue;
            }
        }
    }
    void AnnihilateHighest()
    {
        int max1 = int.MinValue;
        int max2 = int.MinValue;
        GameObject cardtodestroy1 = null;
        GameObject cardtodestroy2 = null;
        zone1 = GameObject.Find("UnitsZone1");
        foreach(Transform zone in zone1.transform)
        {
            CardDisplay[] cards = zone1.GetComponentsInChildren<CardDisplay>();
            foreach(var card in cards)
            {
                if(card.card.golden)
                {
                    continue;
                }
                if(card.points>max1)
                {
                    max1 = card.points;
                    cardtodestroy1 = card.gameObject;
                }
            }
        }
        zone2 = GameObject.Find("UnitsZone2");
        foreach(Transform zone in zone2.transform)
        {
            CardDisplay[] cards = zone2.GetComponentsInChildren<CardDisplay>();
            foreach(var card in cards)
            {
                if(card.card.golden)
                {
                    continue;
                }
                if(card.points>max2)
                {
                    max2 = card.points;
                    cardtodestroy2 = card.gameObject;
                }
            }
        }
        if(max1>max2)
        {
            Destroy(cardtodestroy1);
        }
        else
        {
            Destroy(cardtodestroy2);
        }
    }
    void AnnihilateLowest(GameObject cardplayed)
    {
        int min = int.MaxValue;
        GameObject cardtodestroy = null;
        if(cardplayed.GetComponent<CardDisplay>().team == 1)
        {
            zone1 = GameObject.Find("UnitsZone2");
        }
        else
        {
            zone1 = GameObject.Find("UnitsZone1");
        }
        foreach(Transform zone in zone1.transform)
        {
            CardDisplay[] cards = zone1.GetComponentsInChildren<CardDisplay>();
            foreach(var card in cards)
            {
                if(card.card.golden)
                {
                    continue;
                }
                if(card.points<min)
                {
                    min = card.points;
                    cardtodestroy = card.gameObject;
                }
            }
        }
        if(cardtodestroy != null)
        {
            Destroy(cardtodestroy);
        }
    }
    void EraseFile(GameObject cardplayed)
    {
        int units1 = 0;
        int units2 = 0;
        int units3 = 0;
        if(cardplayed.GetComponent<CardDisplay>().team == 1)
        {
            zone1 = GameObject.Find("Melee2");
            zone2 = GameObject.Find("Ranged2");
            zone3 = GameObject.Find("Siege2");
        }
        else
        {
            zone1 = GameObject.Find("Melee1");
            zone2 = GameObject.Find("Ranged1");
            zone3 = GameObject.Find("Siege1");
        }
        foreach(Transform card in zone1.transform)
        {
            units1++;
        }
        Debug.Log("This is units1 "+units1);
        foreach(Transform card in zone2.transform)
        {
            units2++;
        }
        Debug.Log("This is units2 "+units2);
        foreach(Transform card in zone3.transform)
        {
            units3++;
        }
        Debug.Log("This is units3 "+units3);
        if(units1 == 0) units1 = int.MaxValue;
        if(units2 == 0) units2 = int.MaxValue;
        if(units3 == 0) units3 = int.MaxValue;
        if(units1 <= Math.Min(units2,units3))
        {
            CardDisplay[] cards = zone1.GetComponentsInChildren<CardDisplay>();
            foreach(var card in cards)
            { 
                if(card.card.golden)
                {
                    continue;
                }
                Destroy(card.gameObject);
            }
        }
        else if(units2 <= Math.Min(units1,units3))
        {
            CardDisplay[] cards = zone2.GetComponentsInChildren<CardDisplay>();
            foreach(var card in cards)
            {
                if(card.card.golden)
                {
                    continue;
                }
                Destroy(card.gameObject);
            }
        }
        else if(units3 <= Math.Min(units2,units1))
        {
            CardDisplay[] cards = zone3.GetComponentsInChildren<CardDisplay>();
            foreach(var card in cards)
            {
                if(card.card.golden)
                {
                    continue;
                }
                Destroy(card.gameObject);
            }
        }
    }
}