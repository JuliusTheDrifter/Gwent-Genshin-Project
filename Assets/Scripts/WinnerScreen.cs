using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using TMPro;
//This scrip manages the screen that will be shown when someone wins a round or the game
public class WinnerScreen : MonoBehaviour
{
    GameObject panel;
    TextMeshProUGUI winnerText;
    private bool game;
    void Start()
    {
        OnClick();
    }
    //Shows who won the round
    public void RoundShow(string winner)
    {
        panel = GameObject.Find("FinalPanel");
        winnerText = GameObject.Find("Winner").GetComponent<TextMeshProUGUI>();
        Vector3 pos = panel.transform.position;
        pos.z = 0;
        panel.transform.position = pos;
        if(winner == "P1")
        {
            panel.transform.rotation = Quaternion.Euler(0,0,0);
            winnerText.text = "Player 1 wins the round";
        }
        else
        {
            panel.transform.rotation = Quaternion.Euler(180,180,0);
            winnerText.text = "Player 2 wins the round";
        }
    }
    //Shows who won the game
    public void FinalShow(string winner)
    {
        panel = GameObject.Find("FinalPanel");
        winnerText = GameObject.Find("Winner").GetComponent<TextMeshProUGUI>();
        winnerText.text = winner;
        Vector3 pos = panel.transform.position;
        pos.z = 0;
        panel.transform.position = pos;
        if(winner == "P1")
        {
            winnerText.text = "Player 1 wins the GAME";
            game = true;
        }
        else
        {
            winnerText.text = "Player 2 wins the GAME";
            game = true;
        }
    }
    //Hides the panel on click
    public void OnClick()
    {
        panel = GameObject.Find("FinalPanel");
        Vector3 pos = panel.transform.position;
        pos.z = -10;
        panel.transform.position = pos;
        if(game)
        {
            game = false;
            SceneManager.LoadScene("Menu");
        }
    }
}
