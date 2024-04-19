using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//This script is for the button that takes part in the dinamic of changing 2 cards at the beggining of the first round
public class ChangeCards : MonoBehaviour
{
    public bool changeTime;
    public BattleBehaviour movement;
    public GameObject hand;
    public int counter;
    public void OnClick()
    {
        movement = GameObject.Find("BattleSystem").GetComponent<BattleBehaviour>();
        if(changeTime == true)
        {
            movement.Immovable(hand,true);
            Hide();
            changeTime = false;
        }
    }
    public void Hide() //This method hides the button by changing it's position in the z axis
    {
        Vector3 pos = transform.position;
        pos.z = -10;
        transform.position = pos;
    }
}
