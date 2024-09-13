using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using UnityEngine;

public class Lexer
{
    private readonly List<Token> Tokens = new List<Token>();
    private static Dictionary<string,Token> keywords = new Dictionary<string, Token>();
    private string Input{get;}
    private List<string> errors;
    private int start = 0;
    private int current = 0;
    private int line = 1;
    public Lexer(string input, List<string> errors)
    {
        Input=input;
        this.errors = errors;
        keywords = new Dictionary<string, Token>();
        keywords.Add("while", new Token(TokenType.WHILE,"while","while",0,0));
        keywords.Add("for", new Token(TokenType.FOR,"for","for",0,0));
        keywords.Add("in", new Token(TokenType.IN,"in","in",0,0));
        keywords.Add("true", new Token(TokenType.BOOL,"true","true",0,0));
        keywords.Add("false", new Token(TokenType.BOOL,"false","false",0,0));
        
        keywords.Add("card", new Token(TokenType.CARD, "card", "card", 0, 0));
        keywords.Add("effect", new Token(TokenType.EFFECT, "effect", "effect", 0, 0));
        keywords.Add("Name", new Token(TokenType.NAME, "Name", "Name", 0, 0));
        keywords.Add("Params", new Token(TokenType.PARAMS, "Params", "Params", 0, 0));
        keywords.Add("Action", new Token(TokenType.ACTION, "Action", "Action", 0, 0));
        keywords.Add("Type", new Token(TokenType.TYPE, "Type", "Type", 0, 0));
        keywords.Add("Faction", new Token(TokenType.FACTION, "Faction", "Faction", 0, 0));
        keywords.Add("Power", new Token(TokenType.POWER, "Power", "Power", 0, 0));
        keywords.Add("Range", new Token(TokenType.RANGE, "Range", "Range", 0, 0));
        keywords.Add("Owner", new Token(TokenType.OWNER, "Owner", "Owner", 0, 0));
        keywords.Add("OnActivation", new Token(TokenType.ONACTIVATION, "OnActivation", "OnActivation", 0, 0));
        keywords.Add("Effect", new Token(TokenType.ONACTIVATIONEFFECT, "Effect", "Effect", 0, 0));
        keywords.Add("Selector", new Token(TokenType.SELECTOR, "Selector", "Selector", 0, 0));
        keywords.Add("Single", new Token(TokenType.SINGLE, "Single", "Single", 0, 0));
        keywords.Add("Predicate", new Token(TokenType.PREDICATE, "Predicate", "Predicate", 0, 0));
        keywords.Add("PostAction", new Token(TokenType.POSTACTION, "PostAction", "PostAction", 0, 0));
        keywords.Add("Source", new Token(TokenType.SOURCE, "Source", "Source", 0, 0));

        keywords.Add("Hand", new Token(TokenType.POINTER, "Hand", "Hand", 0, 0));
        keywords.Add("Field", new Token(TokenType.POINTER, "Field", "Field", 0, 0));
        keywords.Add("Deck", new Token(TokenType.POINTER, "Deck", "Deck", 0, 0));
        keywords.Add("Graveyard", new Token(TokenType.POINTER, "Graveyard", "Graveyard", 0, 0));
        keywords.Add("Board", new Token(TokenType.POINTER, "Board", "Board", 0, 0));

        keywords.Add("TriggerPlayer", new Token(TokenType.FUN, "TriggerPlayer", "TriggerPlayer", 0, 0));
        keywords.Add("HandOfPlayer", new Token(TokenType.FUN, "HandOfPlayer", "HandOfPlayer", 0, 0));
        keywords.Add("DeckOfPlayer", new Token(TokenType.FUN, "DeckOfPlayer", "DeckOfPlayer", 0, 0));
        keywords.Add("FieldOfPlayer", new Token(TokenType.FUN, "FieldOfPlayer", "FieldOfPlayer", 0, 0));
        keywords.Add("GraveyardOfPlayer", new Token(TokenType.FUN, "GraveyardOfPlayer", "GraveyardOfPlayer", 0, 0));
        keywords.Add("Find", new Token(TokenType.FUN, "Find", "Find", 0, 0));
        keywords.Add("Push", new Token(TokenType.FUN, "Push", "Push", 0, 0));
        keywords.Add("SendBottom", new Token(TokenType.FUN, "SendBottom", "SendBottom", 0, 0));
        keywords.Add("Pop", new Token(TokenType.FUN, "Pop", "Pop", 0, 0));
        keywords.Add("Remove", new Token(TokenType.FUN, "Remove", "Remove", 0, 0));
        keywords.Add("Shuffle", new Token(TokenType.FUN, "Shuffle", "Shuffle", 0, 0));

        keywords.Add("Number", new Token(TokenType.NUMBERTYPE, "Number", "Number", 0, 0));
        keywords.Add("String", new Token(TokenType.STRINGTYPE, "String", "String", 0, 0));
        keywords.Add("Bool", new Token(TokenType.BOOLTYPE, "Bool", "Bool", 0, 0));
    }
    public List<Token> ScanTokens()
    {
        while (!IsAtTheEnd())
        {
            start = current;
            ScanToken();
        }
        Tokens.Add(new Token(TokenType.EOF, "", "" , line, current));
        return Tokens;
    }
    void ScanToken()
    {
        char c = Advance();
        switch(c)
        {
            //Single-character token
            case '(': AddToken(TokenType.LEFT_PAREN); break;
            case ')': AddToken(TokenType.RIGHT_PAREN); break;
            case '{': AddToken(TokenType.LEFT_BRACE); break;
            case '}': AddToken(TokenType.RIGHT_BRACE); break;
            case '[': AddToken(TokenType.LEFT_BRACK); break;
            case ']': AddToken(TokenType.RIGHT_BRACK); break;
            case ',': AddToken(TokenType.COMMA); break;
            case '.': AddToken(TokenType.DOT); break;
            case ':': AddToken(TokenType.COLON); break;
            case ';': AddToken(TokenType.SEMICOLON); break;
            case '*': AddToken(TokenType.STAR); break;
            case '^': AddToken(TokenType.POW); break;
            case '%': AddToken(TokenType.PERCENT); break;
            //One,two or three character token
            case '+':
            if(Match('+')) AddToken(TokenType.PLUS_PLUS_LEFT);
            else if(Match('=')) AddToken(TokenType.PLUS_EQUALS);
            else AddToken(TokenType.PLUS);
            break;
            case '-':
            if(Match('-')) AddToken(TokenType.MINUS_MINUS_RIGHT);
            else if(Match('=')) AddToken(TokenType.MINUS_EQUALS);
            else AddToken(TokenType.MINUS);
            break;
            case '!':
            AddToken(Match('=') ? TokenType.BANG_EQUAL : TokenType.BANG); 
            break;
            case '=':
            if(Match('=')) AddToken(TokenType.EQUAL_EQUAL);
            else if(Match('>')) AddToken(TokenType.EQUAL_GREATER);
            else AddToken(TokenType.EQUAL);
            break;
            case '<':
            AddToken(Match('=') ? TokenType.LESS_EQUAL : TokenType.LESS);
            break;
            case '>':
            AddToken(Match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER);
            break;
            case '/':
            if(Match('/'))
            {
                while(Peek() != '\n' && !IsAtTheEnd())
                {
                    Advance();
                }
            }
            else AddToken(TokenType.SLASH);
            break;
            case'@':
            AddToken(Match('@') ? TokenType.ATSIGN_ATSIGN : TokenType.ATSIGN); 
            break;
            case'|':
            if(Match('|')) AddToken(TokenType.OR);
            break;
            case'&':
            if(Match('&')) AddToken(TokenType.AND);
            break;
            case'"': String(); break;
            case ' ':
            case '\r':
            case '\t':
            // Ignore whitespace.
            break;
            case '\n':
            line++;
            break;
            default:
            if(IsDigit(c)) Number();
            else if(IsAlpha(c)) Identifier();
            else
            {
                errors.Add($"Lexic error: {line + " Unexpected character." + start + " " + current}");
                if(!IsAtTheEnd()) Advance();
            } 
            break;
        }
    }
    bool IsAtTheEnd()
    {
        return current >= Input.Length;
    }
    char Advance()
    {
        current ++;
        return Input[current - 1];
    }
    bool Match(char expected)
    {
        if(IsAtTheEnd()) return false;
        if(Input[current] != expected) return false;
        current++;
        return true;
    }
    char Peek()
    {
        if(IsAtTheEnd()) return '\0';
        return Input[current];
    }
    char PeekNext()
    {
        if(current+1>= Input.Length) return '\0';
        return Input[current+1];
    }
    void String()
    {
        while(Peek() != '"' && !IsAtTheEnd())
        {
            if(Peek() == '\n') line++;
            Advance();
        }
        if(IsAtTheEnd())
        {
            errors.Add($"Lexic error:Unfinished string in line {line}.");
        }
        else
        {
            Advance();
            string value = Input.Substring(start+1,current-start-2);
            AddToken(TokenType.STRING, value);
        }
    }
    bool IsDigit(char c)
    {
        return c >= '0' && c <= '9'; 
    }
    void Number()
    {
        while(IsDigit(Peek())) Advance();
        if(Peek() == '.' && IsDigit(PeekNext()))
        { 
            Advance();
            while(IsDigit(Peek())) Advance();
        }
        AddToken(TokenType.NUMBER,Convert.ToInt32(Input.Substring(start,current-start)));
    }
    bool IsAlpha(char c)
    {
        return (c >= 'a' && c <= 'z')||(c >= 'A' && c <= 'Z') ||c == '_';
    }
    bool IsAlphaNummeric(char c)
    {
        return IsAlpha(c) || IsDigit(c);
    }
    void Identifier()
    {
        while(IsAlphaNummeric(Peek())) Advance();
        string text = Input.Substring(start,current-start);
        TokenType type = TokenType.IDENTIFIER;
        if(keywords.ContainsKey(text))
        {
            Token token = keywords[text];
            Tokens.Add(new Token(token.Type,token.Lexeme,token.Lexeme,line,current/line)); //Revisar
        }
        else AddToken(type);
    }
    void AddToken(TokenType type)
    {
        AddToken(type,""); //Revisar
    }
    void AddToken(TokenType type, object literal)
    {
        string text = Input.Substring(start,current-start);
        Tokens.Add(new Token(type,text,literal,line,current/line));
    }
}