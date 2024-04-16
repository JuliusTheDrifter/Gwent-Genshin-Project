using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Collections;

public class PointSystem : MonoBehaviour
{
   public TMP_Text player1Points;
   public TMP_Text player2Points;
   public GameObject Melee1;
   public GameObject Melee2;
   public GameObject Ranged1;
   public GameObject Ranged2;
   public GameObject Siege1;
   public GameObject Siege2;
   void Update()
   {
      CollectP1Points();
      CollectP2Points();
   }
   void CollectP1Points()
   {
      int mpoints = 0;
      int rpoints = 0;
      int spoints = 0;
      CardDisplay[] cards = Melee1.GetComponentsInChildren<CardDisplay>();
      foreach(var card in cards)
      {
         mpoints += card.points;
      }
      CardDisplay[] cards1 = Ranged1.GetComponentsInChildren<CardDisplay>();
      foreach(var card in cards1)
      {
         rpoints += card.points;
      }
      CardDisplay[] cards2 = Siege1.GetComponentsInChildren<CardDisplay>();
      foreach(var card in cards2)
      {
         spoints += card.points;
      }
      int totalpoints = mpoints + rpoints + spoints;
      player1Points.text = totalpoints.ToString();
   }
   void CollectP2Points()
   {
      int mpoints = 0;
      int rpoints = 0;
      int spoints = 0;
      CardDisplay[] cards = Melee2.GetComponentsInChildren<CardDisplay>();
      foreach(var card in cards)
      {
         mpoints += card.points;
      }
      CardDisplay[] cards1 = Ranged2.GetComponentsInChildren<CardDisplay>();
      foreach(var card in cards1)
      {
         rpoints += card.points;
      }
      CardDisplay[] cards2 = Siege2.GetComponentsInChildren<CardDisplay>();
      foreach(var card in cards2)
      {
         spoints += card.points;
      }
      int totalpoints = mpoints + rpoints + spoints;
      player2Points.text = totalpoints.ToString();
   }
}
