using System.Data.Common;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Xml;
using System.Collections.Generic;
using System;

public class Parser
{
    private List<Token> Tokens{get;}
    private int current = 0;
    public Exception Ex;
    public Parser(List<Token> tokens)
    {
        Tokens = tokens;
    }
    public Node Parse()
    {
        Program Program = new Program();
        try
        {
            while(!IsAtEnd())
            {
                if(Match(TokenType.CARD))
                {
                    Consume(TokenType.LEFT_BRACE,"Expected '{' after card");
                    Program.CardNodes.Add(ParseCard());
                    Consume(TokenType.RIGHT_BRACE,"Expected '}' after card declaration");
                }
                else if(Match(TokenType.EFFECT))
                {
                    Consume(TokenType.LEFT_BRACE,"Expected '{'");
                    Program.EffectNodes.Add(ParseEffect());
                    Consume(TokenType.RIGHT_BRACE,"Expected '}' after effect declaration");
                }
                else
                {
                    throw new Exception($"'{Peek().Lexeme}' in {Peek().Line}: Card or Effect expected.");
                }
            }
        }
        catch(Exception ex)
        {
            Ex=ex;
        }
        return Program;
    }

    CardNode ParseCard()
    {
        CardNode card = new CardNode();
        int[] counter = new int[6];
        while(!Check(TokenType.RIGHT_BRACE) && !IsAtEnd())
        {
            if(Match(TokenType.TYPE))
            {
                counter[0]+=1;
                Consume(TokenType.COLON,"Expected ':' after Type");
                card.Type = new Type(ParseExpression());
                Consume(TokenType.COMMA,"Expected ',' after expression");
            }
            else if(Match(TokenType.NAME))
            {
                counter[1]+=1;
                Consume(TokenType.COLON,"Expected ':' after Name");
                card.Name = new Name(ParseExpression());
                Consume(TokenType.COMMA,"Expected ',' after expression");
            }
            else if(Match(TokenType.FACTION))
            {
                counter[2]+=1;
                Consume(TokenType.COLON,"Expected ':' after Faction");
                card.Faction = new Faction(ParseExpression());
                Consume(TokenType.COMMA,"Expected ',' after expression");
            }
            else if(Match(TokenType.POWER))
            {
                counter[3]+=1;
                Consume(TokenType.COLON,"Expected ':' after Power");
                card.Power = new Power(ParseExpression());
                Consume(TokenType.COMMA,"Expected ',' after expression");
            }
            else if(Match(TokenType.RANGE))
            {
                counter[4]+=1;
                Consume(TokenType.COLON,"Expected ':' after Range");
                Consume(TokenType.LEFT_BRACK,"Expected '['");
                List<Expression> expressions = new List<Expression>();
                for(int i=0;i<3;i++)
                {
                    expressions.Add(ParseExpression());
                    if(Match(TokenType.COMMA)) continue;
                    else break;
                }
                Consume(TokenType.RIGHT_BRACK,"Expected ']'");
                Consume(TokenType.COMMA,"Expected ',' after Range");
                card.Range = new Range(expressions.ToArray());
            }
            else if(Match(TokenType.ONACTIVATION))
            {
                counter[5]+=1;
                card.OnActivation = ParseOnActivation();
            }
            else
            {
                throw new Exception($"'{Peek().Lexeme}' in {Peek().Line}: Invalid Card property.");
            }
        }
        if(counter[0]<1) throw new Exception("A Type property is missing from card");
        else if(counter[0]>1) throw new Exception("Only one Type is allowed");
        if(counter[1]<1) throw new Exception("A Name property is missing from card");
        else if(counter[1]>1) throw new Exception("Only one Name is allowed");
        if(counter[2]<1) throw new Exception("A Faction property is missing from card");
        else if(counter[2]>1) throw new Exception("Only one Faction is allowed");
        if(counter[3]<1) throw new Exception("A Power property is missing from card");
        else if(counter[3]>1) throw new Exception("Only one Power is allowed");
        if(counter[4]<1) throw new Exception("A Range property is missing from card");
        else if(counter[4]>1) throw new Exception("Only one Range is allowed");
        if(counter[5]>1) throw new Exception("Only one OnActivation is allowed");
        return card;
    }

    EffectNode ParseEffect()
    {
        EffectNode effect = new EffectNode();
        int[] counter = new int[3];
        while(!Check(TokenType.RIGHT_BRACE) && !IsAtEnd())
        {
            if(Match(TokenType.NAME))
            {
                counter[0]+=1;
                Consume(TokenType.COLON,"Expected ':' after Name");
                effect.Name = new Name(ParseExpression());
                Consume(TokenType.COMMA,"Expected ',' after expression");
            }
            else if(Match(TokenType.PARAMS))
            {
                counter[1]+=1;
                Consume(TokenType.COLON,"Expected ':' after Params");
                effect.Params = GetParams();
            }
            else if(Match(TokenType.ACTION))
            {
                counter[2]+=1;
                Consume(TokenType.COLON,"Expected ':' after Action");
                effect.Action = ParseAction();
            }
            else
            {
                throw new Exception($"'{Peek().Lexeme}' in {Peek().Line}: Invalid Effect property.");
            }
        }
        if(counter[0]<1) throw new Exception("A Name property is missing from effect");
        else if(counter[0]>1) throw new Exception("Only one Name is allowed");
        if(counter[1]>1) throw new Exception("Only one Params is allowed");
        if(counter[2]<1) throw new Exception("An Action property is missing from effect");
        else if(counter[2]>1) throw new Exception("Only one Action is allowed");
        return effect;
    }

    OnActivation ParseOnActivation()
    {
        Consume(TokenType.COLON,"Expected ':' after OnActivation");
        Consume(TokenType.LEFT_BRACK,"Expected '['");
        OnActivation onActivation = new OnActivation();
        while(!Check(TokenType.RIGHT_BRACK) && !IsAtEnd())
        {
            onActivation.Elements.Add(ParseOAE());
        }
        Consume(TokenType.RIGHT_BRACK,"Expected ']'");
        return onActivation;
    }

    OnActivationElements ParseOAE()
    {
        Consume(TokenType.LEFT_BRACE,"Expected '{'");
        OAEffect onActivationEffect = null!;
        Selector selector = null!;
        List<PostAction> postActions = new List<PostAction>();
        while(!Check(TokenType.RIGHT_BRACE) && !IsAtEnd())
        {
            if(Match(TokenType.ONACTIVATIONEFFECT))
            {
                if(onActivationEffect==null)
                {
                    Consume(TokenType.COLON,"Expected ':'");
                    onActivationEffect = ParseOAEffect();
                }
                else
                {
                    throw new Exception($"'{Peek().Lexeme}' in {Peek().Line}: Only one OAEffect per Bracks");
                }
            }
            else if(Match(TokenType.SELECTOR))
            {
                if(selector == null)
                {
                    Consume(TokenType.COLON,"Expected ':'");
                    selector = ParseSelector();
                    if(selector.Source == null) throw new Exception($"'{Peek().Lexeme}' in {Peek().Line}: Missing field");
                    //Consume(TokenType.COMMA,"Expected ','");
                }
                else
                {
                    throw new Exception($"'{Peek().Lexeme}' in {Peek().Line}: Only one Selector per OAEffect");
                }
            }
            else if(Match(TokenType.POSTACTION))
            {
                Consume(TokenType.COLON,"Expected ':'");
                postActions.Add(ParsePostAction());
            }
            else
            {
                throw new Exception($"'{Peek().Lexeme}' in {Peek().Line}: Invalid OnActivation field.");
            }
        }
        Consume(TokenType.RIGHT_BRACE,"Expected '}' after OnActivation declaration");
        return new OnActivationElements(onActivationEffect,selector,postActions);
    }

    OAEffect ParseOAEffect()
    {
        string name = null!;
        List<Assignment> assignments = new List<Assignment>();
        if(Check(TokenType.STRING))
        {
            name = Advance().Lexeme.Substring(1,Previous().Lexeme.Length-2);
            Consume(TokenType.COMMA,"Expected ','");
            while(!Check(TokenType.SELECTOR))
            {
                if(Check(TokenType.IDENTIFIER))
                {
                    Variable variable = ParseVariable();
                    Token token = Peek();
                    Consume(TokenType.COLON,"Expected ':'");
                    Expression expression = ParseExpression();
                    Assignment assignment = new Assignment(variable,token,expression);
                    assignments.Add(assignment);
                    Consume(TokenType.COMMA,"Expected ','");
                }   
                else
                {
                    throw new Exception($"'{Peek().Lexeme}' in {Peek().Line}: Invalid OAEffect field");
                }
            }
        }
        else
        {
            Consume(TokenType.LEFT_BRACE,"Expected '{'");
            while(!Check(TokenType.RIGHT_BRACE) && !IsAtEnd())
            {
                if(Match(TokenType.NAME))
                {
                    if(Match(TokenType.COLON))
                    {
                        if(name == null)
                        {
                            name = Advance().Lexeme.Substring(1,Previous().Lexeme.Length-2);
                            if(!Check(TokenType.RIGHT_BRACE)) Consume(TokenType.COMMA,"Expected ','");
                        }
                        else
                        {
                            throw new Exception($"'{Peek().Lexeme}' in {Peek().Line}: Duplicate");
                        }
                    }
                    else
                    {
                        if(name == null)
                        {
                            name = Advance().Lexeme.Substring(1,Previous().Lexeme.Length-2);
                            if(!Check(TokenType.RIGHT_BRACE)) Consume(TokenType.COMMA,"Expected ','");
                        }
                        else
                        {
                            throw new Exception($"'{Peek().Lexeme}' in {Peek().Line}: Duplicate");
                        }
                    }
                }
                else if(Check(TokenType.IDENTIFIER))
                {
                    Variable variable = ParseVariable();
                    Token token = Peek();
                    Consume(TokenType.COLON,"Expected ':'");
                    Expression expression = ParseExpression();
                    Assignment assignment = new Assignment(variable,token,expression);
                    assignments.Add(assignment);
                    if(!Check(TokenType.RIGHT_BRACE)) Consume(TokenType.COMMA,"Expected ','");
                }
                else
                {
                    throw new Exception($"'{Peek().Lexeme}' in {Peek().Line}: Invalid OAEffect field");
                }
            }
            Consume(TokenType.RIGHT_BRACE,"Expected '}'");
        }
        if(name == null) throw new Exception($"'{Peek().Lexeme}' in {Peek().Line}: No name");
        return new OAEffect(name,assignments);
    }

    Selector ParseSelector()
    {
        Consume(TokenType.LEFT_BRACE,"Expected '{'");
        string source = null!;
        Single single = null!;
        Predicate predicate = null!;
        while(!Check(TokenType.RIGHT_BRACE) && !IsAtEnd())
        {
            if(Match(TokenType.SOURCE))
            {
                Consume(TokenType.COLON,"Expected ':'");
                if(source == null)
                {
                    if(Convert.ToString(Peek().Literal) == "deck"||Convert.ToString(Peek().Literal) == "otherDeck"||Convert.ToString(Peek().Literal) == "hand"||Convert.ToString(Peek().Literal) == "otherHand"||Convert.ToString(Peek().Literal) == "field"||Convert.ToString(Peek().Literal) == "otherField"||Convert.ToString(Peek().Literal) == "parent"||Convert.ToString(Peek().Literal) == "board")
                    {
                        source = Advance().Lexeme;
                    }
                    else
                    {
                        throw new Exception($"'{Peek().Lexeme}' in {Peek().Line}: Invalid Source");
                    }
                }
                else
                {
                    throw new Exception($"'{Peek().Lexeme}' in {Peek().Line}: Source duplicate");
                }
                if(!Check(TokenType.RIGHT_BRACE)) Consume(TokenType.COMMA,"Expected ','");
            }
            else if(Match(TokenType.SINGLE))
            {
                Consume(TokenType.COLON,"Expected ':'");
                if(single == null)
                {
                    single = new Single(Advance());
                }
                else
                {
                    throw new Exception($"'{Peek().Lexeme}' in {Peek().Line}: Single duplicate");
                }
                if(!Check(TokenType.RIGHT_BRACE)) Consume(TokenType.COMMA,"Expected ','");
            }
            else if(Match(TokenType.PREDICATE))
            {
                Consume(TokenType.COLON,"Expected ':'");
                if(predicate == null)
                {
                    predicate = ParsePredicate();
                }
                else
                {
                    throw new Exception($"'{Peek().Lexeme}' in {Peek().Line}: Predicate dsuplicate");
                }
                if(!Check(TokenType.RIGHT_BRACE)) Consume(TokenType.COMMA,"Expected ','");
            }
            else
            {
                throw new Exception($"'{Peek().Lexeme}' in {Peek().Line}: Invalid Selector field");
            }
        }
        Consume(TokenType.RIGHT_BRACE,"Expected '}' after Selector declaration");
        if(single == null || predicate == null) throw new Exception($"'{Peek().Lexeme}' in {Peek().Line}: Missing field");
        return new Selector(source,single,predicate);
    }

    PostAction ParsePostAction()
    {
        Consume(TokenType.LEFT_BRACE,"Expected '{'");
        Expression expression = null!;
        Selector selector = null!;
        List<Assignment> assignments = new List<Assignment>();
        while(!Check(TokenType.RIGHT_BRACE) && !IsAtEnd())
        {
            if(Match(TokenType.TYPE))
            {
                Consume(TokenType.COLON,"Expected ':'");
                expression = ParseExpression();
                if(!Check(TokenType.RIGHT_BRACE)) Consume(TokenType.COMMA,"Expected ','");
            }
            else if(Match(TokenType.SELECTOR))
            {
                Consume(TokenType.COLON,"Expected ':'");
                selector = ParseSelector();
                if(selector.Source == null)
                {
                    selector.Source = "parent";
                }
                if(!Check(TokenType.RIGHT_BRACE)) Consume(TokenType.COMMA,"Expected ','");
            }
            else if(Check(TokenType.IDENTIFIER))
            {
                Variable variable = ParseVariable();
                Token token = Peek();
                Consume(TokenType.COLON,"Expected ':'");
                Expression exp = ParseExpression();
                Assignment assignment = new Assignment(variable,token,exp);
                assignments.Add(assignment);
                if(!Check(TokenType.RIGHT_BRACE)) Consume(TokenType.COMMA,"Expected ','");
            }
            else
            {
                throw new Exception($"'{Peek().Lexeme}' in {Peek().Line}: Invalid PostAction field");
            }
        }
        Consume(TokenType.RIGHT_BRACE,"Expected '}'");
        if(expression == null) throw new Exception("Missing PostAction Type");
        return new PostAction(expression,selector);
    }

    Predicate ParsePredicate()
    {
        Consume(TokenType.LEFT_PAREN,"Expected '('");
        Variable unit = ParseVariable();
        unit.type = Variable.Type.CARD;
        Consume(TokenType.RIGHT_PAREN,"Expected ')'");
        Consume(TokenType.EQUAL_GREATER,"Expected '=>'");
        Expression expression = ParseExpression();
        return new Predicate(unit,expression);
    }

    Action ParseAction()
    {
        Consume(TokenType.LEFT_PAREN,"Expected '('");
        Variable target = ParseVariable();
        Consume(TokenType.COMMA,"Expected ','");
        Variable context = ParseVariable();
        Consume(TokenType.RIGHT_PAREN,"Expected ')'");
        Consume(TokenType.EQUAL_GREATER,"Expected '=>'");
        Consume(TokenType.LEFT_BRACE,"Expected '{'");
        StmsBlock stmsBlock = ParseStmsBlock();
        Consume(TokenType.RIGHT_BRACE,"Expected '}'");
        return new Action(target,context,stmsBlock);
    }

    Variable ParseVariable()
    {
        Variable variable = new Variable(Advance());
        if(Check(TokenType.DOT))
        {
            VariableComp variableComp = new VariableComp(variable.Token);
            Variable.Type varType = Variable.Type.NULL;
            while(Match(TokenType.DOT) && !IsAtEnd())
            {
                if(Match(TokenType.FUN))
                {
                    Function function = ParseFunction(Previous().Lexeme);
                    varType = function.Type;
                    variableComp.args.Arguments.Add(function);
                }
                else
                {
                    if(Match(TokenType.TYPE))
                    {
                        Type type = new Type(new String(Previous().Lexeme));
                        varType = Variable.Type.STRING;
                        variableComp.args.Arguments.Add(type);
                    }
                    else if(Match(TokenType.NAME))
                    {
                        Name name = new Name(new String(Previous().Lexeme));
                        varType = Variable.Type.STRING;
                        variableComp.args.Arguments.Add(name);
                    }
                    else if(Match(TokenType.FACTION))
                    {
                        Faction faction = new Faction(new String(Previous().Lexeme));
                        varType = Variable.Type.STRING;
                        variableComp.args.Arguments.Add(faction);
                    }
                    else if(Match(TokenType.POWER))
                    {
                        PowerAsField power = new PowerAsField();
                        varType = Variable.Type.INT;
                        variableComp.args.Arguments.Add(power);
                    }
                    else if(Match(TokenType.RANGE))
                    {
                        Range range = new Range(Previous().Lexeme);
                        varType = Variable.Type.STRING;
                        variableComp.args.Arguments.Add(range);
                    }
                    else if(Match(TokenType.POINTER))
                    {
                        Pointer pointer = new Pointer(Previous().Lexeme);
                        //varType = Variable.Type.STRING;
                        variableComp.args.Arguments.Add(pointer);
                    }
                    else
                    {
                        throw new Exception($"'{Peek().Lexeme}' in {Peek().Line}: Invalid variable");
                    }
                }
            }
            variable = variableComp;
            variable.type = varType;
        }
        return variable;
    }

    StmsBlock ParseStmsBlock()
    {
        StmsBlock block = new StmsBlock();
        while(!Check(TokenType.RIGHT_BRACE) && !IsAtEnd())
        {
            block.statements.Add(ParseStm());
        }
        return block;
    }

    Stmt ParseStm()
    {
        if(Match(TokenType.FOR))
        {
           return ParseForStm();
        }
        else if(Match(TokenType.WHILE))
        {
            return ParseWhileStm();
        }
        else if(Check(TokenType.IDENTIFIER))
        {
            Variable variable = ParseVariable();
            if(variable is VariableComp && Check(TokenType.SEMICOLON))
            {
                VariableComp v = (variable as VariableComp)!;
                if(v.args.Arguments[v.args.Arguments.Count-1].GetType() == typeof(Function))
                {
                    Function function = (v.args.Arguments[v.args.Arguments.Count-1] as Function)!;
                }
                else
                {
                    throw new System.Exception("A compound variable most end in a function");
                }
                Consume(TokenType.SEMICOLON,"Expected ';'");
                return (variable as VariableComp)!;
            }
            else
            {
                return ParseAssignment(variable);
            }
        }
        else if(Check(TokenType.FUN))
        {
            return ParseFunction(Previous().Lexeme);
        }
        else
        {
            throw new System.Exception("Invalid statement");
        }
    }

    ForStatement ParseForStm()
    {
        Variable target = ParseVariable();
        Consume(TokenType.IN,"Expected 'in'");
        Variable targets = ParseVariable();
        Consume(TokenType.LEFT_BRACE,"Expected {");
        StmsBlock stms = ParseStmsBlock();
        Consume(TokenType.RIGHT_BRACE,"Expected }");
        Consume(TokenType.SEMICOLON,"Expected ';'");
        return new ForStatement(target,targets,stms);
    }

    WhileStatement ParseWhileStm()
    {
        Consume(TokenType.LEFT_PAREN,"Expected '('");
        Expression expression = ParseExpression();
        Consume(TokenType.RIGHT_PAREN,"Expected ')'");
        StmsBlock stms = ParseStmsBlock();
        return new WhileStatement(expression,stms);
    }

    Assignment ParseAssignment(Variable variable)
    {
        Token op = Advance();
        Expression expression = ParseExpression();
        Consume(TokenType.SEMICOLON,"Expected ';'");
        return new Assignment(variable,op,expression);
    }

    Function ParseFunction(string name)
    {
        Consume(TokenType.LEFT_PAREN,"Expected '('");
        Args args = new Args();
        while(!Check(TokenType.RIGHT_PAREN) && !IsAtEnd())
        {
            if(Check(TokenType.IDENTIFIER))
            {
                args.Arguments.Add(ParseVariable());
            }
            else if(Check(TokenType.FUN))
            {
                args.Arguments.Add(ParseFunction(Advance().Lexeme));
            }
            else
            {
                args.Arguments.Add(ParseExpression());
            }
            if(!Check(TokenType.RIGHT_PAREN)) Consume(TokenType.COMMA,"Expected ','");
        }
        Consume(TokenType.RIGHT_PAREN,"Expected ')'");
        Function function = new Function(name,args);
        return function;
    }

    Args GetParams()
    {
        Consume(TokenType.LEFT_BRACE,"Expected '{' after Params");
        Args variables = new Args();
        while(!Check(TokenType.RIGHT_BRACE) && !IsAtEnd())
        {
            var variable = ParseVariable();
            Consume(TokenType.COLON,"Expected ':' after parameter");
            if(Check(TokenType.STRINGTYPE)||Check(TokenType.NUMBERTYPE)||Check(TokenType.BOOLTYPE))
            {
                variable.TypeParam(Advance().Type);
                variables.Arguments.Add(variable);
                if(!Check(TokenType.RIGHT_BRACE))
                {
                    Consume(TokenType.COMMA,"Expected ','");
                }
            }
            else
            {
                throw new Exception("Expected type after parameter name");
            }
        }
        Consume(TokenType.RIGHT_BRACE,"Expected '}' after Params declaration");
        Consume(TokenType.COMMA,"Expected ','");
        return variables;
    }

    Expression ParseExpression()
    {
        var result = Equality();
        return result;
    }

    Expression Equality()
    {
        Expression expression = Comparison();
        while(Match(TokenType.BANG_EQUAL)||Match(TokenType.EQUAL_EQUAL))
        {
            Token operators = Previous();
            Expression right = Comparison();
            expression = new BinaryBooleanExpression(expression,operators,right);
        }
        return expression;
    }

    Expression Comparison()
    {
        Expression expression = Term();
        while(Match(TokenType.GREATER)||Match(TokenType.GREATER_EQUAL)||Match(TokenType.LESS)||Match(TokenType.LESS_EQUAL))
        {
            Token operators = Previous();
            Expression right = Term();
            expression = new BinaryBooleanExpression(expression,operators,right);
        }
        return expression;
    }

    Expression Term()
    {
        Expression expression = Factor();
        if(Check(TokenType.PLUS) || Check(TokenType.MINUS))
        {
            while(Match(TokenType.PLUS)||Match(TokenType.MINUS))
            {
                Token operators = Previous();
                Expression right = Factor();
                expression = new BinaryIntergerExpression(expression,operators,right);
            }
        }
        else if(Check(TokenType.ATSIGN) || Check(TokenType.ATSIGN_ATSIGN))
        {
            while(Match(TokenType.ATSIGN)||Match(TokenType.ATSIGN_ATSIGN))
            {
                Token operators = Previous();
                Expression right = Factor();
                expression = new BinaryStringExpression(expression,operators,right);
            }   
        }
        return expression;
    }

    Expression Factor()
    {
        Expression expression = Unary();
        while(Match(TokenType.SLASH)||Match(TokenType.STAR)||Match(TokenType.PERCENT))
        {
            Token operators = Previous();
            Expression right = Unary();
            expression = new BinaryIntergerExpression(expression,operators,right);
        }
        return expression;
    }

    Expression Unary()
    {
        if(Match(TokenType.MINUS)||Match(TokenType.PLUS_PLUS))
        {
            Token operators = Previous();
            Expression right = Unary();
            return new UnaryIntergerExpression(operators,right);
        }
        else if(Match(TokenType.BANG))
        {
            Token operators = Previous();
            Expression right = Unary();
            return new UnaryBooleanExpression(operators,right);   
        }
        else if (Check(TokenType.IDENTIFIER) && LookAhead(TokenType.PLUS_PLUS))
        {
            Expression left = ParseVariable();
            Token operatorToken = Advance();
            return new UnaryIntergerExpression(operatorToken, left);
        }
        return Primary();
    }

    Expression Primary()
    {
        if(Match(TokenType.FALSE)) return new Bool(false);
        if(Match(TokenType.TRUE)) return new Bool(true);
        if(Match(TokenType.NUMBER)) return new Number(Convert.ToInt32(Previous().Literal));
        if(Match(TokenType.STRING))
        {
            return new String(Previous().Lexeme.Substring(1,Previous().Lexeme.Length-2));
        }
        if(Match(TokenType.LEFT_PAREN))
        {
            Expression expression = Equality();
            Consume(TokenType.RIGHT_PAREN,"Expect ')' after expression.");
            return new ExpressionGroup(expression);
        }
        if(Check(TokenType.IDENTIFIER))
        {
            return ParseVariable();
        }
        return new Bool(false); //*
        throw new System.Exception($"'{Peek().Lexeme}' in line {Peek().Line}: Unexpected token.");
    }

    bool Match(TokenType type)
    {
        if(Check(type))
        {
            Advance();
            return true;
        }
        return false;
    }

    bool Check(TokenType type)
    {
        if(IsAtEnd()) return false;
        return Peek().Type == type;
    }
    
    bool LookAhead(TokenType type)
    {
        if (IsAtEnd()) return false;
        return Tokens[current+1].Type == type;
    }

    bool IsAtEnd()
    {
        return Peek().Type == TokenType.EOF;
    }

    Token Peek()
    {
        return Tokens[current];
    }

    Token Advance()
    {
        if(!IsAtEnd()) current++;
        return Previous();
    }

    Token Previous()
    {
        return Tokens[current-1];
    }

    Token Consume(TokenType type, string message)
    {
        Console.WriteLine(Peek().Type + " " + Peek().Lexeme);
        if(Check(type)) return Advance();
        throw new Exception($"'{Peek().Lexeme}' in line {Peek().Line}: {message}");
    }
}