namespace Expression;

public class Operators
{
    public delegate ExpressionType Expression(ExpressionType[] exp);

    public readonly string Operator;

    public readonly int DefaultPriority;

    public int AssignPriority { get; set; }

    public int Position { get; set; }

    public readonly Expression ExpressionOperator;

    public readonly bool Binary;

    public Operators(string s, int priority, Expression expressionOperator, bool binary = false)
    {
        this.Operator = s;
        this.DefaultPriority = priority;
        this.ExpressionOperator = expressionOperator;
        this.Binary = binary;
    }
}