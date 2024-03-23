using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public enum BattleState {START,PLAYER1TURN,PLAYER2TURN,WON,LOST}
public class BattleBehaviour : MonoBehaviour
{
    public BattleState state;
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
    }
}
