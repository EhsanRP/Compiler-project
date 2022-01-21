using Ehsan;
using Pidgin;
using Xunit;

namespace CompilerProjectTests;

public class UnitTest1
{
    [Theory]
    [InlineData("int", "IntegerKind { }")]
    public void CorrectKindsParse(string input, string output)
    {
        var result = Program.Kind.ParseOrThrow(input);
        Assert.Equal(output, result.ToString());
    }

    [Theory]
    [InlineData("my3Variable", "Identifier { Name = my3Variable }")]
    public void CorrectIdentifiersParse(string input, string output)
    {
        var result = Program.Identifier.ParseOrThrow(input);
        Assert.Equal(output, result.ToString());
    }

    [Theory]
    [InlineData("int", "IntegerKind { }")]
    public void CorrectIntegerKindLiteralParse(string input, string output)
    {
        var result = Program.IntegerKind.ParseOrThrow(input);
        Assert.Equal(output, result.ToString());
    }

    [Theory]
    [InlineData("25", "IntegerLiteral { Value = 25 }")]
    public void CorrectIntegerLiteralParse(string input, string output)
    {
        var result = Program.IntegerLiteral.ParseOrThrow(input);
        Assert.Equal(output, result.ToString());
    }

    [Theory]
    [InlineData("a + b", "AddExpression { Left = Identifier { Name = a }, Right = Identifier { Name = b } }")]
    public void CorrectAddExpressionParse(string input, string output)
    {
        var result = Program.AddExpression.ParseOrThrow(input);
        Assert.Equal(output, result.ToString());
    }

    [Theory]
    [InlineData("a - b", "SubtractExpression { Left = Identifier { Name = a }, Right = Identifier { Name = b } }")]
    public void CorrectSubtractExpressionParse(string input, string output)
    {
        var result = Program.SubtractExpression.ParseOrThrow(input);
        Assert.Equal(output, result.ToString());
    }

    [Theory]
    [InlineData("a * b", "MultiplyExpression { Left = Identifier { Name = a }, Right = Identifier { Name = b } }")]
    public void CorrectMultiplyExpressionParse(string input, string output)
    {
        var result = Program.MultiplyExpression.ParseOrThrow(input);
        Assert.Equal(output, result.ToString());
    }

    [Theory]
    [InlineData("a / b", "DivideExpression { Left = Identifier { Name = a }, Right = Identifier { Name = b } }")]
    public void CorrectDivideExpressionParse(string input, string output)
    {
        var result = Program.DivideExpression.ParseOrThrow(input);
        Assert.Equal(output, result.ToString());
    }

    [Theory]
    [InlineData("a % b", "ModuloExpression { Left = Identifier { Name = a }, Right = Identifier { Name = b } }")]
    public void CorrectModuloExpressionParse(string input, string output)
    {
        var result = Program.ModuloExpression.ParseOrThrow(input);
        Assert.Equal(output, result.ToString());
    }

    [Theory]
    [InlineData("a >= b",
        "GreaterThanEqualExpression { Left = Identifier { Name = a }, Right = Identifier { Name = b } }")]
    public void CorrectGreaterThanEqualExpressionParse(string input, string output)
    {
        var result = Program.GreaterThanEqualExpression.ParseOrThrow(input);
        Assert.Equal(output, result.ToString());
    }

    [Theory]
    [InlineData("a > b", "GreaterThanExpression { Left = Identifier { Name = a }, Right = Identifier { Name = b } }")]
    public void CorrectGreaterThanExpressionParse(string input, string output)
    {
        var result = Program.GreaterThanExpression.ParseOrThrow(input);
        Assert.Equal(output, result.ToString());
    }

    [Theory]
    [InlineData("a == b", "EqualExpression { Left = Identifier { Name = a }, Right = Identifier { Name = b } }")]
    public void CorrectEqualExpressionParse(string input, string output)
    {
        var result = Program.EqualExpression.ParseOrThrow(input);
        Assert.Equal(output, result.ToString());
    }

    [Theory]
    [InlineData("a <= b",
        "LessThanEqualExpression { Left = Identifier { Name = a }, Right = Identifier { Name = b } }")]
    public void CorrectLessThanEqualExpressionParse(string input, string output)
    {
        var result = Program.LessThanEqualExpression.ParseOrThrow(input);
        Assert.Equal(output, result.ToString());
    }

    [Theory]
    [InlineData("a < b", "LessThanExpression { Left = Identifier { Name = a }, Right = Identifier { Name = b } }")]
    public void CorrectLessThanExpressionParse(string input, string output)
    {
        var result = Program.LessThanExpression.ParseOrThrow(input);
        Assert.Equal(output, result.ToString());
    }

    [Theory]
    [InlineData("a && b", "AndExpression { Left = Identifier { Name = a }, Right = Identifier { Name = b } }")]
    public void CorrectAndExpressionParse(string input, string output)
    {
        var result = Program.AndExpression.ParseOrThrow(input);
        Assert.Equal(output, result.ToString());
    }

    [Theory]
    [InlineData("a || b", "OrExpression { Left = Identifier { Name = a }, Right = Identifier { Name = b } }")]
    public void CorrectOrExpressionParse(string input, string output)
    {
        var result = Program.OrExpression.ParseOrThrow(input);
        Assert.Equal(output, result.ToString());
    }

    [Theory]
    [InlineData("++a", "IncrementExpression { Left = Identifier { Name = a } }")]
    public void CorrectIncrementExpressionParse(string input, string output)
    {
        var result = Program.IncrementExpression.ParseOrThrow(input);
        Assert.Equal(output, result.ToString());
    }

    [Theory]
    [InlineData("!a", "NotExpression { Left = Identifier { Name = a } }")]
    public void CorrectNotExpressionParse(string input, string output)
    {
        var result = Program.NotExpression.ParseOrThrow(input);
        Assert.Equal(output, result.ToString());
    }

    [Theory]
    [InlineData("--a", "DecrementExpression { Left = Identifier { Name = a } }")]
    public void CorrectDecrementExpressionParse(string input, string output)
    {
        var result = Program.DecrementExpression.ParseOrThrow(input);
        Assert.Equal(output, result.ToString());
    }

    [Theory]
    [InlineData("int a = b;",
        "FullAssignmentStatement { Kind = IntegerKind { }, Identifier = Identifier { Name = a }, Value = Identifier { Name = b } }")]
    public void CorrectFullAssignmentStatementParse(string input, string output)
    {
        var result = Program.FullAssignmentStatement.ParseOrThrow(input);
        Assert.Equal(output, result.ToString());
    }

    [Theory]
    [InlineData("a = b;", "HalfAssignment { Identifier = Identifier { Name = a }, Value = Identifier { Name = b } }")]
    public void CorrectHalfAssignmentParse(string input, string output)
    {
        var result = Program.HalfAssignmentStatement.ParseOrThrow(input);
        Assert.Equal(output, result.ToString());
    }

    [Theory]
    [InlineData("int a += b;",
        "FullAssignmentStatement { Kind = IntegerKind { }, Identifier = Identifier { Name = a }, Value = Identifier { Name = b } }")]
    public void CorrectFullAddAssignmentStatementParse(string input, string output)
    {
        var result = Program.FullAddAssignmentStatement.ParseOrThrow(input);
        Assert.Equal(output, result.ToString());
    }

    [Theory]
    [InlineData("a += b;", "HalfAssignment { Identifier = Identifier { Name = a }, Value = Identifier { Name = b } }")]
    public void CorrectHalfAddAssignmentStatementParse(string input, string output)
    {
        var result = Program.HalfAddAssignmentStatement.ParseOrThrow(input);
        Assert.Equal(output, result.ToString());
    }

    [Theory]
    [InlineData("a -= b;", "HalfAssignment { Identifier = Identifier { Name = a }, Value = Identifier { Name = b } }")]
    public void CorrectHalfSubtractAssignmentStatementParse(string input, string output)
    {
        var result = Program.HalfSubtractAssignmentStatement.ParseOrThrow(input);
        Assert.Equal(output, result.ToString());
    }
    
    [Theory]
    [InlineData("a == b", "EqualExpression { Left = Identifier { Name = a }, Right = Identifier { Name = b } }")]
    [InlineData("a >= b", "GreaterThanEqualExpression { Left = Identifier { Name = a }, Right = Identifier { Name = b } }")]
    [InlineData("a > b", "GreaterThanExpression { Left = Identifier { Name = a }, Right = Identifier { Name = b } }")]
    [InlineData("a <= b", "LessThanEqualExpression { Left = Identifier { Name = a }, Right = Identifier { Name = b } }")]
    [InlineData("a < b", "LessThanExpression { Left = Identifier { Name = a }, Right = Identifier { Name = b } }")]
    [InlineData("!a", "NotExpression { Left = Identifier { Name = a } }")]
    [InlineData("a && b", "AndExpression { Left = Identifier { Name = a }, Right = Identifier { Name = b } }")]
    [InlineData("a || b", "OrExpression { Left = Identifier { Name = a }, Right = Identifier { Name = b } }")]
    public void CorrectBooleanExpressionsParse(string input, string output)
    {
        var result = Program.BooleanExpression.ParseOrThrow(input);
        Assert.Equal(output, result.ToString());
    }
    
    [Theory]
    
    [InlineData("++a", "IncrementExpression { Left = Identifier { Name = a } }")]
    [InlineData("--a", "DecrementExpression { Left = Identifier { Name = a } }")]
    
    [InlineData("a == b", "EqualExpression { Left = Identifier { Name = a }, Right = Identifier { Name = b } }")]
    [InlineData("a >= b", "GreaterThanEqualExpression { Left = Identifier { Name = a }, Right = Identifier { Name = b } }")]
    [InlineData("a > b", "GreaterThanExpression { Left = Identifier { Name = a }, Right = Identifier { Name = b } }")]
    [InlineData("a <= b", "LessThanEqualExpression { Left = Identifier { Name = a }, Right = Identifier { Name = b } }")]
    [InlineData("a < b", "LessThanExpression { Left = Identifier { Name = a }, Right = Identifier { Name = b } }")]
    [InlineData("!a", "NotExpression { Left = Identifier { Name = a } }")]
    [InlineData("a && b", "AndExpression { Left = Identifier { Name = a }, Right = Identifier { Name = b } }")]
    [InlineData("a || b", "OrExpression { Left = Identifier { Name = a }, Right = Identifier { Name = b } }")]
    
    [InlineData("my3Variable", "Identifier { Name = my3Variable }")]
    [InlineData("a + b", "AddExpression { Left = Identifier { Name = a }, Right = Identifier { Name = b } }")]
    [InlineData("a - b", "SubtractExpression { Left = Identifier { Name = a }, Right = Identifier { Name = b } }")]
    [InlineData("a * b", "MultiplyExpression { Left = Identifier { Name = a }, Right = Identifier { Name = b } }")]
    [InlineData("a / b", "DivideExpression { Left = Identifier { Name = a }, Right = Identifier { Name = b } }")]
    [InlineData("a % b", "ModuloExpression { Left = Identifier { Name = a }, Right = Identifier { Name = b } }")]

    public void CorrectExpressionParse(string input, string output)
    {
        var result = Program.Expression.ParseOrThrow(input);
        Assert.Equal(output, result.ToString());
    }
}