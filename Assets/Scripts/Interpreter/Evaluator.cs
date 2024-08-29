using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

public class Evaluator
{
    Card Card;
    Context Context;
    public Evaluator(Card card,Context context)
    {
        Card = card;
        Context = context;
        UnityEngine.Debug.Log("Creado");
    }

    public void EvaluateEffects()
    {
        UnityEngine.Debug.Log("Empezamos");
        foreach(var oAElement in Card.effects.Elements)
        {
            EvaluateOAE(oAElement);
        }
        UnityEngine.Debug.Log("Terminamos");
    }
    
    void EvaluateOAE(OnActivationElements oAElement)
    {
        UnityEngine.Debug.Log("OAEfecto");
        EvaluateOAEffect(oAElement.OAEffect,oAElement.Selector);
        if(oAElement.postActions != null)
        {
            foreach(var postAction in oAElement.postActions)
            {
                EvaluatePostActions(postAction,oAElement.Selector.Source);
            }
        }
        UnityEngine.Debug.Log("Fin de efecto");
    }

    void EvaluateOAEffect(OAEffect oAEffect,Selector selector)
    {
        UnityEngine.Debug.Log("Efecto real");
        EffectNode effect = Context.effects[oAEffect.Name];
        foreach(var assignment in oAEffect.Assingments)
        {
            Context.variables[assignment.Left.Value] = assignment.Right.Evaluate(Context); 
        }
        if(selector != null)
        {
            Context.variables["targets"] = EvaluateSelector(selector);
            EvaluateAction(effect.Action);
            Context.variables.Remove("targets");
        }
        else
        {
            EvaluateAction(effect.Action);
        }
        UnityEngine.Debug.Log("Fin de efecto real");
    }

    void EvaluatePostActions(PostAction postAction,string source)
    {
        EffectNode effect = Context.effects[(string)postAction.Type.Evaluate(Context)];
        foreach(var assignment in postAction.Assingments)
        {
            Context.variables[assignment.Left.Value] = assignment.Right.Evaluate(Context); 
        }
        if(postAction.Selector != null)
        {
            Context.variables["targets"] = EvaluateSelector(postAction.Selector,source);
            EvaluateAction(effect.Action);
            Context.variables.Remove("targets");
        }
        else
        {
            EvaluateAction(effect.Action);
        }
    }

    List<Card> EvaluateSelector(Selector selector, string source = null)
    {
        List<Card> cards = new List<Card>();
        if(selector.Source == "parent") cards = EvaluateSource(source);
        else cards = EvaluateSource(selector.Source);
        cards.Where(EvaluatePredicate(selector.Predicate)).ToList(); //To be fixed
        if(selector.Single.Value)
        {
            List<Card> result = new List<Card>{cards[0]};
            return result;
        }
        else
        {
            return cards;
        }
    }

    List<Card> EvaluateSource(string source)
    {
        switch(source)
        {
            case "hand": return Context.battleBehaviour.HandOfPlayer(Context.battleBehaviour.TriggerPlayer()).Cards;
            case "otherHand":if(Context.battleBehaviour.TriggerPlayer()==2) return Context.battleBehaviour.HandOfPlayer(1).Cards;
            else return Context.battleBehaviour.HandOfPlayer(2).Cards;
            case "deck": return Context.battleBehaviour.DeckOfPlayer(Context.battleBehaviour.TriggerPlayer()).Cards;
            case "otherDeck":if(Context.battleBehaviour.TriggerPlayer()==2) return Context.battleBehaviour.DeckOfPlayer(1).Cards;
            else return Context.battleBehaviour.DeckOfPlayer(2).Cards;
            case "field": return Context.battleBehaviour.FieldOfPlayer(Context.battleBehaviour.TriggerPlayer()).Cards;
            case "otherField":if(Context.battleBehaviour.TriggerPlayer()==2) return Context.battleBehaviour.FieldOfPlayer(1).Cards;
            else return Context.battleBehaviour.FieldOfPlayer(2).Cards;
            default: return Context.battleBehaviour.Board();
        }
    }

    Func<Card,bool> EvaluatePredicate(Predicate predicate) //Wrong
    {
        return card =>
        {
            Context.variables[predicate.Var.Value] = card;
            return (bool)predicate.Condition.Evaluate(Context);
        };
    }

    void EvaluateAction(Action action)
    {
        ExecuteStmtBlock(action.Block);
    }

    void ExecuteStmtBlock(StmsBlock stmsBlock)
    {
        foreach(var stm in stmsBlock.statements)
        {
            stm.Execute(Context);
        }
        UnityEngine.Debug.Log("aaaaaaaaaaa");
    }

}