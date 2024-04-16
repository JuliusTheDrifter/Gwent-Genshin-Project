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
            endTurn.EndTurn();
            UnityEngine.Debug.Log("Effect");
            effects = GameObject.Find("BattleSystem").GetComponent<Effects>();
            effects.PlayCardEffect(gameObject.GetComponent<CardDisplay>().card.effect,gameObject);
            
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
            if(effects.inspireLoop)
            {
                /*CardDisplay[] cards = GameObject.Find("Weather").GetComponentsInChildren<CardDisplay>();
                if(cards != null)
                {
                    foreach(var card in cards)
                    {
                        effects.PlayCardEffect(card.card.effect,card.gameObject);
                    }
                }*/
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
}
