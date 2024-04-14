using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;
using TMPro;
public class CardDataBase : MonoBehaviour
{
    public int count;
    public TMP_Text deckCount;
    public static List<GameObject> cardList = new List<GameObject>();
    void Awake()
    {
        /*cardList.Add(new Card("None",0,"None","None",0,Resources.Load<Sprite>(""),Resources.Load<Sprite>(""),Resources.Load<Sprite>("")));
        cardList.Add(new Card("Monstad",1,"Leader","The city of freedom",0,Resources.Load<Sprite>("Wind and Freedom"),Resources.Load<Sprite>(""),Resources.Load<Sprite>("")));
        cardList.Add(new Card("Diluc",2,"Melee","I'm Batman",9,Resources.Load<Sprite>("Diluc"),Resources.Load<Sprite>("Icon_Sword"),Resources.Load<Sprite>("Gold")));
        cardList.Add(new Card("Eula",3,"Siege","I will have vengeance!",8,Resources.Load<Sprite>(""),Resources.Load<Sprite>(""),Resources.Load<Sprite>("")));
        cardList.Add(new Card("Klee",4,"Siege","Explosion!",7,Resources.Load<Sprite>(""),Resources.Load<Sprite>(""),Resources.Load<Sprite>("")));
        cardList.Add(new Card("Fischl",5,"Ranged","",6,Resources.Load<Sprite>(""),Resources.Load<Sprite>(""),Resources.Load<Sprite>("")));
        cardList.Add(new Card("Kaeya",6,"Ranged","Calvary captain of the Knigths of Favonius",7,Resources.Load<Sprite>(""),Resources.Load<Sprite>(""),Resources.Load<Sprite>("")));
        cardList.Add(new Card("Bennet",7,"Melee","Bennett's adventure team to the rescue!",5,Resources.Load<Sprite>(""),Resources.Load<Sprite>(""),Resources.Load<Sprite>("")));
        cardList.Add(new Card("Jean",8,"Melee","Grand Master of the Knigths of Favonius",7,Resources.Load<Sprite>(""),Resources.Load<Sprite>(""),Resources.Load<Sprite>("")));
        cardList.Add(new Card("Leave it to me!",9,"Support","None",0,Resources.Load<Sprite>(""),Resources.Load<Sprite>(""),Resources.Load<Sprite>("")));
        cardList.Add(new Card("Strategize",10,"Support","None",0,Resources.Load<Sprite>(""),Resources.Load<Sprite>(""),Resources.Load<Sprite>("")));
        cardList.Add(new Card("Food",11,"Support","None",0,Resources.Load<Sprite>(""),Resources.Load<Sprite>(""),Resources.Load<Sprite>("")));
        cardList.Add(new Card("Special cards?",12,"Support","None",0,Resources.Load<Sprite>(""),Resources.Load<Sprite>(""),Resources.Load<Sprite>("")));
        cardList.Add(new Card("Earthquake",13,"Field","None",0,Resources.Load<Sprite>(""),Resources.Load<Sprite>(""),Resources.Load<Sprite>("")));
        cardList.Add(new Card("Flood",14,"Field","None",0,Resources.Load<Sprite>(""),Resources.Load<Sprite>(""),Resources.Load<Sprite>("")));
        cardList.Add(new Card("Blizzard",15,"Field","None",0,Resources.Load<Sprite>(""),Resources.Load<Sprite>(""),Resources.Load<Sprite>("")));
        cardList.Add(new Card("Blessing of the Divine",16,"Clear","None",0,Resources.Load<Sprite>(""),Resources.Load<Sprite>(""),Resources.Load<Sprite>("")));
        cardList.Add(new Card("Another clear yet to define?",17,"Clear","None",0,Resources.Load<Sprite>(""),Resources.Load<Sprite>(""),Resources.Load<Sprite>("")));
        cardList.Add(new Card("Katherine",18,"Substitute","None",0,Resources.Load<Sprite>(""),Resources.Load<Sprite>(""),Resources.Load<Sprite>("")));
        cardList.Add(new Card("Liben",19,"Substitute","None",0,Resources.Load<Sprite>(""),Resources.Load<Sprite>(""),Resources.Load<Sprite>("")));*/
        for(int i=0;i<10;i++)
        cardList.Add(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/CardsPrefab/Diluc.prefab"));
        cardList.Add(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/CardsPrefab/Eula.prefab"));
        Debug.Log($"La cantidad de cartas en el deck es de: {cardList.Count}");
    }
    /*void Start()
    {
        deckCount.text = cardList.Count.ToString();
    }*/
}
