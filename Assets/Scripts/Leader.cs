using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leader : MonoBehaviour
{
    public BattleBehaviour endTurn;
    public void OnClick()
    {
        endTurn.EndTurn();
    }
}
