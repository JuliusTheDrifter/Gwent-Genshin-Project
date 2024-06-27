public abstract class AST
{
    public abstract object Evaluate();
    public abstract void Print();
}
public class BinaryOperator : AST
{
    protected AST Left;
    protected Token Operators;
    protected AST Right;
    public BinaryOperator(AST left,Token operators,AST right)
    {
        Left = left;
        Operators = operators;
        Right = right;
    }
    public override object Evaluate()
    {
        throw new NotImplementedException();
    }
    public override void Print()
    {
        throw new NotImplementedException();
    }
}
public class UnaryExpression : AST
{
    protected Token Operators;
    protected AST Right;
    public UnaryExpression(Token operators,AST right)
    {
        Operators = operators;
        Right = right;
    }
    public override object Evaluate()
    {
        throw new NotImplementedException();
    }
    public override void Print()
    {
        throw new NotImplementedException();
    }
}
public class Number : AST
{
    public Token Token;
    public int Value;
    public Number(Token token)
    {
        Token = token;
    }
    public override object Evaluate()
    {
        throw new NotImplementedException();
    }
    public override void Print()
    {
        throw new NotImplementedException();
    }
}
public class String : AST
{
    public string Text;
    public String(Token token)
    {
        Text = token.Lexeme; //?
    }
    public override void Print()
    {
        throw new NotImplementedException();
    }
    public override object Evaluate()
    {
        throw new NotImplementedException();
    }
}
public class Name : AST
{
    public string Name;
    public Name(Token token)
    {

    }
}
class LiteralExpression : Expression
{
    protected Object Literal;
    public LiteralExpression(Object literal)
    {
        Literal = literal;
    }
    public override object Evaluate()
    {
        return Literal;
    }
}
class ExpressionGroup : Expression
{
    public Expression Exp;
    public ExpressionGroup(Expression expression)
    {
        Exp = expression;
    }
    public override object Evaluate()
    {
        return Exp.Evaluate();
    }
}