using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public void Hide()
    {
        Vector3 pos = transform.position;
        pos.z = -10;
        transform.position = pos;
    }
}
