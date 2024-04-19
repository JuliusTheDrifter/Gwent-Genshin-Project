using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Zoom : MonoBehaviour //This script shows a bigger form of the card when the mouse is over the card
{
    public GameObject Canvas;
    public GameObject zoomCard;
    public BattleBehaviour turn;
    private Vector2 zoomScale = new Vector2(3,3);
    public void Awake()
    {
        Canvas = GameObject.Find("Board");
    }
    public void OnMouseEnter() //When mouse enters the card, shows a bigger form of the card in an specific place
    {
        turn = GameObject.Find("BattleSystem").GetComponent<BattleBehaviour>();
        //Depending on the turn, the zoomcard rotates
        if (turn.player1Turn)
        {
            zoomCard = Instantiate(gameObject,new Vector2(210,725), Quaternion.identity);
            zoomCard.transform.SetParent(Canvas.transform,false);
            zoomCard.transform.localScale = zoomScale;
        }
        else
        {
            zoomCard = Instantiate(gameObject,new Vector2(210,725), Quaternion.Euler(180,180,0));
            zoomCard.transform.SetParent(Canvas.transform,false);
            zoomCard.transform.localScale = zoomScale;
        }
    }
    public void OnMouseExit() //When mouse exits the card, the zoomcard is destroyed
    {
        Destroy(zoomCard);
    }
}
