
class Program
{
    public static void Main(string[] args)
    {
        string s = "2+2";
        Lexer a = new Lexer(s);
        List<Token> t = a.ScanTokens();
        Parser b = new Parser(t);
        Expression c = b.Parse();
        Console.WriteLine(c);
    }
}
