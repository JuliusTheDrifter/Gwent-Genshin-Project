using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeCards : MonoBehaviour
{
    public bool changeTime1;
    public bool changeTime2;
    public BattleBehaviour movement;
    public GameObject hand;
    public void OnClick()
    {
        movement = GameObject.Find("BattleSystem").GetComponent<BattleBehaviour>();
        if(changeTime1 && movement.player1Turn)
        {
            hand = GameObject.Find("Hand1");
            movement.Immovable(hand,changeTime1);
            changeTime1 = false;
            HideB1();
        }
        else if(changeTime2 && !movement.player1Turn)
        {
            hand = GameObject.Find("Hand2");
            movement.Immovable(hand,changeTime2);
            changeTime2 = false;
            HideB2();
        }
    }
    public void HideB1()
    {
        Transform button = GameObject.Find("ToChangeP1").transform;
        Vector3 pos = button.position;
        pos.z = -10;
        button.position = pos;
    }
    public void HideB2()
    {
        Transform button = GameObject.Find("ToChangeP2").transform;
        Vector3 pos = button.position;
        pos.z = -10;
        button.position = pos;
    }
    public void ShowB1()
    {
        Transform button = GameObject.Find("ToChangeP1").transform;
        Vector3 pos = button.position;
        pos.z = 0;
        button.position = pos;
    }
    public void ShowB2()
    {
        Transform button = GameObject.Find("ToChangeP2").transform;
        Vector3 pos = button.position;
        pos.z = 0;
        button.position = pos;
    }
}
