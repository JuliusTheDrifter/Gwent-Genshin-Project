using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadInput : MonoBehaviour
{
    private string Input;
    public void ReadStringInput(string input)
    {
        Input = input;
        Debug.Log(Input);
    }
}
