using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CardDisplay : MonoBehaviour //This script is the information that will be shown in the UI
{
    public string position;
    public int team;
    public int points;
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
        //This is to only show the points of the units cards
        if(position == "Melee1"||position == "Melee2"||position == "Ranged1"||position == "Ranged2"||position == "Siege1"||position == "Siege2")
        {
            pointsText.text = points.ToString();
        }
    }
}