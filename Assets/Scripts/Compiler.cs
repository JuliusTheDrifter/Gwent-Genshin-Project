using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Numerics;
using Unity.VisualScripting;

public class Compiler : MonoBehaviour
{
    public TMP_Text myTextMeshPro;
    public GameObject panel;
    public GameObject scroll;
    public GameObject prefab;
    public Hand hand;
    public Card card;
    public Transform canvas;

    public void Compile(TMP_Text myTextMeshPro)
    {
        string input = myTextMeshPro.text;
        List<string> errors = new List<string>();
        Lexer lexer = new Lexer(input,errors);
        List<Token> tokens = lexer.ScanTokens();
        errors.RemoveAt(errors.Count-1);
        if(errors.Count != 0)
        {
            string joinedText = string.Join("\n", errors);
            PrintErrors(joinedText);
            return;
        }
        Parser parser = new Parser(tokens);
        Node node = parser.Parse();
        if(parser.Ex != null)
        {
            PrintErrors(parser.Ex.ToString());
            return;
        }
        Context context = new Context();
        context.battleBehaviour = GameObject.Find("BattleSystem").GetComponent<BattleBehaviour>();
        SemanticalCheck semanticalCheck = new SemanticalCheck(node,context,errors);
        if(errors.Count != 0)
        {
            string joinedText = string.Join("\n", errors);
            PrintErrors(joinedText);
            return;
        }
        foreach(CardNode card in (node as Program).CardNodes)
        {
            SpawnCard(card,context);
        }
    }
    void PrintErrors(string joinedText)
    {
        myTextMeshPro.text += joinedText;
        TogglePanelVisibility();
    }
    
    public void TogglePanelVisibility()
    {
        bool isActive = panel.activeSelf;
        panel.SetActive(!isActive);
    }

    public void ToggleEditPanel()
    {
        bool isActive = scroll.activeSelf;
        scroll.SetActive(!isActive);
    }

    void SpawnCard(CardNode cardnode, Context context)
    {
        Card card = new Card();
        card.name = cardnode.Name.name.Evaluate(new Context()).ToString();
        card.type = cardnode.Type.type.Evaluate(new Context()).ToString();
        card.faction = cardnode.Faction.faction.Evaluate(new Context()).ToString();
        card.points = (int)cardnode.Power.power.Evaluate(new Context());
        card.context = context;
        int i=0;
        foreach(var zone in cardnode.Range.range)
        {
            card.range[i++] = zone.Evaluate(new Context()) as string;
        }
        card.effects = cardnode.OnActivation;
        card.prefab = prefab;
        card.Owner = 1;
        card.effectText = "a";
        GameObject gameObject = Instantiate(prefab);
        gameObject.GetComponent<CardDisplay>().card = card;
        gameObject.GetComponent<CardDisplay>().team = 1;
        hand.Push(gameObject);
    }
}
