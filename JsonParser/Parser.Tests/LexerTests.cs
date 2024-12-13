namespace Parser.Tests;

[TestFixture]
public class ParserTests
{
     [Test]
    public void Lex_ShouldReturnOpenCurlyBraceToken()
    {
        var parser = new Parser("{");
        var token = parser.Lex();
        Assert.That(token.Kind, Is.EqualTo(TokenKind.OpenCurlyBrace));
        Assert.That(token.Literal, Is.EqualTo('{'));
    }

    [Test]
    public void Lex_ShouldReturnCloseCurlyBraceToken()
    {
        var parser = new Parser("}");
        var token = parser.Lex();
        Assert.That(token.Kind, Is.EqualTo(TokenKind.CloseCurlyBrace));
        Assert.That(token.Literal, Is.EqualTo('}'));
    }

    [Test]
    public void Lex_ShouldReturnCommaToken()
    {
        var parser = new Parser(",");
        var token = parser.Lex();
        Assert.That(token.Kind, Is.EqualTo(TokenKind.Comma));
        Assert.That(token.Literal, Is.EqualTo(','));
    }

    [Test]
    public void Lex_ShouldReturnColonToken()
    {
        var parser = new Parser(":");
        var token = parser.Lex();
        Assert.That(token.Kind, Is.EqualTo(TokenKind.Colon));
        Assert.That(token.Literal, Is.EqualTo(':'));
    }

    [Test]
    public void Lex_ShouldReturnOpenSquareBracketToken()
    {
        var parser = new Parser("[");
        var token = parser.Lex();
        Assert.That(token.Kind, Is.EqualTo(TokenKind.OpenSquareBracket));
        Assert.That(token.Literal, Is.EqualTo('['));
    }

    [Test]
    public void Lex_ShouldReturnCloseSquareBracketToken()
    {
        var parser = new Parser("]");
        var token = parser.Lex();
        Assert.That(token.Kind, Is.EqualTo(TokenKind.CloseSquareBracket));
        Assert.That(token.Literal, Is.EqualTo(']'));
    }

    [Test]
    public void Lex_ShouldReturnStringLiteralToken()
    {
        var parser = new Parser("\"hello\"");
        var token = parser.Lex();
        Assert.That(token.Kind, Is.EqualTo(TokenKind.StringLiteral));
        Assert.That(token.Literal, Is.EqualTo("hello"));
    }

    [Test]
    public void Lex_ShouldReturnNumberLiteralToken()
    {
        var parser = new Parser("123.45");
        var token = parser.Lex();
        Assert.That(token.Kind, Is.EqualTo(TokenKind.NumberLiteral));
        Assert.That(token.Literal, Is.EqualTo(123.45));
    }

    [Test]
    public void Lex_ShouldReturnEOFToken()
    {
        var parser = new Parser("");
        var token = parser.Lex();
        Assert.That(token.Kind, Is.EqualTo(TokenKind.EOF));
        Assert.That(token.Literal, Is.EqualTo('\0'));
    }
}