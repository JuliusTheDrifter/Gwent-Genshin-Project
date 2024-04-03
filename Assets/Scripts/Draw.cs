using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class Draw : MonoBehaviour
{
    public GameObject Card;
    public GameObject Hand1;
    public GameObject Hand2;
    void Start()
    {
        for(int i=0;i<10;i++)
        {
            OnClick1();
            OnClick2();
        }
    }
    public void OnClick1()
    {
        int randomIndex = Random.Range(0, CardDataBase.cardList.Count);
        GameObject playercard = Instantiate(CardDataBase.cardList[randomIndex], new Vector3(0, 0, 0), Quaternion.identity);
        playercard.transform.SetParent(Hand1.transform, false);


        //CardDataBase.cardList.RemoveAt(randomIndex);
        /*GameObject playercard = Instantiate(CardDataBase.cardList[Random.Range(0,CardDataBase.cardList.Count)],new Vector3(0,0,0), Quaternion.identity);
        playercard.transform.SetParent(Hand.transform, false);*/ 
    }
    public void OnClick2()
    {
        int randomIndex = Random.Range(0, CardDataBase.cardList.Count);
        GameObject playercard = Instantiate(CardDataBase.cardList[randomIndex], new Vector3(0, 0, 0), Quaternion.identity);
        playercard.transform.SetParent(Hand2.transform, false);

        //CardDataBase.cardList.RemoveAt(randomIndex);
        /*GameObject playercard = Instantiate(CardDataBase.cardList[Random.Range(0,CardDataBase.cardList.Count)],new Vector3(0,0,0), Quaternion.identity);
        playercard.transform.SetParent(Hand.transform, false);*/ 
    }

}
//CardDataBase.cardList.Remove(playercard);