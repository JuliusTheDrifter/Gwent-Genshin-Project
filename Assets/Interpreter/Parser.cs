using System.Linq.Expressions;
using System.Text.RegularExpressions;

public class Parser
{
    private List<Token> Tokens{get;}
    private int current = 0;
    public Parser(List<Token> tokens)
    {
        Tokens = tokens;
    }
    public Expression Parse()
    {
        return Expression();
    }
    Expression Expression()
    {
        return Equality();
    }
    Expression Equality()
    {
        Expression expression = Comparison();
        TokenType[] tokenTypes = {TokenType.BANG_EQUAL,TokenType.EQUAL_EQUAL};
        while(Match(tokenTypes))
        {
            Token operators = Previous();
            Expression right = Comparison();
            expression = new BinaryOperator(expression,operators,right);
        }
        return expression;
    }
    Expression Comparison()
    {
        Expression expression = Term();
        TokenType[] tokenTypes = {TokenType.GREATER,TokenType.GREATER_EQUAL,TokenType.LESS,TokenType.LESS_EQUAL};
        while(Match(tokenTypes))
        {
            Token operators = Previous();
            Expression right = Term();
            expression = new BinaryOperator(expression,operators,right);
        }
        return expression;
    }
    Expression Term()
    {
        Expression expression = Factor();
        TokenType[] tokenTypes = {TokenType.PLUS,TokenType.MINUS};
        while(Match(tokenTypes))
        {
            Token operators = Previous();
            Expression right = Factor();
            expression = new BinaryOperator(expression,operators,right);
        }
        return expression;
    }
    Expression Factor()
    {
        Expression expression = Unary();
        TokenType[] tokenTypes = {TokenType.SLASH,TokenType.STAR};
        while(Match(tokenTypes))
        {
            Token operators = Previous();
            Expression right = Unary();
            expression = new BinaryOperator(expression,operators,right);
        }
        return expression;
    }
    Expression Unary()
    {
        TokenType[] tokenTypes = {TokenType.BANG,TokenType.MINUS};
        if(Match(tokenTypes))
        {
            Token operators = Previous();
            Expression right = Unary();
            return new UnaryExpression(operators,right);
        }
        return Primary();
    }
    Expression Primary()
    {
        if(Match(TokenType.FALSE)) return new LiteralExpression(false);
        if(Match(TokenType.TRUE)) return new LiteralExpression(true);
        TokenType[] tokenTypes = {TokenType.NUMBER,TokenType.STRING};
        if(Match(tokenTypes)) return new LiteralExpression(Previous().Literal);
        if(Match(TokenType.LEFT_PAREN))
        {
            Expression expression = Expression();
            Consume(TokenType.RIGHT_PAREN,"Expect ')' after expression.");
            return new ExpressionGroup(expression);
        }
        throw new Exception(Peek().Line + " " + "at " + Peek().Lexeme + " Expectted expression.");
    }
    bool Match(TokenType[] type)
    {
        for(int i=0;i<type.Length;i++)
        {
            if(Check(type[i]))
            {
                Advance();
                return true;
            }
        }
        return false;
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
        if(Check(type)) return Advance();
        throw new Exception(Peek().Line + " " + "at " + Peek().Lexeme + " " + message);
    }
}