using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
public enum BattleState {START,PLAYER1TURN,PLAYER2TURN,WON,LOST}
public class BattleBehaviour : MonoBehaviour
{
    public bool player1Turn;
    public GameObject player1Hand;
    public GameObject player2Hand;
    
    void Start()
    {
        EndTurn();
    }
    public void EndTurn()
    {
        player1Turn =! player1Turn;
        if(player1Turn)
        {
            Visibility1(player1Hand,true);
            Visibility2(player2Hand,false);
            NoInteractions(player2Hand,false);
            NoInteractions(player1Hand,true);
        }
        else
        {
            Visibility1(player1Hand,false);
            Visibility2(player2Hand,true);
            NoInteractions(player1Hand,false);
            NoInteractions(player2Hand,true);
        }
    }
    public void NoInteractions(GameObject playerHand,bool  active)
    {
        DragAndDrop[] cards = playerHand.GetComponentsInChildren<DragAndDrop>();
        if(active)
        {
            foreach(var card in cards)
            {
                card.enabled = true;
            }
        }
        else
        {
            foreach(var card in cards)
            {
                card.enabled = false;
            }
        }
    }
    public void  Visibility1(GameObject playerHand,bool visible)
    {
        UnityEngine.Vector3 pos = transform.position;
        foreach(Transform card in playerHand.transform)
        {
            if(!visible)
            {
                pos = card.transform.position;
                pos.z = -10;
                card.transform.position = pos;
            }
            else
            {
                pos = card.transform.position;
                pos.z = 0;
                card.transform.position = pos;
            }
        }
    }
    public void  Visibility2(GameObject playerHand,bool visible)
    {
        UnityEngine.Vector3 pos = transform.position;
        foreach(Transform card in playerHand.transform)
        {
            if(!visible)
            {
                pos = card.transform.position;
                pos.z = -10;
                card.transform.position = pos;
            }
            else
            {
                pos = card.transform.position;
                pos.z = 0;
                card.transform.position = pos;
            }
        }
    }
}































/*public BattleState state;
    void Start()
    {
        state = BattleState.START;
        SetupBattle();
    }
    void Update()
    {
        
    }
    void SetupBattle()
    {
        state = BattleState.PLAYER1TURN;
        Player();
    }
    void Player()
    {

    }
    public void OnPassButton()
    {
        if(state != BattleState.PLAYER1TURN)
        {
            return;
        }
    }*/
