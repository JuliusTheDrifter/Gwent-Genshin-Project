using System;
using System.Collections.Generic;
using UnityEngine;
public interface Node
{
    public void Print(int indent = 0);
}

public class Program : Node
{
    public List<CardNode> CardNodes;
    public List<EffectNode> EffectNodes;
    public Program()
    {
        CardNodes = new List<CardNode>();
        EffectNodes = new List<EffectNode>();
    }
    public void Print(int indent = 0)
    {

        Debug.Log(new string(' ', indent) + "Program:");
        foreach (var card in CardNodes)
        {
            card.Print(indent + 2);
        }
        foreach (var effect in EffectNodes)
        {
            effect.Print(indent + 2);
        }
    }
}

public class CardNode : Node
{
    public CardType Type;
    public Name Name;
    public Faction Faction;
    public Power Power;
    public Range Range;
    public OnActivation OnActivation;
    public CardNode()
    {
        
    }
    public CardNode(CardType type,Name name,Faction faction,Power power,Range range,OnActivation onActivation)
    {
        Type = type;
        Name = name;
        Faction = faction;
        Power = power;
        Range = range;
        OnActivation = onActivation;
    }
    public void Print(int indent = 0)
    {

        Debug.Log(new string(' ', indent) + "Card:");
        Type?.Print(indent + 2);
        Name?.Print(indent + 2);
        Faction?.Print(indent + 2);
        Power?.Print(indent + 2);
        Range?.Print(indent + 2);
        OnActivation?.Print(indent + 2);
    }
}

public class Name : Node
{
    public Expression name;

    public Name (Expression expression)
    {
        name = expression;
    }
    public void Print(int indent = 0)
    {

        Debug.Log(new string(' ', indent) + "Name:");
        name.Print(indent + 2);
    }
}

public class CardType : Node
{
    public Expression type;

    public CardType (Expression expression)
    {
        type = expression;
    }
    public void Print(int indent = 0)
    {

        Debug.Log(new string(' ', indent) + "CardType:");
        type.Print(indent + 2);
    }
}

public class Faction : Node
{
    public Expression faction;
    public Faction(Expression expression)
    {
        faction = expression;
    }
    public void Print(int indent = 0)
    {

        Debug.Log(new string(' ', indent) + "Faction:");
        faction.Print(indent + 2);
    }
}

public class Power : Node
{
    public Expression power;

    public Power(Expression expression)
    {
        power = expression;
    }
    public void Print(int indent = 0)
    {

        Debug.Log(new string(' ', indent) + "Power:");
        power.Print(indent + 2);
    }
}

public class PowerAsField : Node
{
    public PowerAsField()
    {

    }

    public void Print(int indent) //Checkear
    {

    }
}

public class Range : Node
{
    public Expression[] range;
    string Lexeme;

    public Range(Expression[] expressions)
    {
        range = expressions;
    }
    public Range(string lexeme)
    {
        Lexeme = lexeme;
    }
    public void Print(int indent = 0)
    {

        Debug.Log(new string(' ', indent) + "Range:");
        if(range != null)
        {
            foreach(var expr in range)
            {
                expr.Print(indent + 2);
            }
        }
        else
        {

            Debug.Log(new string(' ', indent) + "Lexeme: " + Lexeme);
        }
    }
}

public class OnActivation : Node
{
    public List<OnActivationElements> Elements;
    public OnActivation()
    {
        Elements = new List<OnActivationElements>();   
    }
    public void Print(int indent = 0)
    {

        Debug.Log(new string(' ', indent) + "OnActivation:");
        foreach (var element in Elements)
        {
            element.Print(indent + 2);
        }
    }
}

public class OnActivationElements : Node
{
    public OAEffect OAEffect;
    public Selector Selector;
    public List<PostAction> postActions;
    public OnActivationElements(OAEffect oaEffect, Selector selector, List<PostAction> pA)
    {
        OAEffect = oaEffect;
        Selector = selector;
        postActions = pA;
    }
    public void Print(int indent = 0)
    {

        Debug.Log(new string(' ', indent) + "OnActivationElements:");
        OAEffect?.Print(indent + 2);
        Selector?.Print(indent + 2);
        foreach(var postAction in postActions)
        {
            postAction?.Print(indent + 2);
        }
    }
}

public class OAEffect : Node
{
    public string Name;
    public List<Assignment> Assingments;
    public OAEffect(string name, List<Assignment> assingments)
    {
        Name = name;
        Assingments = assingments;
    }
    public void Print(int indent = 0)
    {


        Debug.Log(new string(' ', indent) + "OAEffect:");
        Debug.Log(new string(' ', indent + 2) + "Name: " + Name);
        foreach (var assignment in Assingments)
        {
            assignment.Print(indent + 2);
        }
    }
}

public class Selector : Node
{
    public string Source;
    public Single Single;
    public Predicate Predicate;
    public Selector(string source,Single single,Predicate predicate)
    {
        Source = source;
        Single = single;
        Predicate = predicate;
    }
    public void Print(int indent = 0)
    {


        Debug.Log(new string(' ', indent) + "Selector:");
        Debug.Log(new string(' ', indent + 2) + "Source: " + Source);
        Single?.Print(indent + 2);
        Predicate?.Print(indent + 2);
    }
}

public class Single : Node
{
    public bool Value;
    public Single(Token token)
    {
        if(token.Type == TokenType.BOOL)
        {
            if(token.Lexeme == "true")  Value = true;
            else Value = false;
        }
    }
    public void Print(int indent = 0)
    {

        Debug.Log(new string(' ', indent) + "Single: " + Value);
    }
}

public class Predicate : Node
{
    public Variable Var;
    public Expression Condition;
    public Predicate(Variable var,Expression condition)
    {
        Var = var;
        Condition = condition;
    }
    public void Print(int indent = 0)
    {

        Debug.Log(new string(' ', indent) + "Predicate:");
        Var?.Print(indent + 2);
        Condition?.Print(indent + 2);
    }
}

public class PostAction : Node
{
    public Expression Type;
    public List<Assignment> Assingments;
    public Selector Selector;
    public PostAction(Expression type,Selector selector)
    {
        Type = type;
        Selector = selector;
        Assingments = new List<Assignment>();
    }
    public void Print(int indent = 0)
    {

        Debug.Log(new string(' ', indent) + "PostAction:");
        Type?.Print(indent + 2);
        Selector?.Print(indent + 2);
    }
}

public class EffectNode : Node
{
    public Name Name;
    public Args Params;
    public Action Action;
    public EffectNode()
    {
       
    }

    public EffectNode(Name name,Args param,Action action)
    {
        Name = name;
        Params = param;
        Action = action;
    }

    public void Print(int indent = 0)
    {

        Debug.Log(new string(' ', indent) + "Effect:");
        Name?.Print(indent + 2);
        Params?.Print(indent + 2);
        Action?.Print(indent + 2);
    }
}

public class Action : Node
{
    public Variable Targets;
    public Variable Context;
    public StmsBlock Block;
    public Action(Variable targets,Variable context,StmsBlock block)
    {
        Targets = targets;
        Context = context;
        Block = block;
    }
    public void Print(int indent = 0)
    {

        Debug.Log(new string(' ', indent) + "Action:");
        Targets?.Print(indent + 2);
        Context?.Print(indent + 2);
        Block?.Print(indent + 2);
    }
}

public abstract class Expression : Node
{
    public abstract void Print(int indent=0);
    public abstract object Evaluate(Context context);
}

public class Number : Expression
{
    public int Value;
    public Number(int value)
    {
        Value = value;
    }

    public override object Evaluate(Context context)
    {
        return Value;
    }

    public override void Print(int indent = 0)
    {
        Debug.Log(new string(' ', indent) + "Number: " + Value);
    }
}

public class String : Expression
{
    public string Value;
    public String(string value)
    {
        Value = value;
    }

    public override object Evaluate(Context context)
    {
        return Value;
    }

    public override void Print(int indent = 0)
    {
        Debug.Log(new string(' ', indent) + "String: " + Value);
    }
}

public class Bool : Expression
{
    public bool Value;
    public Bool(bool value)
    {
        Value = value;
    }

    public override object Evaluate(Context context)
    {
        return Value;
    }

    public override void Print(int indent = 0)
    {
        Debug.Log(new string(' ', indent) + "Bool: " + Value);
    }
}

public class BinaryExpression : Expression
{
    public Expression Left;
    public Token Operators;
    public Expression Right;
    public BinaryExpression(Expression left,Token operators,Expression right)
    {
        Left = left;
        Operators = operators;
        Right = right;
    }
    public override object Evaluate(Context context)
    {
        object leftValue = Left.Evaluate(context);
        object rightValue = Right.Evaluate(context);
        if(leftValue is int && rightValue is int)
        {
            switch (Operators.Type)
            {
                case TokenType.PLUS:
                    return Convert.ToInt32(leftValue) + Convert.ToInt32(rightValue);    
                case TokenType.MINUS:
                    return Convert.ToInt32(leftValue) - Convert.ToInt32(rightValue);
                case TokenType.STAR:
                    return Convert.ToInt32(leftValue) * Convert.ToInt32(rightValue);
                case TokenType.SLASH:
                    return Convert.ToInt32(leftValue) / Convert.ToInt32(rightValue);
                case TokenType.PERCENT:
                    return Convert.ToInt32(leftValue) % Convert.ToInt32(rightValue);
                case TokenType.POW:
                    return Math.Pow(Convert.ToInt32(leftValue),Convert.ToInt32(rightValue));
                case TokenType.GREATER:
                    return Convert.ToInt32(leftValue) > Convert.ToInt32(rightValue);
                case TokenType.GREATER_EQUAL:
                    return Convert.ToInt32(leftValue) >= Convert.ToInt32(rightValue);
                case TokenType.LESS:
                    return Convert.ToInt32(leftValue) < Convert.ToInt32(rightValue);
                case TokenType.LESS_EQUAL:
                    return Convert.ToInt32(leftValue) <= Convert.ToInt32(rightValue);
                case TokenType.BANG_EQUAL: return !leftValue.Equals(rightValue);
                case TokenType.EQUAL_EQUAL: return leftValue.Equals(rightValue);
                default:
                throw new InvalidOperationException("Unsupported operator: " + Operators.Lexeme);
            }
        }
        else if(leftValue is string && rightValue is string)
        {
            switch (Operators.Type)
            {
                case TokenType.ATSIGN : return leftValue.ToString() + rightValue.ToString();
                case TokenType.ATSIGN_ATSIGN : return leftValue.ToString() + " " + rightValue.ToString();
                case TokenType.EQUAL_EQUAL: return leftValue.Equals(rightValue);
                default:
                throw new InvalidOperationException("Unsupported operator: " + Operators.Lexeme);
            }
        }
        else throw new InvalidOperationException("Unsupported operator: " + Operators.Lexeme);
    }
    public override void Print(int indent = 0)
    {
        Debug.Log(new string(' ', indent) + "BinaryOperator: " + Operators.Lexeme);
        Left.Print(indent + 2);
        Right.Print(indent + 2);
    }
}

public class BinaryBooleanExpression : BinaryExpression
{
    public BinaryBooleanExpression(Expression left, Token operators, Expression right) : base(left, operators, right)
    {}
}

public class BinaryIntergerExpression : BinaryExpression
{
    public BinaryIntergerExpression(Expression left, Token operators, Expression right) : base(left, operators, right)
    {}
}

public class BinaryStringExpression : BinaryExpression
{
    public BinaryStringExpression(Expression left, Token operators, Expression right) : base(left, operators, right)
    {}
}

public class UnaryExpression : Expression
{
    public Token Operators;
    public Expression Right;
    public UnaryExpression(Token operators,Expression right)
    {
        Operators = operators;
        Right = right;
    }
    public override object Evaluate(Context context)
    {
        object rightValue = Right.Evaluate(context);

        switch (Operators.Type)
        {
            case TokenType.MINUS:
                return -Convert.ToDouble(rightValue);
            case TokenType.BANG:
                return !(bool)rightValue;
            case TokenType.PLUS_PLUS_LEFT:
                return Convert.ToInt32(rightValue) + 1;
            case TokenType.MINUS_MINUS_LEFT:
                return Convert.ToInt32(rightValue) - 1;
            case TokenType.PLUS_PLUS_RIGHT:
                int originalValue = Convert.ToInt32(rightValue);
                int newValue = originalValue + 1;
                context.variables[(Right as Variable).Value] = newValue;
                return originalValue;
            case TokenType.MINUS_MINUS_RIGHT:
                int oGval = Convert.ToInt32(rightValue);
                int newVal = oGval + 1;
                context.variables[(Right as Variable).Value] = newVal;
                return oGval;
            default:
                throw new InvalidOperationException("Unsupported operator: " + Operators.Lexeme);
        }
    }
    
    public override void Print(int indent = 0)
    {
        Debug.Log(new string(' ', indent) + "UnaryExpression: " + Operators.Lexeme);
        Right.Print(indent + 2);
    }
}

public class UnaryBooleanExpression : UnaryExpression
{
    public UnaryBooleanExpression(Token operators, Expression right) : base(operators,right){}
}

public class UnaryIntergerExpression : UnaryExpression
{
    public UnaryIntergerExpression(Token operators, Expression right) : base(operators,right){}
}

public class ExpressionGroup : Expression
{
    public Expression Exp;
    public ExpressionGroup(Expression expression)
    {
        Exp = expression;
    }
    public override object Evaluate(Context context)
    {
        return Exp.Evaluate(context);
    }
    public override void Print(int indent = 0)
    {
        Debug.Log(new string(' ', indent) + "ExpressionGroup:");
        Exp.Print(indent + 2);
    }
}

public class Variable : Expression
{
    public Token Token{get;}
    public string Value{get;}
    public Type type;
    public enum Type
    {
        TARGETS, CONTEXT, CARD, FIELD, INT, STRING, BOOL, VOID, NULL
    }
    public Variable(Token token)
    {
        this.Token = token;
        Value = token.Lexeme;
        type = Type.NULL;
    }
    public void TypeParam(TokenType tokenType)
    {
        if(tokenType == TokenType.BOOLTYPE)
        {
            type = Type.BOOL;
        }
        if(tokenType == TokenType.NUMBERTYPE)
        {
            type = Type.INT;
        }
        if(tokenType == TokenType.STRINGTYPE)
        {
            type = Type.STRING;
        }
    }

    public override object Evaluate(Context context)
    {
        return context.variables[Value];
    }

    public override void Print(int indent = 0)
    {
        Debug.Log(new string(' ', indent) + "Variable: " + Value + " (" + type.ToString() + ")");
    }
}

public class VariableComp : Variable,Stmt
{
    public Args args;

    public VariableComp(Token token) : base(token)
    {
        args = new Args();
    }

    public void Execute(Context context)
    {
        object last = null;
        foreach(var arg in args.Arguments)
        {
            if(arg is Function)
            {
                last = (arg as Function).GetValue(context,last);
            }
            else if(arg is Pointer)
            {
                Pointer pointer = arg as Pointer;
                switch(pointer.pointer)
                {
                    case "Hand": last = context.battleBehaviour.HandOfPlayer(context.battleBehaviour.TriggerPlayer());break;
                    case "Deck": last = context.battleBehaviour.DeckOfPlayer(context.battleBehaviour.TriggerPlayer());break;
                    case "Graveyard": last = context.battleBehaviour.GraveYardOfPlayer(context.battleBehaviour.TriggerPlayer());break;
                    case "Field": last = context.battleBehaviour.FieldOfPlayer(context.battleBehaviour.TriggerPlayer());break;
                    case "Board": last = context.battleBehaviour.Board();break;
                }
            }
        }
    }

    public override object Evaluate(Context context)
    {
        object last = context.variables[Value];
        foreach(var arg in args.Arguments)
        {
            if(arg is Function)
            {
                last = (arg as Function).GetValue(context,last);
            }
            else if(arg is Indexer)
            {
                if(last is CardList)
                {
                    List<Card> cards = (last as CardList).Cards;
                    Indexer indexer = arg as Indexer;
                    last = cards[indexer.index];
                }
                else
                {
                    string[] range = last as string[];
                    Indexer indexer = arg as Indexer;
                    last = range[indexer.index];
                }
            }
            else if(arg is Pointer)
            {
                Pointer pointer = arg as Pointer;
                switch(pointer.pointer)
                {
                    case "Hand": last = context.battleBehaviour.HandOfPlayer(context.battleBehaviour.TriggerPlayer());break;
                    case "Deck": last = context.battleBehaviour.DeckOfPlayer(context.battleBehaviour.TriggerPlayer());break;
                    case "Graveyard": last = context.battleBehaviour.GraveYardOfPlayer(context.battleBehaviour.TriggerPlayer());break;
                    case "Field": last = context.battleBehaviour.FieldOfPlayer(context.battleBehaviour.TriggerPlayer());break;
                    case "Board": last = context.battleBehaviour.Board();break;
                }
            }
            else
            {
                Card card = last as Card;
                switch(arg)
                {
                    case CardType: last = card.type;break;
                    case Name: last = card.name;break;
                    case Faction: last = card.faction;break;
                    case PowerAsField: last = card.points;break;
                    case Range: last = card.range;break;
                    case Owner: last = card.Owner;break;
                }
            }
        }
        return last;
    }

    public void AssignValue(Context context, object value)
    {
        object last = null;
        if(Value == "target")
        {
            last = context.variables[Value];
        }
        foreach(var arg in args.Arguments)
        {
            if(arg is Function)
            {
                last = (arg as Function).GetValue(context,last);
            }
            else if(arg is Indexer)
            {
                if(last is CardList)
                {
                    List<Card> cards = (last as CardList).Cards;
                    Indexer indexer = arg as Indexer;
                    last = cards[indexer.index];
                }
                else
                {
                    string[] range = last as string[];
                    Indexer indexer = arg as Indexer;
                    range[indexer.index] = value as string;
                }
            }
            else if(arg is Pointer)
            {
                Pointer pointer = arg as Pointer;
                switch(pointer.pointer)
                {
                    case "Hand": last = context.battleBehaviour.HandOfPlayer(context.battleBehaviour.TriggerPlayer());break;
                    case "Deck": last = context.battleBehaviour.DeckOfPlayer(context.battleBehaviour.TriggerPlayer());break;
                    case "Graveyard": last = context.battleBehaviour.GraveYardOfPlayer(context.battleBehaviour.TriggerPlayer());break;
                    case "Field": last = context.battleBehaviour.FieldOfPlayer(context.battleBehaviour.TriggerPlayer());break;
                    case "Board": last = context.battleBehaviour.Board();break;
                }
            }
            else
            {
                Card card = last as Card;
                switch(arg)
                {
                    case CardType: card.type = value as string;break;
                    case Name: card.name = value as string;break;
                    case Faction: card.faction = value as string;break;
                    case PowerAsField: card.points = Convert.ToInt32(value);break;
                    case Range: last = card.range;break;
                }
            }
        }
    }

    public override void Print(int indent = 0)
    {
        Debug.Log(new string(' ', indent) + "VariableComp: " + Value);
        args?.Print(indent + 2);
    }
}

public class Args : Node
{
    public List<Node> Arguments;
    public Args()
    {
        Arguments = new List<Node>();
    }

    public void Print(int indent = 0)
    {
        Debug.Log(new string(' ', indent) + "Args:");
        foreach (var arg in Arguments)
        {
            arg.Print(indent + 2);
        }
    }
}

public interface Stmt : Node
{
    public void Execute(Context context);
}

public class StmsBlock : Node
{
    public List<Stmt> statements;

    public StmsBlock()
    {
        statements = new List<Stmt>();
    }

    public void Print(int indent = 0)
    {
        Debug.Log(new string(' ', indent) + "StmsBlock:");
        foreach (var stmt in statements)
        {
            stmt.Print(indent + 2);
        }
    }
}

public class Assignment : Stmt
{
    public Variable Left;
    public Token Op;
    public Expression Right;
    public Assignment(Variable left, Token op, Expression right)
    {
        Left = left;
        Op = op;
        Right = right;
    }

    public void Execute(Context context)
    {
        if(Op.Type == TokenType.EQUAL)
        {
            if(Left is VariableComp)
            {
                (Left as VariableComp).AssignValue(context,Right.Evaluate(context));
            }
            else
            {
                context.variables[Left.Value] = Right.Evaluate(context);
            }
        }
        else if(Op.Type == TokenType.PLUS_EQUALS)
        {
            if(Left is VariableComp)
            {
                (Left as VariableComp).AssignValue(context,Convert.ToInt32(Left.Evaluate(context))+Convert.ToInt32(Right.Evaluate(context)));
            }
            else
            {
                int result = Convert.ToInt32(context.variables[Left.Value]);
                result += Convert.ToInt32(Right.Evaluate(context));
                context.variables[Left.Value] = result;
            }
        }
        else if(Op.Type == TokenType.MINUS_EQUALS)
        {
            if(Left is VariableComp)
            {
                (Left as VariableComp).AssignValue(context,Convert.ToInt32(Left.Evaluate(context))-Convert.ToInt32(Right.Evaluate(context)));
            }
            else
            {
                int result = Convert.ToInt32(context.variables[Left.Value]);
                result -= Convert.ToInt32(Right.Evaluate(context));
                context.variables[Left.Value] = result;
            }
        }
    }

    public void Print(int indent = 0)
    {
        Debug.Log(new string(' ', indent) + "Assignment:");
        Left?.Print(indent + 2);
        Debug.Log(new string(' ', indent + 2) + "Op: " + Op.Lexeme);
        Right?.Print(indent + 2);
    }
}

public class WhileStatement : Stmt
{
    public Expression Condition;
    public StmsBlock Body;
    public WhileStatement(Expression condition,StmsBlock body)
    {
        Condition = condition;
        Body = body;
    }

    public void Execute(Context context)
    {
        while((bool)Condition.Evaluate(context))
        {
            foreach(var stmt in Body.statements)
            {
                stmt.Execute(context);
            }
        }
    }

    public void Print(int indent = 0)
    {
        Debug.Log(new string(' ', indent) + "WhileStatement:");
        Debug.Log(new string(' ', indent + 2) + "Condition:");
        Condition?.Print(indent + 2);
        Debug.Log(new string(' ', indent + 2) + "Body:");
        Body?.Print(indent + 2);
    }
}

public class ForStatement : Stmt
{
    public Variable Target;
    public Variable Targets;
    public StmsBlock Body;
    public ForStatement(Variable target, Variable targets, StmsBlock body)
    {
        Target = target;
        Targets = targets;
        Body = body;
    }

    public void Execute(Context context)
    {
        foreach(Card target in context.variables["targets"] as List<Card>)
        {
            context.variables["target"] = target;
            foreach(var stmt in Body.statements)
            {
                stmt.Execute(context);
            }
            context.variables.Remove("target");
        }
    }

    public void Print(int indent = 0)
    {
        Debug.Log(new string(' ', indent) + "ForStatement:");
        Debug.Log(new string(' ', indent + 2) + "Target:");
        Target?.Print(indent + 2);
        Debug.Log(new string(' ', indent + 2) + "Targets:");
        Targets?.Print(indent + 2);
        Debug.Log(new string(' ', indent + 2) + "Body:");
        Body?.Print(indent + 2);
    }
}

public class Function : Stmt
{
    public string FunctionName;
    public Args Args;
    public Variable.Type Type;
    public Function(string functionName,Args args)
    {
        FunctionName = functionName;
        Args = args;
        Type = Variable.Type.NULL;
        TypeToReturn();
    }

    public void TypeToReturn()
    {
        if (FunctionName == "FieldOfPlayer") Type = Variable.Type.CONTEXT;
        if (FunctionName == "HandOfPlayer") Type = Variable.Type.FIELD;
        if (FunctionName == "GraveyardOfPlayer") Type = Variable.Type.FIELD;
        if (FunctionName == "DeckOfPlayer") Type = Variable.Type.FIELD;
        if (FunctionName == "Find") Type = Variable.Type.TARGETS;
        if (FunctionName == "Push") Type = Variable.Type.VOID;
        if (FunctionName == "SendBottom") Type = Variable.Type.VOID;
        if (FunctionName == "Pop") Type = Variable.Type.CARD;
        if (FunctionName == "Remove") Type = Variable.Type.VOID;
        if (FunctionName == "Shuffle") Type = Variable.Type.VOID;
        if (FunctionName == "Add") Type = Variable.Type.VOID;
    }

    public void Print(int indent = 0)
    {
        Debug.Log(new string(' ', indent) + "Function:");
        Debug.Log(new string(' ', indent + 2) + "FunctionName: " + FunctionName);
        Args?.Print(indent + 2);
        Debug.Log(new string(' ', indent + 2) + "Return Type: " + Type.ToString());
    }

    public void Execute(Context context)
    {
        throw new NotImplementedException();
    }

    public object GetValue(Context context, object value)
    {
        switch(FunctionName)
        {
            case "TriggerPlayer": return context.battleBehaviour.TriggerPlayer();
            case "HandOfPlayer": if(Args.Arguments[0] is Function) return context.battleBehaviour.HandOfPlayer(Convert.ToInt32((Args.Arguments[0] as Function).GetValue(context,value)));
            else return context.battleBehaviour.HandOfPlayer(Convert.ToInt32((Args.Arguments[0] as Expression).Evaluate(context)));
            case "DeckOfPlayer": if(Args.Arguments[0] is Function) return context.battleBehaviour.DeckOfPlayer(Convert.ToInt32((Args.Arguments[0] as Function).GetValue(context,value)));
            else return context.battleBehaviour.DeckOfPlayer(Convert.ToInt32((Args.Arguments[0] as Expression).Evaluate(context)));
            case "GraveyardOfPlayer": if(Args.Arguments[0] is Function) return context.battleBehaviour.GraveYardOfPlayer(Convert.ToInt32((Args.Arguments[0] as Function).GetValue(context,value)));
            else return context.battleBehaviour.GraveYardOfPlayer(Convert.ToInt32((Args.Arguments[0] as Expression).Evaluate(context)));
            case "FieldOfPlayer": if(Args.Arguments[0] is Function) return context.battleBehaviour.FieldOfPlayer(Convert.ToInt32((Args.Arguments[0] as Function).GetValue(context,value)));
            else return context.battleBehaviour.FieldOfPlayer(Convert.ToInt32((Args.Arguments[0] as Expression).Evaluate(context)));
            //case "Find": return (value as CardList).Find()
            case "Push": (value as CardList).Push((Args.Arguments[0] as Expression).Evaluate(context) as Card);return null;
            case "SendBottom": (value as CardList).SendBottom((Args.Arguments[0] as Expression).Evaluate(context) as Card);return null;
            case "Pop": return (value as CardList).Pop();
            case "Remove": (value as CardList).Remove((Args.Arguments[0] as Expression).Evaluate(context) as Card);return null;
            case "Shuffle": (value as CardList).Shuffle();return null; 
            default: return null;
        }
    }
}

public class Pointer : Node
{
    public string pointer;
    public Pointer(string pointer)
    {
        this.pointer = pointer;
    }

    public void Print(int indent = 0)
    {
        Debug.Log(new string(' ', indent) + "Pointer: " + pointer);
    }
}

public class Owner : Node
{
    public string owner;
    public Owner(string owner)
    {
        this.owner = owner;
    }

    public void Print(int indent = 0)
    {
        Debug.Log(new string(' ', indent) + "Owner: " + owner);
    }
}

public class Indexer : Node
{
    public int index;
    public Indexer(int index)
    {
        this.index = index;
    }

    public void Print(int indent = 0)
    {
        Debug.Log(new string(' ', indent) + "Indexer: " + index);
    }
}