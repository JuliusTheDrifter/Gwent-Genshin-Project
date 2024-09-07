using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Xml;
using System;
public class Context
{
    public Dictionary<string,Card> cards = new Dictionary<string,Card>();
    public Dictionary<string,EffectNode> effects = new Dictionary<string,EffectNode>();
    public Dictionary<string,object> variables = new Dictionary<string,object>();
    public BattleBehaviour battleBehaviour;
    public void AddCard(string name)
    {
        if(cards.ContainsKey(name))
        {
            throw new Exception($"There is another card with the name: '{name}'.");
        }
        cards[name] = new Card();
    }
    public void AddEffect(string name)
    {
        if(effects.ContainsKey(name))
        {
            throw new Exception($"There is another card with the name: '{name}'.");
        }
        effects[name] = new EffectNode();
    }

    public EffectNode GetEffect(string name)
    {
        if(effects.ContainsKey(name))
        {
            return effects[name];
        }
        else throw new Exception($"There is no effect with the name: '{name}'.");;
    }

}