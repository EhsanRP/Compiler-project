using System.Collections.Immutable;
using System.Text;
using Pidgin;
using Pidgin.Expression;
using static Pidgin.Parser;
using static Pidgin.Parser<char>;

namespace CompilerProject;

public abstract record Node;

public abstract record MyExpression : Node
{
    public override string ToString()
    {
        return base.ToString();
    }
};

public enum KindType
{
    Integer,
}

public record Kind(KindType Type) : Node;

public record Identifier(string Name) : MyExpression
{
    public override string ToString()
    {
        return "Identifier { Value = "+Name+" }";
    }
};

public record Literal(int Value) : MyExpression
{
    public override string ToString()
    {
        return "Literal { Value = "+Value+" }";
    }
};

public record Call(MyExpression Expression, ImmutableArray<MyExpression> Arguments) : MyExpression;

public enum UnaryOperatorType
{
    Increment,
    Decrement,
    Negate,
    Complement
}

public record UnaryOperation(UnaryOperatorType Type, MyExpression Expression) : MyExpression;

public enum BinaryOperatorType
{
    Add,
    Subtract,
    Mulitply,
    Divide,
    Modulo,
    And,
    Or,
    GreaterThanEqual,
    GreaterThan,
    Equal,
    LessThan,
    LessThanEqual,
}

public record BinaryOperation(BinaryOperatorType Type, MyExpression Left, MyExpression Right) : MyExpression;

public abstract record Statement : Node;

public record ExpressionStatement(MyExpression Expression) : Statement;

public record Assignment(Maybe<Kind> Kind, Identifier Identifier, MyExpression Value) : Statement
{
    public override string ToString()
    {
        return "Assignment { Kind = "+ Kind.Value.Type+", Identifier = Identifier { Name = "+ Identifier.Name +" }, Value = Literal { Value = " + Value + " } }";
    }
};

public record IfStatement(MyExpression Condition, ImmutableArray<Statement> Statements) : Statement
{
    public override string ToString()
    {
        var statementsTostring = new StringBuilder();
        foreach (var statement in Statements)
        {
            statementsTostring.Append(statement.ToString() + " ");
        }
        return Condition.ToString()  + statementsTostring ;
    }
};

public record WhileStatement(MyExpression Condition, ImmutableArray<Statement> Statements) : Statement
{
    public override string ToString()
    {
        var statementsTostring = new StringBuilder();
        foreach (var statement in Statements)
        {
            statementsTostring.Append(statement.ToString() + " ");
        }
        return "WhileStatement { "+ Condition.ToString()  + statementsTostring + "}" ;
    }
};

public record DoWhileStatement(ImmutableArray<Statement> Statements, MyExpression Condition) : Statement
{
    public override string ToString()
    {
        var statementsTostring = new StringBuilder();
        foreach (var statement in Statements)
        {
            statementsTostring.Append(statement.ToString() + " ");
        }
        return "DoWhileStatement { " +   statementsTostring +Condition.ToString() + "}";
    }
};

public record ForStatement(
    Assignment InitialAssignment,
    MyExpression Condition,
    MyExpression StepAssignment,
    ImmutableArray<Statement> Statements) : Statement
{
    public override string ToString()
    {
        var statementsTostring = new StringBuilder();
        foreach (var statement in Statements)
        {
            statementsTostring.Append(statement.ToString() + " ");
        }
        return "ForStatement { " +InitialAssignment.ToString() + Condition.ToString() + StepAssignment.ToString() + statementsTostring+ "}";
    }
};

public record JavaProgram(ImmutableArray<Statement> Statements) : Node;

public static class JavaParser
{
    public static Parser<char, T> Tok<T>(Parser<char, T> token)
        => Try(token).Before(SkipWhitespaces);

    public static Parser<char, string> Tok(string token)
        => Tok(String(token));

    public static readonly Parser<char, Kind> Kind
        = Tok(String("int"))
            .Select<Kind>(name => new Kind(KindType.Integer))
            .Labelled("integer kind");

    public static Parser<char, T> Parenthesised<T>(Parser<char, T> parser)
        => parser.Between(Tok("("), Tok(")"));

    public static Parser<char, T> Braced<T>(Parser<char, T> parser)
        => parser.Between(Tok("{"), Tok("}"));

    public static Parser<char, Func<MyExpression, MyExpression, MyExpression>> Binary(
        Parser<char, BinaryOperatorType> op)
        => op.Select<Func<MyExpression, MyExpression, MyExpression>>(type => (l, r) => new BinaryOperation(type, l, r));

    public static Parser<char, Func<MyExpression, MyExpression>> Unary(Parser<char, UnaryOperatorType> op)
        => op.Select<Func<MyExpression, MyExpression>>(type => o => new UnaryOperation(type, o));

    public static readonly Parser<char, Func<MyExpression, MyExpression, MyExpression>> Add
        = Binary(Tok("+").ThenReturn(BinaryOperatorType.Add));

    public static readonly Parser<char, Func<MyExpression, MyExpression, MyExpression>> Subtract
        = Binary(Tok("-").ThenReturn(BinaryOperatorType.Subtract));

    public static readonly Parser<char, Func<MyExpression, MyExpression, MyExpression>> Multiply
        = Binary(Tok("*").ThenReturn(BinaryOperatorType.Mulitply));

    public static readonly Parser<char, Func<MyExpression, MyExpression, MyExpression>> Divide
        = Binary(Tok("/").ThenReturn(BinaryOperatorType.Divide));

    public static readonly Parser<char, Func<MyExpression, MyExpression, MyExpression>> Modulo
        = Binary(Tok("%").ThenReturn(BinaryOperatorType.Modulo));

    public static readonly Parser<char, Func<MyExpression, MyExpression, MyExpression>> And
        = Binary(Tok("&&").ThenReturn(BinaryOperatorType.And));

    public static readonly Parser<char, Func<MyExpression, MyExpression, MyExpression>> Or
        = Binary(Tok("||").ThenReturn(BinaryOperatorType.Or));

    public static readonly Parser<char, Func<MyExpression, MyExpression, MyExpression>> GreaterThanEqual
        = Binary(Tok(">=").ThenReturn(BinaryOperatorType.GreaterThanEqual));

    public static readonly Parser<char, Func<MyExpression, MyExpression, MyExpression>> GreaterThan
        = Binary(Tok(">").ThenReturn(BinaryOperatorType.GreaterThan));

    public static readonly Parser<char, Func<MyExpression, MyExpression, MyExpression>> Equal
        = Binary(Tok("==").ThenReturn(BinaryOperatorType.Equal));

    public static readonly Parser<char, Func<MyExpression, MyExpression, MyExpression>> LessThan
        = Binary(Tok("<").ThenReturn(BinaryOperatorType.LessThan));

    public static readonly Parser<char, Func<MyExpression, MyExpression, MyExpression>> LessThanEqual
        = Binary(Tok("<=").ThenReturn(BinaryOperatorType.LessThanEqual));

    public static readonly Parser<char, Func<MyExpression, MyExpression>> Increment
        = Unary(Tok("++").ThenReturn(UnaryOperatorType.Increment));

    public static readonly Parser<char, Func<MyExpression, MyExpression>> Decrement
        = Unary(Tok("--").ThenReturn(UnaryOperatorType.Decrement));

    public static readonly Parser<char, Func<MyExpression, MyExpression>> Negate
        = Unary(Tok("-").ThenReturn(UnaryOperatorType.Negate));

    public static readonly Parser<char, Func<MyExpression, MyExpression>> Complement
        = Unary(Tok("!").ThenReturn(UnaryOperatorType.Complement));


    public static readonly Parser<char, MyExpression> Identifier
        = Try(
            Tok(Letter.Then(LetterOrDigit.ManyString(), (h, t) => h + t))
                .Bind(identifier =>
                {
                    return identifier switch
                    {
                        "int" => Fail<MyExpression>(),
                        "if" => Fail<MyExpression>(),
                        "while" => Fail<MyExpression>(),
                        "do" => Fail<MyExpression>(),
                        "for" => Fail<MyExpression>(),
                        _ => Return(identifier).Select<MyExpression>(name => new Identifier(name))
                    };
                })
                .Labelled("identifier")
        );

    public static readonly Parser<char, MyExpression> Literal
        = Tok(Num)
            .Select<MyExpression>(value => new Literal(value))
            .Labelled("integer literal");

    public static Parser<char, Func<MyExpression, MyExpression>> Call(Parser<char, MyExpression> subExpr)
        => Parenthesised(subExpr.Separated(Tok(",")))
            .Select<Func<MyExpression, MyExpression>>(args => method => new Call(method, args.ToImmutableArray()))
            .Labelled("function call");

    public static readonly Parser<char, MyExpression> Expression = ExpressionParser.Build<char, MyExpression>(
        expr => (
            OneOf(
                Identifier,
                Literal,
                Parenthesised(expr).Labelled("parenthesised expression")
            ),
            new[]
            {
                Operator.PostfixChainable(Call(expr)),
                Operator.Postfix(Increment).And(Operator.Postfix(Decrement)),
                Operator.Prefix(Negate).And(Operator.Prefix(Complement)),
                Operator.InfixL(Multiply).And(Operator.InfixL(Divide)),
                Operator.InfixL(Add).And(Operator.InfixL(Subtract)),
                Operator.InfixL(Modulo),
                Operator.InfixL(And),
                Operator.InfixL(Or),
                Operator.InfixL(GreaterThanEqual)
                    .And(Operator.InfixL(GreaterThan))
                    .And(Operator.InfixL(Equal))
                    .And(Operator.InfixL(LessThan))
                    .And(Operator.InfixL(LessThanEqual))
            }
        )
    ).Labelled("expression");

    public static Parser<char, Statement> Statement;

    public static Parser<char, Statement> ExpressionStatement =
        Expression.Before(Tok(";")).Select<Statement>(x => new ExpressionStatement(x));

    public static Parser<char, Statement> Assignment = Map(
        (kind, identifier, expression) =>
            (Statement) new Assignment(kind, (Identifier) identifier, expression),
        Kind.Optional(),
        Identifier.Before(Tok("=")),
            Expression.Before(Tok(";")))
        .Labelled("assignment");

    public static Parser<char, Statement> IfStatement = Map(
            (_, expression, statements) =>
                (Statement) new IfStatement(expression, statements.Select(x => (Statement) x).ToImmutableArray()),
            Tok("if"),
            Parenthesised(Expression),
            Braced(Rec(() => Statement).Many()))
        .Labelled("if statement");

    public static Parser<char, Statement> WhileStatement = Map(
            (_, expression, statements) =>
                (Statement) new WhileStatement(expression, statements.Select(x => (Statement) x).ToImmutableArray()),
            Tok("while"),
            Parenthesised(Expression),
            Braced(Rec(() => Statement).Many()))
        .Labelled("while statement");

    public static Parser<char, Statement> DoWhileStatement = Map(
            (_, statements, expression) =>
                (Statement) new DoWhileStatement(statements.Select(x => (Statement) x).ToImmutableArray(), expression),
            Tok("do"),
            Braced(Rec(() => Statement).Many()).Before(String("while").Between(Whitespaces,Whitespaces)),
            Parenthesised(Expression).Before(Tok(";"))
        )
        .Labelled("do while statement");

    public static Parser<char, Statement> ForStatement = Map(
        (_1, _2, init, cond, step, _3, ss) =>
            (Statement) new ForStatement((Assignment) init, cond, step, ss.Select(x => (Statement) x).ToImmutableArray()),
        Tok("for"),
        Tok("("),
        Assignment,
        Expression.Between(Whitespaces,Whitespaces).Before(Char(';')).Between(Whitespaces,Whitespaces),
        Expression,
        Tok(")"),
        Braced(Rec(() => Statement).Many()));

    public static Parser<char, JavaProgram> Program;
    
    static JavaParser()
    {
        Statement = OneOf(
            ExpressionStatement,
            Assignment,
            IfStatement,
            WhileStatement,
            DoWhileStatement,
            ForStatement);
        
        Program =
            Statement.Many()
                .Select(statements => 
                    new JavaProgram(statements.Select(x => (Statement)x).ToImmutableArray()))
                .Between(SkipWhitespaces, SkipWhitespaces);
    }

    public static JavaProgram ParseOrThrow(string input)
        => Program.ParseOrThrow(input);
}

public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            while (true)
            {
                var input = Console.ReadLine();
                if (input == "exit")
                {
                    break;
                }
                var node = JavaParser.ParseOrThrow(input);

                foreach (var test in node.Statements)
                {
                    Console.WriteLine(test);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}