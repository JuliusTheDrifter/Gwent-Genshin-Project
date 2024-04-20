using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
//This method is to move the cards with the mouse and drop it in specific areas
public class DragAndDrop : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    private RectTransform rectTransform;
    private bool isDragging;
    private Vector2 startPosition;
    public GameObject Canvas;
    public GameObject DropZone;
    private GameObject startParent;
    private GameObject dropZone;
    public BattleBehaviour endTurn;
    public BattleBehaviour turns;
    public Effects effects;
    public Deck deck;
    public Draw draw;
    public ChangeCards changeCards;
    private bool IsOverDropZone;
    private bool canBePlaced;
    void Awake() //Gets the rectTransform of the card
    {
        rectTransform = GetComponent<RectTransform>();
    }
    void Start()
    {
        Canvas = GameObject.Find("Board");
        DropZone = GameObject.Find("Area");
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        IsOverDropZone = true;
        dropZone = collision.gameObject;
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        IsOverDropZone = false;
        dropZone = null;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = transform.position; //Saves the starting position
        isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        turns = GameObject.Find("BattleSystem").GetComponent<BattleBehaviour>();
        bool rotateMouse = turns.player1Turn;
        //This checks if the card can move
        if(!canBePlaced)
        {
            //This rotates the mouse mobility acording to the turn
            if(rotateMouse)
            {
                rectTransform.anchoredPosition += 3 * eventData.delta;
            }
            else
            {
                rectTransform.anchoredPosition -= 3 * eventData.delta;
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        if(IsOverDropZone && CorrectZone()) //This checks if the card can be placed in the zone
        {
            //This sets the card as child of the dropzone
            transform.SetParent(dropZone.transform, false);
            canBePlaced=true;
            endTurn = GameObject.Find("BattleSystem").GetComponent<BattleBehaviour>();
            UnityEngine.Debug.Log("Effect");
            effects = GameObject.Find("BattleSystem").GetComponent<Effects>();
            effects.PlayCardEffect(gameObject.GetComponent<CardDisplay>().card.effect,gameObject);
            //This is to check if there's any Inspire card and activate its effect again
            if(effects.inspireLoop)
            {
                CardDisplay[] cards = new CardDisplay[6];
                cards[0] = GameObject.Find("InspireM1").GetComponentInChildren<CardDisplay>();
                cards[1] = GameObject.Find("InspireR1").GetComponentInChildren<CardDisplay>();
                cards[2] = GameObject.Find("InspireS1").GetComponentInChildren<CardDisplay>();
                cards[3] = GameObject.Find("InspireM2").GetComponentInChildren<CardDisplay>();
                cards[4] = GameObject.Find("InspireR2").GetComponentInChildren<CardDisplay>();
                cards[5] = GameObject.Find("InspireS2").GetComponentInChildren<CardDisplay>();
                for(int i=0;i<6;i++)
                {
                    if(cards[i]!=null)
                    {
                        effects.PlayCardEffect(cards[i].card.effect,cards[i].gameObject);
                    }
                }
            }
            //This is to check if there's any Weather card and activate its effect again
            if(effects.weatherLoop)
            {
                CardDisplay[] cards = GameObject.Find("Weather").GetComponentsInChildren<CardDisplay>();
                if(cards != null)
                {
                    foreach(var card in cards)
                    {
                        effects.PlayCardEffect(card.card.effect,card.gameObject);
                    }
                }
            }
            //If you drop a decoy the turn won't end until you use its effect, else the turn will end when you drop any other card
            if(!endTurn.decoyTime)
            {
                endTurn.EndTurn();
            }
        }
        //If the cards wasn't placed correctly, it will return to the starting position
        else
        {
            transform.position = startPosition;
            transform.SetParent(startParent.transform, false);
        }
    }
    //This method checks is the theZone component and compares it to the cardPosition string and returns a bool
    public bool CorrectZone()
    {
        ZoneConditions conditions = dropZone.GetComponent<ZoneConditions>(); 
        string zoneName = conditions.theZone;
        CardDisplay card = gameObject.GetComponent<CardDisplay>();
        if(zoneName==card.position)return true;
        //Decoys can be placed in all the units zones
        else if(card.team==1&&zoneName=="Melee1"&&card.card.effect=="Decoy")return true;
        else if(card.team==1&&zoneName=="Ranged1"&&card.card.effect=="Decoy")return true;
        else if(card.team==1&&zoneName=="Siege1"&&card.card.effect=="Decoy")return true;
        else if(card.team==2&&zoneName=="Melee2"&&card.card.effect=="Decoy")return true;
        else if(card.team==2&&zoneName=="Ranged2"&&card.card.effect=="Decoy")return true;
        else if(card.team==2&&zoneName=="Siege2"&&card.card.effect=="Decoy")return true;
        else return false;
    }
    //This method is for the Decoy effect, it returns the card on the field you clicked on to the hand
    public void OnPointerClick()
    {
        CardDisplay cardDisplay = GetComponent<CardDisplay>();
        BattleBehaviour decoy = GameObject.Find("BattleSystem").GetComponent<BattleBehaviour>();
        //Checks if the decoy is active and if the team of the card clicked is the correct one
        if(decoy.decoyTime && decoy.team1)
        {
            if(cardDisplay.team ==1)
            {
                GameObject zone1 = GameObject.Find("Hand1");
                cardDisplay.points = cardDisplay.card.points;
                transform.position = zone1.transform.position;
                transform.SetParent(zone1.transform,false);
                decoy.decoyTime = false;
                canBePlaced=false;
                decoy.EndTurn();
            }
        }
        else if(decoy.decoyTime && decoy.team2)
        {
            if(cardDisplay.team ==2)
            {
                GameObject zone1 = GameObject.Find("Hand2");
                cardDisplay.points = cardDisplay.card.points;
                transform.position = zone1.transform.position;
                transform.SetParent(zone1.transform,false);
                decoy.decoyTime = false;
                canBePlaced=false;
                decoy.EndTurn();
            }
        }
    }
    //This method allows the player to change a maximun of 2 cards by adding the card click to the deck and drawing another one
    public void ChangingCards()
    {
        draw = GameObject.Find("BattleSystem").GetComponent<Draw>();
        endTurn = GameObject.Find("BattleSystem").GetComponent<BattleBehaviour>();
        CardDisplay cardDisplay = GetComponent<CardDisplay>();
        //This checks which turn is it
        if(endTurn.player1Turn)
        {
            changeCards = GameObject.Find("ToChangeP1").GetComponent<ChangeCards>();
            GameObject hand = GameObject.Find("Hand1");
            //This checks if it's changing time
            if(changeCards.changeTime)
            {
                deck = GameObject.Find("DeckManager1").GetComponent<Deck>();
                List<GameObject> deckCards = deck.GetCards();
                if(cardDisplay.team == 1)
                {
                    deckCards.Add(gameObject);
                    draw.Draw1();
                    Destroy(gameObject);
                    changeCards.counter++;
                    if(changeCards.counter ==2)
                    {
                        endTurn.Immovable(hand,true);
                        changeCards.changeTime = false;
                        changeCards.Hide();
                    }
                }
            }
        }
        else if(!endTurn.player1Turn)
        {
            changeCards = GameObject.Find("ToChangeP2").GetComponent<ChangeCards>();
            GameObject hand = GameObject.Find("Hand2");
            if(changeCards.changeTime)
            {
                deck = GameObject.Find("DeckManager2").GetComponent<Deck>();
                List<GameObject> deckCards = deck.GetCards();
                if(cardDisplay.team == 2)
                {
                    deckCards.Add(gameObject);
                    draw.Draw2();
                    Destroy(gameObject);
                    changeCards.counter++;
                    if(changeCards.counter == 2)
                    {
                        endTurn.Immovable(hand,true);
                        changeCards.changeTime = false;
                        changeCards.Hide();
                    }
                }
            } 
        }
    }
}
