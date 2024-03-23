using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class Draw : MonoBehaviour
{
    public GameObject Card;
    public GameObject Hand;
    public void OnClick()
    {
        int randomIndex = Random.Range(0, CardDataBase.cardList.Count);
        GameObject playercard = Instantiate(CardDataBase.cardList[randomIndex], new Vector3(0, 0, 0), Quaternion.identity);
        playercard.transform.SetParent(Hand.transform, false);

        CardDataBase.cardList.RemoveAt(randomIndex);
        /*GameObject playercard = Instantiate(CardDataBase.cardList[Random.Range(0,CardDataBase.cardList.Count)],new Vector3(0,0,0), Quaternion.identity);
        playercard.transform.SetParent(Hand.transform, false);*/ 
    }
}
//CardDataBase.cardList.Remove(playercard);