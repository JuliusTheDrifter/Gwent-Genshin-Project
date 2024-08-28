using System;
using System.Collections.Generic;
using System.Linq;

public class Evaluator
{
    BattleBehaviour battleBehaviour;
    Card Card;
    Context Context;
    public Evaluator(Card card,Context context)
    {
        Card = card;
        Context = context;
    }

    public void EvaluateEffects()
    {
        foreach(var oAElement in Card.effects.Elements)
        {
            EvaluateOAE(oAElement);
        }
    }
    
    void EvaluateOAE(OnActivationElements oAElement)
    {
        EvaluateOAEffect(oAElement.OAEffect,oAElement.Selector);
        if(oAElement.postActions != null)
        {
            foreach(var postAction in oAElement.postActions)
            {
                EvaluatePostActions(postAction,oAElement.Selector.Source);
            }
        }
    }

    void EvaluateOAEffect(OAEffect oAEffect,Selector selector)
    {
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
            case "hand": return battleBehaviour.HandOfPlayer(battleBehaviour.TriggerPlayer()).Cards;
            case "otherHand":if(battleBehaviour.TriggerPlayer()==2) return battleBehaviour.HandOfPlayer(1).Cards;
            else return battleBehaviour.HandOfPlayer(2).Cards;
            case "deck": return battleBehaviour.DeckOfPlayer(battleBehaviour.TriggerPlayer()).Cards;
            case "otherDeck":if(battleBehaviour.TriggerPlayer()==2) return battleBehaviour.DeckOfPlayer(1).Cards;
            else return battleBehaviour.DeckOfPlayer(2).Cards;
            case "field": return battleBehaviour.FieldOfPlayer(battleBehaviour.TriggerPlayer()).Cards;
            case "otherField":if(battleBehaviour.TriggerPlayer()==2) return battleBehaviour.FieldOfPlayer(1).Cards;
            else return battleBehaviour.FieldOfPlayer(2).Cards;
            default: return battleBehaviour.Board();
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
    }
}