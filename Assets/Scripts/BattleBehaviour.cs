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
    public bool decoyTime;
    public bool team1;
    public bool team2;
    private int counter = 0;
    public int P1roundsWon;
    public int P2roundsWon;
    public GameObject player1Hand;
    public GameObject player2Hand;
    public Button leader1;
    public Button leader2;
    public Draw draw;
    public WinnerScreen winner;
    public ChangeCards time;
    public TMP_Text player1Points;
    public TMP_Text player2Points;
    [SerializeField] private Camera mainCamera;
    
    void Start()
    {
        EndTurn();
        Immovable(player1Hand,false);
        Immovable(player2Hand,false);
        time = GameObject.Find("ToChangeP1").GetComponent<ChangeCards>();
        time.changeTime1 = true;
        time.changeTime2 = true;
    }
    public void OnClick()
    {
        round =! round;
        player1Turn =! player1Turn;
        EndTurn();
        counter++;
        if(counter==2)
        {
            draw = GameObject.Find("BattleSystem").GetComponent<Draw>();
            for(int i=0;i<2;i++)
            {
                draw.Draw1();
                draw.Draw2();
            }
            winner = GameObject.Find("FinalPanel").GetComponent<WinnerScreen>();
            int p1points = int.Parse(player1Points.text);
            int p2points = int.Parse(player2Points.text);
            if(p1points > p2points)
            {
                P1roundsWon++;
                if(P1roundsWon ==1)
                {
                    Image roundColor = GameObject.Find("RoundC1").GetComponent<Image>();
                    roundColor.color = Color.green;
                }
                else if(P1roundsWon ==2)
                {
                    Image roundColor = GameObject.Find("RoundC2").GetComponent<Image>();
                    roundColor.color = Color.green;
                }
                winner.RoundShow("P1");
                player1Turn = false;
                EndTurn();
                Immovable(player1Hand,false);
                Immovable(player2Hand,false);
                time.changeTime1 = true;
                time.changeTime2 = true;
            }
            else if(p2points > p1points)
            {
                P2roundsWon++;
                if(P2roundsWon ==1)
                {
                    Image roundColor = GameObject.Find("RoundC3").GetComponent<Image>();
                    roundColor.color = Color.green;
                }
                else if(P2roundsWon ==2)
                {
                    Image roundColor = GameObject.Find("RoundC4").GetComponent<Image>();
                    roundColor.color = Color.green;
                }
                winner.RoundShow("P2");
                player1Turn = true;
                EndTurn();
                Immovable(player1Hand,false);
                Immovable(player2Hand,false);
                time.changeTime1 = true;
                time.changeTime2 = true;
            }
            else
            {
                P1roundsWon++;
                if(P1roundsWon ==1)
                {
                    Image roundColor = GameObject.Find("RoundC1").GetComponent<Image>();
                    roundColor.color = Color.green;
                }
                else if(P1roundsWon ==2)
                {
                    Image roundColor = GameObject.Find("RoundC2").GetComponent<Image>();
                    roundColor.color = Color.green;
                }
                P2roundsWon++;
                if(P2roundsWon ==1)
                {
                    Image roundColor = GameObject.Find("RoundC3").GetComponent<Image>();
                    roundColor.color = Color.green;
                }
                else if(P2roundsWon ==2)
                {
                    Image roundColor = GameObject.Find("RoundC4").GetComponent<Image>();
                    roundColor.color = Color.green;
                }
                winner.RoundShow("Tie");
                player1Turn = false;
                EndTurn();
                Immovable(player1Hand,false);
                Immovable(player2Hand,false);
                time.changeTime1 = true;
                time.changeTime2 = true;
            }
            if(P1roundsWon == 2)
            {
                winner.FinalShow("P1");
            }
            else if(P2roundsWon == 2)
            {
                winner.FinalShow("P2");
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
    public void Immovable(GameObject hand,bool movable)
    {
        DragAndDrop[] cards = hand.GetComponentsInChildren<DragAndDrop>();
        foreach(DragAndDrop card in cards)
        {
            card.enabled = movable;
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
        GameObject units1 = GameObject.Find("UnitsZone1");
        foreach(Transform zone in units1.transform)
        {
            foreach(Transform card in zone.transform)
            {
                Destroy(card.gameObject);
            }
        }
        GameObject units2 = GameObject.Find("UnitsZone2");
        foreach(Transform zone in units2.transform)
        {
            foreach(Transform card in zone.transform)
            {
                Destroy(card.gameObject);
            }
        }
        GameObject inspires1 = GameObject.Find("Inspires1");
        foreach(Transform zone in inspires1.transform)
        {
            foreach(Transform card in zone.transform)
            {
                Destroy(card.gameObject);
            }
        }
        GameObject inspires2 = GameObject.Find("Inspires2");
        foreach(Transform zone in inspires2.transform)
        {
            foreach(Transform card in zone.transform)
            {
                Destroy(card.gameObject);
            }
        }
        GameObject weather = GameObject.Find("Weather");
        foreach(Transform card in weather.transform)
        {
            Destroy(card.gameObject);
        }
    }
}