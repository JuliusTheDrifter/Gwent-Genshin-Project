public class Context
{
    public Dictionary<string,Card> cards = new Dictionary<string,Card>();
    public Dictionary<string,Effect> effects = new Dictionary<string,Effect>();

    public void AddCard(string name)
    {
        if(cards.ContainsKey(name))
        {
            throw new Exception($"Hay otra carta con el nombre: '{name}'.");
        }
        cards[name] = new Card();
    }
    public void AddEffect(string name)
    {
        if(effects.ContainsKey(name))
        {
            throw new Exception($"Hay otra carta con el nombre: '{name}'.");
        }
        effects[name] = new Effect();
    }

    public Effect GetEffect(string name)
    {
        if(effects.ContainsKey(name))
        {
            return effects[name];
        }
        else throw new Exception($"There is no effect with the name: '{name}'.");;
    }

}