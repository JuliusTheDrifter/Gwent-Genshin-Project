using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public enum BattleState {START,PLAYER1TURN,PLAYER2TURN,WON,LOST}
public class BattleBehaviour : MonoBehaviour
{
    public bool player1Turn;
    public bool round;
    private int counter = 0;
    public GameObject player1Hand;
    public GameObject player2Hand;
    public Button leader1;
    public Button leader2;
    public TMP_Text player1Points;
    public TMP_Text player2Points;
    [SerializeField] private Camera mainCamera;
    
    void Start()
    {
        EndTurn();
    }
    public void OnClick()
    {
        round =! round;
        player1Turn =! player1Turn;
        EndTurn();
        counter += 1;
        if(counter==2)
        {
            int p1points = int.Parse(player1Points.text);
            int p2points = int.Parse(player2Points.text);
            if(p1points > p2points)
            {

            }
            else
            {

            }
            CleanField();
            counter = 0;
        }
    }
    public void EndTurn()
    {
        if(!round)
        {
            player1Turn =! player1Turn;
        }
        if(player1Turn)
        {
            mainCamera.transform.rotation = UnityEngine.Quaternion.Euler(0,0,0);
            Visibility(player1Hand,true);
            NoInteractions(leader1,true);
            Visibility(player2Hand,false);
            NoInteractions(leader2,false);
        }
        else
        {
            mainCamera.transform.rotation = UnityEngine.Quaternion.Euler(180,180,0);
            Visibility(player1Hand,false);
            Pose(player2Hand);
            NoInteractions(leader1,false);
            Visibility(player2Hand,true);
            NoInteractions(leader2,true);
        }
    }
    public void NoInteractions(Button leader,bool  active)
    {
        if(active)
        {
            leader.interactable = active;
        }
        else
        {
            leader.interactable = active;
        }
    }
    public void  Visibility(GameObject playerHand,bool visible)
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
    public void Pose(GameObject playerHand)
    {
        UnityEngine.Quaternion pos = transform.rotation;
        foreach(Transform card in playerHand.transform)
        {
            pos = card.transform.rotation;
            pos = UnityEngine.Quaternion.Euler(180f,180f,0);
            card.transform.rotation = pos;
        }
    }
    void CleanField()
    {
        GameObject field = GameObject.Find("Board");
        foreach(Transform zone in field.transform)
        {
            foreach(Transform card in zone.transform)
            {
                Destroy(card.gameObject);
            }
        }
    }
}