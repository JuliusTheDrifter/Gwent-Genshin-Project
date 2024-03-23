using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    private RectTransform rectTransform;
    private bool isDragging;
    private Vector2 startPosition;
    public GameObject Canvas;
    public GameObject DropZone;
    private GameObject startParent;
    private GameObject dropZone;
    private bool IsOverDropZone;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    void Start()//new
    {
        Canvas = GameObject.Find("Board");
        DropZone = GameObject.Find("Area");
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        IsOverDropZone = true;
        dropZone = collision.gameObject;
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        IsOverDropZone = false;
        dropZone = null;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = transform.position;
        isDragging = true;
            
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += 3 * eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        if(IsOverDropZone)
        {
            transform.SetParent(dropZone.transform, false);
            this.enabled = false;
        }
        else
        {
            transform.position = startPosition;
            transform.SetParent(startParent.transform, false);
        }
    }
}
