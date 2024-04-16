using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CardDisplay : MonoBehaviour
{
   public string position;
    public int team;
    public int points;
    public Card card;
    public TMP_Text nameText;
    public TMP_Text descriptionText;
    public TMP_Text pointsText;
    public Image artworkImage;
    public Image typeImage;
    public Image auraImage;
    void Start()
    {
        artworkImage.sprite = card.artwork;
        auraImage.sprite = card.aura;                                                                                                                                                                                                                                                                                                                                                                                                                    
        typeImage.sprite = card.type;
        nameText.text = card.name;
        descriptionText.text = card.description;
        points = card.points;
        pointsText.text = points.ToString();
    }
}