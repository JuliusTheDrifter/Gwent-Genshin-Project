using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CardDisplay : MonoBehaviour //This script is the information that will be shown in the UI
{
    public int team;
    public int points;
    public string type;
    public string faction;
    public string[] range = new string[3];
    public bool isEnhanced;    
    public Card card;
    public TMP_Text nameText;
    public TMP_Text descriptionText;
    public TMP_Text pointsText;
    public Image artworkImage;
    public Image typeImage;
    public Image auraImage;
    void Start() //This is to show the info in the UI
    {
        artworkImage.sprite = card.artwork;
        auraImage.sprite = card.aura;                                                                                                                                                                                                                                                                                                                                                                                                                    
        typeImage.sprite = card.posSprite;
        nameText.text = card.name;
        descriptionText.text = card.description;
        points = card.points;
        type = card.type;
        faction = card.faction;
        range = card.range;
        team = card.Owner;
        if(type == "Oro" || type == "Plata")
        {
            pointsText.text = points.ToString();
        }
    }
    public void UpdateDisplay()
    {
        if(type == "Oro" || type == "Plata")
        {
            pointsText.text = points.ToString();
        }
    }
}