using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Numerics;

public class Compiler : MonoBehaviour
{
    public TMP_Text myTextMeshPro;
    public GameObject panel;
    public Button button;
    public GameObject prefab;
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
        SemanticalCheck semanticalCheck = new SemanticalCheck(node,context,errors);
        if(errors.Count != 0)
        {
            string joinedText = string.Join("\n", errors);
            PrintErrors(joinedText);
            return;
        }
        
    }
    void PrintErrors(string joinedText)
    {
        myTextMeshPro.text += joinedText;
        TogglePanelVisibility();
    }
    
    void TogglePanelVisibility()
    {
        bool isActive = panel.activeSelf;
        panel.SetActive(!isActive);
    }
    void SpawnCard()
    {
        UnityEngine.Vector3 vector3 = new UnityEngine.Vector3(342, 40, 0);
        Instantiate(prefab,vector3,UnityEngine.Quaternion.identity).transform.SetParent(canvas,false);
    }
}
