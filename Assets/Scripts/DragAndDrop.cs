using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

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
    private void Awake()
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
        startPosition = transform.position;
        isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        turns = GameObject.Find("BattleSystem").GetComponent<BattleBehaviour>();
        bool rotateMouse = turns.player1Turn;
        if(!canBePlaced)
        {
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
        if(IsOverDropZone && CorrectZone())
        {
            transform.SetParent(dropZone.transform, false);
            canBePlaced=true;
            endTurn = GameObject.Find("BattleSystem").GetComponent<BattleBehaviour>();
            UnityEngine.Debug.Log("Effect");
            effects = GameObject.Find("BattleSystem").GetComponent<Effects>();
            effects.PlayCardEffect(gameObject.GetComponent<CardDisplay>().card.effect,gameObject);
            
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
            if(!endTurn.decoyTime)
            {
                endTurn.EndTurn();
            }
        }
        else
        {
            transform.position = startPosition;
            transform.SetParent(startParent.transform, false);
        }
    }
    public bool CorrectZone()
    {
        ZoneConditions conditions = dropZone.GetComponent<ZoneConditions>(); 
        string zoneName = conditions.theZone;
        string cardPosition = gameObject.GetComponent<CardDisplay>().position;
        if(zoneName==cardPosition)return true;
        else return false;
    }
    public void OnPointerClick()
    {
        CardDisplay cardDisplay = GetComponent<CardDisplay>();
        BattleBehaviour decoy = GameObject.Find("BattleSystem").GetComponent<BattleBehaviour>();
        if(decoy.decoyTime && decoy.team1)
        {
            if(cardDisplay.team ==1)
            {
                GameObject zone1 = GameObject.Find("Hand1");
                transform.position = zone1.transform.position;
                transform.SetParent(zone1.transform,false);
                decoy.decoyTime = false;
                decoy.EndTurn();
            }
        }
        else if(decoy.decoyTime && decoy.team2)
        {
            if(cardDisplay.team ==2)
            {
                GameObject zone1 = GameObject.Find("Hand2");
                transform.position = zone1.transform.position;
                transform.SetParent(zone1.transform,false);
                decoy.decoyTime = false;
                decoy.EndTurn();
            }
        }
    }
    public void ChangingCards()
    {
        draw = GameObject.Find("BattleSystem").GetComponent<Draw>();
        changeCards = GameObject.Find("ToChangeP1").GetComponent<ChangeCards>();
        CardDisplay cardDisplay = GetComponent<CardDisplay>();
        int counter = 0;
        if(changeCards.changeTime1)
        {
            deck = GameObject.Find("DeckManager1").GetComponent<Deck>();
            List<GameObject> deckCards = deck.GetCards();
            if(cardDisplay.team == 1)
            {
                deckCards.Add(gameObject);
                Destroy(gameObject);
                draw.Draw1();
                counter++;
                if(counter ==2)
                {
                    changeCards.HideB1();
                }
            }
        }
        else if(changeCards.changeTime2)
        {
            deck = GameObject.Find("DeckManager2").GetComponent<Deck>();
            List<GameObject> deckCards = deck.GetCards();
            if(cardDisplay.team == 2)
            {
                deckCards.Add(gameObject);
                Destroy(gameObject);
                draw.Draw2();
                counter++;
                if(counter == 2)
                {
                    changeCards.HideB2();
                }
            }
        }
    }
}
