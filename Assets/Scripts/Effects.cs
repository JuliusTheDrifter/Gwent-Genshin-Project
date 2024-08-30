using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Effects : MonoBehaviour //This script has the effects of the cards
{
    public GameObject zone1;
    public GameObject zone2;
    public GameObject zone3;
    public GameObject zone4;
    public GameObject zone5;
    public GameObject zone6;
    public bool inspireLoop;
    public bool weatherLoop;
    public BattleBehaviour decoy;
    public BattleBehaviour deathCount;
    public Hand hand1;
    public Draw draw;
    public Deck deck;
    public Effects effects;
    //This method checks which effect the card has and call the method corresponding to it
    public void PlayCardEffect(string effect, GameObject card)
    {
        if(effect == "OwnFileIncrease")
        {
            //OwnFileIncrease(card);
        }
        if(effect == "Weather")
        {
            //Weather(card);
        }
        if(effect == "Cleanse")
        {
           //Cleanse(card);
        }
        if(effect == "Inspire")
        {
            //Inspire(card);
        }
        if(effect == "Average")
        {
            //Average(card);
        }
        if(effect == "Multiply")
        {
            //Multiply(card);
        }
        if(effect == "Highest")
        {
            //AnnihilateHighest();
        }
        if(effect == "Lowest")
        {
            //AnnihilateLowest(card);
        }
        if(effect == "EraseFile")
        {
            //EraseFile(card);
        }
        if(effect == "Draw") //Draw a card
        {
            draw = GameObject.Find("BattleSystem").GetComponent<Draw>();
            CardDisplay cardDisplay = card.GetComponent<CardDisplay>();
            if(cardDisplay.team == 1)
            {
                draw.DrawCard(1);
            }
            else
            {
                draw.DrawCard(2);
            }
        }
        if(effect == "CallWeather")
        {
            //CallWeather(card);
        }
        if(effect == "Decoy") //This allows the decoy effect to happen
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
        if(effect == "a")
        {
            Evaluator evaluator = new Evaluator(card.GetComponent<CardDisplay>().card,card.GetComponent<CardDisplay>().card.context);
            evaluator.EvaluateEffects();
        }
    }
    //All of the effects ignore the golden cards
    //All of the points effects change the texts colors when applied

    //Increases the points of the cards in the same zone of the card that activated the effect
    /*void OwnFileIncrease(GameObject cardplayed)
    {
        string cardPosition = cardplayed.GetComponent<CardDisplay>().position;
        zone1 = GameObject.Find(cardPosition);
        //Gets the CardDisplay script from all the children of the zone
        CardDisplay[] cards = zone1.GetComponentsInChildren<CardDisplay>();
        //Increases the card points by 5
        foreach(var card in cards)
        {
            if(card.card.golden)
            {
                continue;
            }
            card.points += 5;
            card.pointsText.text = card.points.ToString();
            card.pointsText.color = Color.green;
        }
    }
    //Decreases all cards points in the zones selected to 1 affecting the player and the enemy player
    void Weather(GameObject cardplayed)
    {
        string weatherType = cardplayed.GetComponent<CardDisplay>().card.name;
        //Selects the zones to affect depending on the weather type
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
        //Activates the weatherLoop so the effect is always active each time a card is dropped
        weatherLoop = true;
    }
    //Erase all the weather cards activated
    void Cleanse(GameObject cardplayed)
    {
        deathCount = GameObject.Find("BattleSystem").GetComponent<BattleBehaviour>();
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
            //To add the cards to the graveyard
            CardDisplay cardDisplay = card.gameObject.GetComponent<CardDisplay>();
            if(cardDisplay.team ==1)
            {
                int deathcard = int.Parse(deathCount.GraveYard1.text);
                deathcard++;
                deathCount.GraveYard1.text = deathcard.ToString();
            }
            else
            {
                int deathcard = int.Parse(deathCount.GraveYard2.text);
                deathcard++;
                deathCount.GraveYard1.text = deathcard.ToString();
            }
            Destroy(card.gameObject);
        }
        //For each weather type erased, returns the cards points affected by the weather back to normla
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
                card.isEnhanced = false;
            }
            CardDisplay[] siege2 = zone3.GetComponentsInChildren<CardDisplay>();
            foreach(var card in siege2)
            {
                card.points = card.card.points;
                card.pointsText.text = card.points.ToString();
                card.pointsText.color = Color.black;
                card.isEnhanced = false;
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
                card.isEnhanced = false;
            }
            CardDisplay[] ranged2 = zone3.GetComponentsInChildren<CardDisplay>();
            foreach(var card in ranged2)
            {
                card.points = card.card.points;
                card.pointsText.text = card.points.ToString();
                card.pointsText.color = Color.black;
                card.isEnhanced = false;
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
                card.isEnhanced = false;
            }
            CardDisplay[] melee2 = zone3.GetComponentsInChildren<CardDisplay>();
            foreach(var card in melee2)
            {
                card.points = card.card.points;
                card.pointsText.text = card.points.ToString();
                card.pointsText.color = Color.black;
                card.isEnhanced = false;
            }
        }
        CardDisplay toDestroy = cardplayed.GetComponent<CardDisplay>();
        if(toDestroy.team == 1)
        {
            int deathcard = int.Parse(deathCount.GraveYard1.text);
            deathcard++;
            deathCount.GraveYard1.text = deathcard.ToString();
        }
        else
        {
            int deathcard = int.Parse(deathCount.GraveYard2.text);
            deathcard++;
            deathCount.GraveYard2.text = deathcard.ToString();
        }
        Destroy(cardplayed);
        weatherLoop = false;
    }
    //Increases the points of the cards in a specific zone
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
            //Checks if the cards was already affected by this effect
            if(!card.isEnhanced)
            {
                if(card.card.golden)
                {
                    continue;
                }
                card.points += 5;
                card.pointsText.text = card.points.ToString();
                card.pointsText.color = Color.green;
                card.isEnhanced = true;
            }
        }
        inspireLoop = true;
    }
    //Calculates the average of all the cards in the field and sets their points to that average
    void Average(GameObject cardplayed)
    {
        int sum = 0;
        int div = 0;
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
                sum += card.points;
                div++;
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
                sum += card.points;
                div++;
            }
        }
        sum /= div;
        foreach(Transform zone in zone1.transform)
        {
            CardDisplay[] cards = zone1.GetComponentsInChildren<CardDisplay>();
            foreach(var card in cards)
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
        foreach(Transform zone in zone2.transform)
        {
            CardDisplay[] cards = zone2.GetComponentsInChildren<CardDisplay>();
            foreach(var card in cards)
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
    }
    //The points of the card with this effect gets multiplied by the amount of others cards with the same id in the field
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
    //Eliminates the card with the highest points in the whole field
    void AnnihilateHighest()
    {
        int max1 = int.MinValue;
        int max2 = int.MinValue;
        GameObject cardtodestroy1 = null;
        GameObject cardtodestroy2 = null;
        zone1 = GameObject.Find("UnitsZone1");
        deathCount = GameObject.Find("BattleSystem").GetComponent<BattleBehaviour>();
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
            int deathcard = int.Parse(deathCount.GraveYard1.text);
            deathcard++;
            deathCount.GraveYard1.text = deathcard.ToString();
            Destroy(cardtodestroy1);
        }
        else
        {
            int deathcard = int.Parse(deathCount.GraveYard2.text);
            deathcard++;
            deathCount.GraveYard2.text = deathcard.ToString();
            Destroy(cardtodestroy2);
        }
    }
    //Eliminates the card with the lowest points of the enemy areas
    void AnnihilateLowest(GameObject cardplayed)
    {
        int min = int.MaxValue;
        GameObject cardtodestroy = null;
        deathCount = GameObject.Find("BattleSystem").GetComponent<BattleBehaviour>();
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
            CardDisplay cardDisplay = cardtodestroy.GetComponent<CardDisplay>();
            if(cardDisplay.team == 1)
            {
                int deathcard = int.Parse(deathCount.GraveYard1.text);
                deathcard++;
                deathCount.GraveYard1.text = deathcard.ToString();
            }
            else
            {
                int deathcard = int.Parse(deathCount.GraveYard2.text);
                deathcard++;
                deathCount.GraveYard2.text = deathcard.ToString();
            }
            Destroy(cardtodestroy);
        }
    }
    //Erase all of the cards of area in the field with the less amount of cards
    void EraseFile(GameObject cardplayed)
    {
        int units1 = 0;
        int units2 = 0;
        int units3 = 0;
        int units4 = 0;
        int units5 = 0;
        int units6 = 0;
        deathCount = GameObject.Find("BattleSystem").GetComponent<BattleBehaviour>();
        zone1 = GameObject.Find("Melee1");
        zone2 = GameObject.Find("Melee2");
        zone3 = GameObject.Find("Ranged1");
        zone4 = GameObject.Find("Ranged2");
        zone5 = GameObject.Find("Siege1");
        zone6 = GameObject.Find("Siege2");
        foreach(Transform card in zone1.transform)
        {
            units1++;
        }
        foreach(Transform card in zone2.transform)
        {
            units2++;
        }
        foreach(Transform card in zone3.transform)
        {
            units3++;
        }
        foreach(Transform card in zone4.transform)
        {
            units4++;
        }
        foreach(Transform card in zone5.transform)
        {
            units5++;
        }
        foreach(Transform card in zone6.transform)
        {
            units6++;
        }
        if(units1 == 0) units1 = int.MaxValue;
        if(units2 == 0) units2 = int.MaxValue;
        if(units3 == 0) units3 = int.MaxValue;
        if(units4 == 0) units4 = int.MaxValue;
        if(units5 == 0) units5 = int.MaxValue;
        if(units6 == 0) units6 = int.MaxValue;
        int[] units = {units1,units2,units3,units4,units5,units6};
        int temp = int.MaxValue;
        for(int i=0;i<units.Length;i++)
        {
            if(units[i] < temp)
            {
                temp = units[i];
            }
        }
        if(units1 == temp)
        {
            CardDisplay[] cards = zone1.GetComponentsInChildren<CardDisplay>();
            foreach(var card in cards)
            { 
                if(card.card.golden)
                {
                    continue;
                }
                int deathcard = int.Parse(deathCount.GraveYard1.text);
                deathcard++;
                deathCount.GraveYard1.text = deathcard.ToString();
                
                Destroy(card.gameObject);
            }
        }
        else if(units2 == temp)
        {
            CardDisplay[] cards = zone2.GetComponentsInChildren<CardDisplay>();
            foreach(var card in cards)
            {
                if(card.card.golden)
                {
                    continue;
                }
                int deathcard = int.Parse(deathCount.GraveYard2.text);
                deathcard++;
                deathCount.GraveYard2.text = deathcard.ToString();

                Destroy(card.gameObject);
            }
        }
        else if(units3 == temp)
        {
            CardDisplay[] cards = zone3.GetComponentsInChildren<CardDisplay>();
            foreach(var card in cards)
            {
                if(card.card.golden)
                {
                    continue;
                }
                int deathcard = int.Parse(deathCount.GraveYard1.text);
                deathcard++;
                deathCount.GraveYard1.text = deathcard.ToString();

                Destroy(card.gameObject);
            }
        }
        else if(units4 == temp)
        {
            CardDisplay[] cards = zone4.GetComponentsInChildren<CardDisplay>();
            foreach(var card in cards)
            { 
                if(card.card.golden)
                {
                    continue;
                }
                int deathcard = int.Parse(deathCount.GraveYard2.text);
                deathcard++;
                deathCount.GraveYard2.text = deathcard.ToString();

                Destroy(card.gameObject);
            }
        }
        else if(units5 == temp)
        {
            CardDisplay[] cards = zone5.GetComponentsInChildren<CardDisplay>();
            foreach(var card in cards)
            {
                if(card.card.golden)
                {
                    continue;
                }
                int deathcard = int.Parse(deathCount.GraveYard1.text);
                deathcard++;
                deathCount.GraveYard1.text = deathcard.ToString();

                Destroy(card.gameObject);
            }
        }
        else if(units6 == temp)
        {
            CardDisplay[] cards = zone6.GetComponentsInChildren<CardDisplay>();
            foreach(var card in cards)
            {
                if(card.card.golden)
                {
                    continue;
                }
                int deathcard = int.Parse(deathCount.GraveYard2.text);
                deathcard++;
                deathCount.GraveYard2.text = deathcard.ToString();

                Destroy(card.gameObject);
            }
        }
    }
    void CallWeather(GameObject cardplayed) //This effect sets a weather in the field
    {
        Debug.Log("Comenzo el efecto");
        CardDisplay cardDisplay = cardplayed.GetComponent<CardDisplay>();
        zone1 = GameObject.Find("Weather");
        if(cardDisplay.team ==1)
        {
            deck = GameObject.Find("DeckManager1").GetComponent<Deck>();
            List<GameObject> deckCards = deck.GetComponent<Deck>().Cards;
            for(int i=0;i<deckCards.Count;i++)
            {
                if(deckCards[i].GetComponent<CardDisplay>().name == "Frost"||deckCards[i].GetComponent<CardDisplay>().name == "Winds"||deckCards[i].GetComponent<CardDisplay>().name == "Earthquake")
                {
                    GameObject playerCard = Instantiate(deckCards[i], new Vector3(0, 0, 0), Quaternion.identity);
                    playerCard.transform.SetParent(zone1.transform, false);
                    deck.Remove(deckCards[i]);
                    effects = GameObject.Find("BattleSystem").GetComponent<Effects>();
                    effects.PlayCardEffect(playerCard.GetComponent<CardDisplay>().card.effectText,playerCard);
                    break;
                }
            }
        }
        else
        {
            deck = GameObject.Find("DeckManager2").GetComponent<Deck>();
            List<GameObject> deckCards = deck.GetComponent<Deck>().Cards;
            for(int i=0;i<deckCards.Count;i++)
            {
                if(deckCards[i].GetComponent<CardDisplay>().name == "Frost"||deckCards[i].GetComponent<CardDisplay>().name == "Winds"||deckCards[i].GetComponent<CardDisplay>().name == "Earthquake")
                {
                    GameObject playerCard = Instantiate(deckCards[i], new Vector3(0, 0, 0), Quaternion.identity);
                    playerCard.transform.SetParent(zone1.transform, false);
                    deck.Remove(deckCards[i]);
                    effects = GameObject.Find("BattleSystem").GetComponent<Effects>();
                    effects.PlayCardEffect(playerCard.GetComponent<CardDisplay>().card.effectText,playerCard);
                    break;
                }
            }
        }
    }*/
}