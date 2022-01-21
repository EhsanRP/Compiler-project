// See https://aka.ms/new-console-template for more information

using Pidgin;
using static Pidgin.Parser;

namespace Ehsan;

public abstract record Node;

public abstract record Kind : Node;

public record IntegerKind : Kind;

public abstract record Expression : Node;

public record Identifier(string Name) : Expression;

public record IntegerLiteral(int Value) : Expression;

public record AddExpression(Expression Left, Expression Right) : Expression;

public record SubtractExpression(Expression Left, Expression Right) : Expression;

public record MultiplyExpression(Expression Left, Expression Right) : Expression;

public record DivideExpression(Expression Left, Expression Right) : Expression;

public record ModuloExpression(Expression Left, Expression Right) : Expression;

public record GreaterThanEqualExpression(Expression Left, Expression Right) : Expression;

public record GreaterThanExpression(Expression Left, Expression Right) : Expression;

public record EqualExpression(Expression Left, Expression Right) : Expression;

public record LessThanExpression(Expression Left, Expression Right) : Expression;

public record LessThanEqualExpression(Expression Left, Expression Right) : Expression;

public record AndExpression(Expression Left, Expression Right) : Expression;

public record OrExpression(Expression Left, Expression Right) : Expression;

public record NotExpression(Expression Left) : Expression;

public record IncrementExpression(Expression Left) : Expression;

public record DecrementExpression(Expression Left) : Expression;

public abstract record Statement : Node;

public abstract record Assignment : Statement;

public record FullAssignmentStatement(Kind Kind, Identifier Identifier, Expression Value) : Assignment;

public record HalfAssignment(Identifier Identifier, Expression Value) : Assignment;

public record FullAddAssignmentStatement(Kind Kind, Identifier Identifier, Expression Value) : Statement;

public record HalfAddAssignmentStatement(Identifier Identifier, Expression Value) : Statement;

public record FullSubtractAssignmentStatement(Kind Kind, Identifier Identifier, Expression Value) : Statement;

public record HalfSubtractAssignmentStatement(Identifier Identifier, Expression Value) : Statement;

public record FullMultiplyAssignmentStatement(Kind Kind, Identifier Identifier, Expression Value) : Statement;

public record HalfMultiplyAssignmentStatement(Identifier Identifier, Expression Value) : Statement;

public record FullDivideAssignmentStatement(Kind Kind, Identifier Identifier, Expression Value) : Statement;

public record HalfDivideAssignmentStatement(Identifier Identifier, Expression Value) : Statement;

public record IfStatement(Expression Condition, Statement[] Statements) : Statement;

public record WhileStatement(Expression Condition, Statement[] Statements) : Statement;

public record DoWhileStatement(Statement[] Statements, Expression Condition) : Statement;

public record ForStatement(
    Assignment InitialAssignment,
    Expression Condition,
    HalfAssignment StepAssignment) : Statement;

public record MyProgram(Statement[] StatementArray) : Node;

public class Program
{
    public static Parser<char, Node> IntegerKind;
    public static Parser<char, Node> Kind;
    public static Parser<char, Node> Identifier;
    public static Parser<char, Node> IntegerLiteral;
    public static Parser<char, Node> Expression;
    public static Parser<char, Node> AddExpression;
    public static Parser<char, Node> SubtractExpression;
    public static Parser<char, Node> MultiplyExpression;
    public static Parser<char, Node> DivideExpression;
    public static Parser<char, Node> ModuloExpression;
    public static Parser<char, Node> GreaterThanEqualExpression;
    public static Parser<char, Node> GreaterThanExpression;
    public static Parser<char, Node> EqualExpression;
    public static Parser<char, Node> LessThanEqualExpression;
    public static Parser<char, Node> LessThanExpression;
    public static Parser<char, Node> AndExpression;
    public static Parser<char, Node> OrExpression;
    public static Parser<char, Node> IncrementExpression;
    public static Parser<char, Node> NotExpression;
    public static Parser<char, Node> DecrementExpression;
    public static Parser<char, Node> BooleanExpression;
    public static Parser<char, Node> ExpressionStatement;
    public static Parser<char, Node> Statement;
    public static Parser<char, Node> FullAssignmentStatement;
    public static Parser<char, Node> HalfAssignmentStatement;
    public static Parser<char, Node> FullAddAssignmentStatement;
    public static Parser<char, Node> HalfAddAssignmentStatement;
    public static Parser<char, Node> HalfSubtractAssignmentStatement;
    public static Parser<char, Node> IfStatement;
    public static Parser<char, Node> WhileStatement;
    public static Parser<char, Node> DoWhileStatement;
    public static Parser<char, Node> Parser;


    static Program()
    {
        IntegerKind = String("int").Select<Node>(_ => new IntegerKind());

        Kind = OneOf(IntegerKind);

        Identifier = Map(
                (first, rest) => first + rest,
                Letter,
                LetterOrDigit.ManyString())
            .Select<Node>(name => new Identifier(name));

        IntegerLiteral = Num.Select<Node>(i => new IntegerLiteral(i));

        Expression = null;

        Parser<char, Node> MakeBinaryExpressionParser(string op, Func<Expression, Expression, Node> factory)
        {
            return Map(
                (left, _, right) => factory((Expression) left, (Expression) right),
                Rec(() => Expression).Between(Whitespaces, Whitespaces),
                String(op).Between(Whitespaces, Whitespaces),
                Rec(() => Expression).Between(Whitespaces, Whitespaces));
        }
        AddExpression = MakeBinaryExpressionParser(
            "+",
            (left, right) => new AddExpression(left, right));

        SubtractExpression = MakeBinaryExpressionParser(
            "-",
            (left, right) => new SubtractExpression(left, right));

        MultiplyExpression = MakeBinaryExpressionParser(
            "*",
            (left, right) => new MultiplyExpression(left, right));

        DivideExpression = MakeBinaryExpressionParser(
            "/",
            (left, right) => new DivideExpression(left, right));

        ModuloExpression = MakeBinaryExpressionParser(
            "%",
            (left, right) => new ModuloExpression(left, right));

        GreaterThanEqualExpression = MakeBinaryExpressionParser(
            ">=",
            (left, right) => new GreaterThanEqualExpression(left, right));

        GreaterThanExpression = MakeBinaryExpressionParser(
            ">",
            (left, right) => new GreaterThanExpression(left, right));

        EqualExpression = MakeBinaryExpressionParser(
            "==",
            (left, right) => new EqualExpression(left, right));

        LessThanEqualExpression = MakeBinaryExpressionParser(
            "<=",
            (left, right) => new LessThanEqualExpression(left, right));

        LessThanExpression = MakeBinaryExpressionParser(
            "<",
            (left, right) => new LessThanExpression(left, right));

        AndExpression = MakeBinaryExpressionParser(
            "&&",
            (left, right) => new AndExpression(left, right));

        OrExpression = MakeBinaryExpressionParser(
            "||",
            (left, right) => new OrExpression(left, right));


        Parser<char, Node> MakeUnaryExpressionParser(string op, Func<Expression, Node> factory)
        {
            return String(op)
                .Then(Rec(() => Expression).Between(Whitespaces, Whitespaces))
                .Select(exp => factory((Expression) exp));
        }

        IncrementExpression = MakeUnaryExpressionParser(
            "++",
            left => new IncrementExpression(left));

        NotExpression = MakeUnaryExpressionParser(
            "!",
            left => new NotExpression(left));

        DecrementExpression = MakeUnaryExpressionParser(
            "--",
            left => new DecrementExpression(left));

        BooleanExpression = OneOf(
            Try(NotExpression),
            Try(GreaterThanEqualExpression),
            Try(GreaterThanExpression),
            Try(LessThanEqualExpression),
            Try(LessThanExpression),
            Try(AndExpression),
            Try(OrExpression), 
            Try(EqualExpression));

        Expression = OneOf(
            IncrementExpression,
            DecrementExpression,
            BooleanExpression,
            AddExpression,
            SubtractExpression ,
            MultiplyExpression,
            DivideExpression,
            ModuloExpression,
            Try(Identifier));

        

        ExpressionStatement = Expression
            .Between(Whitespaces, Whitespaces)
            .Before(Char(';').Between(Whitespaces, Whitespaces));

        Parser<char, Node> MakeFullAssignmentStatement(string token, Func<Kind, Identifier, Expression, Node> factory)
        {
            return Map(
                    (k, i, _, e) => factory((Kind) k, (Identifier) i, (Expression) e),
                    Kind.Between(Whitespaces, Whitespaces),
                    Identifier.Between(Whitespaces, Whitespaces),
                    String(token).Between(Whitespaces, Whitespaces),
                    Expression.Between(Whitespaces, Whitespaces))
                .Before(Char(';').Between(Whitespaces, Whitespaces));
        }

        Parser<char, Node> MakeHalfAssignmentStatement(string token, Func<Identifier, Expression, Node> factory)
        {
            return Map(
                    (i, _, e) => (Node) new HalfAssignment((Identifier) i, (Expression) e),
                    Identifier.Between(Whitespaces, Whitespaces),
                    String(token).Between(Whitespaces, Whitespaces),
                    Expression.Between(Whitespaces, Whitespaces))
                .Before(Char(';').Between(Whitespaces, Whitespaces));
        }

        Statement = null;

        FullAssignmentStatement = MakeFullAssignmentStatement(
            "=",
            (k, i, e) => new FullAssignmentStatement(k, i, e));

        HalfAssignmentStatement = MakeHalfAssignmentStatement(
            "=",
            (i, e) => new HalfAssignment(i, e));

        FullAddAssignmentStatement = MakeFullAssignmentStatement(
            "+=",
            (k, i, e) => new FullAssignmentStatement(k, i, e));

        HalfAddAssignmentStatement = MakeHalfAssignmentStatement(
            "+=",
            (i, e) => new HalfAssignment(i, e));

        HalfSubtractAssignmentStatement = MakeHalfAssignmentStatement(
            "-=",
            (i, e) => new HalfAssignment(i, e));

        IfStatement = Map(
            (_1, b, _2, xs) =>
                (Node) new IfStatement((Expression) b, xs.Select(x => (Statement) x).ToArray()),
            String("if").Between(Whitespaces, Whitespaces),
            BooleanExpression.Between(Whitespaces, Whitespaces).Between(Char('('), Char(')')),
            Whitespaces,
            Rec(() => Statement).Between(Whitespaces, Whitespaces).Many().Between(Char('{'), Char('}')));

        WhileStatement = Map(
            (_1, b, xs) => (Node) new WhileStatement((Expression) b, xs.Select(x => (Statement) x).ToArray()),
            String("while")
                .Between(Whitespaces, Whitespaces),
            BooleanExpression
                .Between(Whitespaces, Whitespaces)
                .Between(Char('('), Char(')')
                    .Between(Whitespaces, Whitespaces)),
            Rec(() => Statement)
                .Between(Whitespaces, Whitespaces)
                .Many()
                .Between(Char('{'), Char('}')
                    .Between(Whitespaces, Whitespaces)));

        DoWhileStatement = Map(
            (_1, xs, _2, b) => (Node) new DoWhileStatement(xs.Select(x => (Statement) x).ToArray(), (Expression) b),
            String("do").Between(Whitespaces, Whitespaces),
            Rec(() => Statement)
                .Between(Whitespaces, Whitespaces)
                .Many()
                .Between(Char('{'), Char('}')
                    .Between(Whitespaces, Whitespaces)),
            String("while")
                .Between(Whitespaces, Whitespaces),
            BooleanExpression
                .Between(Whitespaces, Whitespaces)
                .Between(Char('('), Char(')')
                    .Between(Whitespaces, Whitespaces)));

        Statement = OneOf(
            ExpressionStatement,
            FullAssignmentStatement,
            HalfAssignmentStatement,
            FullAddAssignmentStatement,
            HalfAddAssignmentStatement);

        Parser = Statement
            .Between(Whitespaces, Whitespaces)
            .Many()
            .Select(xs => (Node) new MyProgram(xs.Select(x => (Statement) x).ToArray()));
    }

    public static void Main(string[] args)
    {
        //TODO: Expressions do not support numbers. Example: "2 + 3" or "int a = 2" 
        try
        {
            var node = BooleanExpression.ParseOrThrow("a >= b");
            Console.WriteLine(node);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}