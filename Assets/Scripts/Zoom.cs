using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class Zoom : MonoBehaviour
{
    public GameObject Canvas;
    public GameObject zoomCard;
    private Vector2 zoomScale = new Vector2(3,3);
    public void Awake()
    {
        Canvas = GameObject.Find("Board");
    }
    public void OnMouseEnter()
    {
        zoomCard = Instantiate(gameObject,new Vector2(210,725), Quaternion.identity);
        zoomCard.transform.SetParent(Canvas.transform,false);
        zoomCard.transform.localScale = zoomScale;
    }
    public void OnMouseExit()
    {
        Destroy(zoomCard);
    } 
}
