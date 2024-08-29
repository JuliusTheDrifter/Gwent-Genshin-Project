using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BattleBehaviour : MonoBehaviour //This is the scrip where I manage the turns and rounds
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
    public GameObject Units1;
    public GameObject Units2;
    public GameObject Inspires1;
    public GameObject Inspires2;
    public Button leader1;
    public Button leader2;
    public Draw draw;
    public WinnerScreen winner;
    public ChangeCards time1;
    public ChangeCards time2;
    public TMP_Text player1Points;
    public TMP_Text player2Points;
    public TMP_Text GraveYard1;
    public TMP_Text GraveYard2;
    [SerializeField] private Camera mainCamera;
    
    void Start()
    {
        //Here each player draw 10 cards at the start of the game
        //Player 1 draws 1 more card than player 2 because of his leader effect
        for(int i=0;i<11;i++)
        {
            draw.DrawCard(1);
        }
        for(int i=0;i<10;i++)
        {
            draw.DrawCard(2);
        }
        NoInteractions(leader1,false);
        NoInteractions(leader2,false);
        EndTurn();
        //Here the script ChangeCards is used only at the first round for the player to change a maximun of 2 cards
        time1 = GameObject.Find("ToChangeP1").GetComponent<ChangeCards>();
        time2 = GameObject.Find("ToChangeP2").GetComponent<ChangeCards>();
        time1.changeTime = true;
        time2.changeTime = true;
        time2.Hide();
        Immovable(player1Hand,false);
        Immovable(player2Hand,false);
    }

    public int TriggerPlayer()
    {
        if(player1Turn)
        {
            return 1;
        }
        else
        {
            return 2;
        }
    }

    public Hand HandOfPlayer(int player) //*
    {
        if(player == 1)
        {
            return GameObject.Find("Hand1").GetComponent<Hand>();
        }
        else
        {
            return GameObject.Find("Hand2").GetComponent<Hand>();
        }
    }

    public Deck DeckOfPlayer(int player) //*
    {
        if(player == 1)
        {
            return GameObject.Find("DeckManager1").GetComponent<Deck>();
        }
        else
        {
            return GameObject.Find("DeckManager2").GetComponent<Deck>();
        }
    }

    public Field FieldOfPlayer(int player) //*
    {
        if(player == 1)
        {
            return GameObject.Find("UnitsZone1").GetComponent<Field>();
        }
        else
        {
            return GameObject.Find("UnitsZone2").GetComponent<Field>();
        }
    }

    public Graveyard GraveYardOfPlayer(int player) //*
    {
        if(player == 1)
        {
            return GameObject.Find("Graveyard1").GetComponent<Graveyard>();
        }
        else
        {
            return GameObject.Find("Graveyard2").GetComponent<Graveyard>();
        }
    }

    public List<Card> Board() //*
    {
        List<Card> cards = new List<Card>();
        foreach(Transform zone in Units1.transform)
        {
            foreach(Transform card in zone.transform)
            {
                cards.Add(card.GetComponent<CardDisplay>().card);
            }
        }
        foreach(Transform zone in Units2.transform)
        {
            foreach(Transform card in zone.transform)
            {
                cards.Add(card.GetComponent<CardDisplay>().card);
            }
        }
        foreach(Transform zone in Inspires1.transform)
        {
            foreach(Transform card in zone.transform)
            {
                cards.Add(card.GetComponent<CardDisplay>().card);
            }
        }
        foreach(Transform zone in Inspires2.transform)
        {
            foreach(Transform card in zone.transform)
            {
                cards.Add(card.GetComponent<CardDisplay>().card);
            }
        }
        return cards;
    }

    public void OnClick() //This method is to manage the rounds
    {
        if(player1Turn)
        {
            if(time1.changeTime)
            {
                Immovable(player1Hand,true);
                time1.changeTime = false;
                time1.Hide();
            }
        }
        else if(!player1Turn)
        {
            if(time2.changeTime)
            {
                Immovable(player2Hand,true);
                time2.changeTime = false;
                time2.Hide();
            } 
        }
        round =! round;
        player1Turn =! player1Turn;
        EndTurn();
        counter++;
        if(counter==2)
        {
            draw = GameObject.Find("BattleSystem").GetComponent<Draw>();
            for(int i=0;i<2;i++) //Each player draws 2 cards at the start of each round
            {
                draw.DrawCard(1);
                draw.DrawCard(2);
            }
            winner = GameObject.Find("FinalPanel").GetComponent<WinnerScreen>();
            int p1points = int.Parse(player1Points.text);
            int p2points = int.Parse(player2Points.text);
            if(p1points > p2points) //Here the winner of the round is chosen
            {
                P1roundsWon++;
                if(P1roundsWon ==1)
                {
                    Image roundColor = GameObject.Find("RoundC1").GetComponent<Image>();
                    roundColor.color = Color.green; //This turns the red circules to green
                }
                else if(P1roundsWon ==2)
                {
                    Image roundColor = GameObject.Find("RoundC2").GetComponent<Image>();
                    roundColor.color = Color.green;
                }
                winner.RoundShow("P1");
                player1Turn = false;
                EndTurn();
            }
            else if(p2points >= p1points)
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
            }
            //Here the winner of the game is chosen
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

    public void EndTurn() //This method changes turns every time is called
    {
        if(!round)
        {
            player1Turn =! player1Turn;
        }
        if(player1Turn)
        {
            mainCamera.transform.rotation = UnityEngine.Quaternion.Euler(0,0,0); //Put the camera back to normal
            Visibility(player1Hand,true);
            Visibility(player2Hand,false);
        }
        else
        {
            if(time2.changeTime)
            {
                UnityEngine.Vector3 pos = time2.gameObject.transform.position;
                pos.z = 0;
                time2.gameObject.transform.position = pos;
            }
            mainCamera.transform.rotation = UnityEngine.Quaternion.Euler(180,180,0); //Rotates the camera
            Visibility(player1Hand,false);
            Pose(player2Hand);
            Visibility(player2Hand,true);
        }
    }

    public void NoInteractions(Button leader,bool  active) //This method disables/enables the Leader button
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

    //This method disable/enable the Drag method so the cards can't/can be movable
    public void Immovable(GameObject hand,bool movable)
    {
        DragAndDrop[] cards = hand.GetComponentsInChildren<DragAndDrop>();
        foreach(DragAndDrop card in cards)
        {
            card.enabled = movable;
        }
    }

    //This method hides/shows the cards from the hand by putting each card in -10/0 in the z axis
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

    public void Pose(GameObject playerHand) //This method rotates the player2 cards
    {
        UnityEngine.Quaternion pos = transform.rotation;
        foreach(Transform card in playerHand.transform)
        {
            pos = card.transform.rotation;
            pos = UnityEngine.Quaternion.Euler(180f,180f,0);
            card.transform.rotation = pos;
        }
    }

    void CleanField() //This method eliminates the cards from the field
    {
        int deathcard1 = 0;
        int deathcard2 = 0;
        GameObject units1 = GameObject.Find("UnitsZone1");
        foreach(Transform zone in units1.transform)
        {
            foreach(Transform card in zone.transform)
            {
                deathcard1++;
                Destroy(card.gameObject);
            }
        }
        GameObject units2 = GameObject.Find("UnitsZone2");
        foreach(Transform zone in units2.transform)
        {
            foreach(Transform card in zone.transform)
            {
                deathcard2++;
                Destroy(card.gameObject);
            }
        }
        GameObject inspires1 = GameObject.Find("Inspires1");
        foreach(Transform zone in inspires1.transform)
        {
            foreach(Transform card in zone.transform)
            {
                deathcard1++;
                Destroy(card.gameObject);
            }
        }
        GameObject inspires2 = GameObject.Find("Inspires2");
        foreach(Transform zone in inspires2.transform)
        {
            foreach(Transform card in zone.transform)
            {
                deathcard2++;
                Destroy(card.gameObject);
            }
        }
        GameObject weather = GameObject.Find("Weather");
        foreach(Transform card in weather.transform)
        {
            CardDisplay cardDisplay = card.gameObject.GetComponent<CardDisplay>();
            if(cardDisplay.team == 1) deathcard1++;
            else deathcard2++;

            Destroy(card.gameObject);
        }
        //To show the amount of cards that were eliminated
        GraveYard1.text = deathcard1.ToString();
        GraveYard2.text = deathcard2.ToString();
    }

}