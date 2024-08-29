public class Token
{
    public TokenType Type{get;set;}
    public string Lexeme{get;set;}
    public object Literal{get;set;}
    public int Line{get;set;}
    public int Column{get;set;}
    public Token(TokenType type, string lexeme, object literal, int line, int column)
    {
        Type=type;
        Lexeme=lexeme;
        Literal=literal;
        Line=line;
        Column = column;
    }
}
public enum TokenType
{
    //Single-character tokens
    LEFT_PAREN, RIGHT_PAREN, LEFT_BRACE, RIGHT_BRACE, LEFT_BRACK, RIGHT_BRACK,
    COMMA, DOT, COLON, SEMICOLON, SLASH, STAR, POW, PERCENT,
    //One or more character tokens
    MINUS, MINUS_MINUS_LEFT, MINUS_MINUS_RIGHT, MINUS_EQUALS,
    PLUS, PLUS_PLUS_LEFT, PLUS_PLUS_RIGHT, PLUS_EQUALS,
    BANG, BANG_EQUAL,
    EQUAL, EQUAL_EQUAL, EQUAL_GREATER,
    GREATER, GREATER_EQUAL,
    LESS, LESS_EQUAL,
    ATSIGN, ATSIGN_ATSIGN,
    //Literals
    IDENTIFIER, STRING, NUMBER, BOOL,
    //KeyWords
    AND, FALSE, TRUE, OR, IN,
    WHILE, FOR, CARD, EFFECT, OWNER,
    NAME, PARAMS, ACTION, TYPE,
    FACTION, POWER, RANGE, ONACTIVATION,
    SELECTOR, SINGLE, PREDICATE, POSTACTION, SOURCE,
    ZONE, FUN, POINTER, ONACTIVATIONEFFECT,
    NUMBERTYPE, STRINGTYPE, BOOLTYPE, 

    EOF
}