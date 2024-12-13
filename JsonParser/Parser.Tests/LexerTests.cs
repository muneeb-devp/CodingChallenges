namespace Parser.Tests;

[TestFixture]
public class LexerTests
{
     [Test]
    public void Lex_ShouldReturnOpenCurlyBraceToken()
    {
        var parser = new Lexer("{");
        var token = parser.Lex();
        
        Assert.Multiple(() =>
        {
            Assert.That(token.Kind, Is.EqualTo(TokenKind.OpenCurlyBrace));
            Assert.That(token.Literal, Is.EqualTo('{'));
        });
    }

    [Test]
    public void Lex_ShouldReturnCloseCurlyBraceToken()
    {
        var parser = new Lexer("}");
        var token = parser.Lex();
        
        Assert.Multiple(() =>
        {
            Assert.That(token.Kind, Is.EqualTo(TokenKind.CloseCurlyBrace));
            Assert.That(token.Literal, Is.EqualTo('}'));
        });
    }

    [Test]
    public void Lex_ShouldReturnCommaToken()
    {
        var parser = new Lexer(",");
        var token = parser.Lex();
        
        Assert.Multiple(() =>
        {
            Assert.That(token.Kind, Is.EqualTo(TokenKind.Comma));
            Assert.That(token.Literal, Is.EqualTo(','));
        });
    }

    [Test]
    public void Lex_ShouldReturnColonToken()
    {
        var parser = new Lexer(":");
        var token = parser.Lex();
        
        Assert.Multiple(() =>
        {
            Assert.That(token.Kind, Is.EqualTo(TokenKind.Colon));
            Assert.That(token.Literal, Is.EqualTo(':'));
        });
    }

    [Test]
    public void Lex_ShouldReturnOpenSquareBracketToken()
    {
        var parser = new Lexer("[");
        var token = parser.Lex();
        
        Assert.Multiple(() =>
        {
            Assert.That(token.Kind, Is.EqualTo(TokenKind.OpenSquareBracket));
            Assert.That(token.Literal, Is.EqualTo('['));
        });
    }

    [Test]
    public void Lex_ShouldReturnCloseSquareBracketToken()
    {
        var parser = new Lexer("]");
        var token = parser.Lex();
        
        Assert.Multiple(() =>
        {
            Assert.That(token.Kind, Is.EqualTo(TokenKind.CloseSquareBracket));
            Assert.That(token.Literal, Is.EqualTo(']'));
        });
    }

    [Test]
    public void Lex_ShouldReturnStringLiteralToken()
    {
        var parser = new Lexer("\"hello\"");
        var token = parser.Lex();
        
        Assert.Multiple(() =>
        {
            Assert.That(token.Kind, Is.EqualTo(TokenKind.StringLiteral));
            Assert.That(token.Literal, Is.EqualTo("hello"));
        });
    }

    [Test]
    public void Lex_ShouldReturnNumberLiteralToken()
    {
        var parser = new Lexer("123.45");
        var token = parser.Lex();
        
        Assert.Multiple(() =>
        {
            Assert.That(token.Kind, Is.EqualTo(TokenKind.NumberLiteral));
            Assert.That(token.Literal, Is.EqualTo(123.45));
        });
    }

    [Test]
    public void Lex_ShouldReturnBooleanLiteralToken()
    {
        var parser = new Lexer("true");
        var token = parser.Lex();
        
        Assert.Multiple(() =>
        {
            Assert.That(token.Kind, Is.EqualTo(TokenKind.BooleanLiteral));
            Assert.That(token.Literal, Is.EqualTo(true));
        });
    }
    
    [Test]
    public void Lex_ShouldReturnEOFToken()
    {
        var parser = new Lexer("            ");
        var token = parser.Lex();
        Assert.Multiple(() =>
        {
            Assert.That(token.Kind, Is.EqualTo(TokenKind.EOF));
            Assert.That(token.Literal, Is.EqualTo('\0'));
        });
    }
    
    [Test]
    public void Lex_ShouldReturnNullToken()
    {
        var lexer = new Lexer("null");
        var token = lexer.Lex();

        Assert.Multiple(() =>
        {
            Assert.That(token.Kind, Is.EqualTo(TokenKind.Null));
            Assert.That(token.Literal, Is.Null);
        });
    }
}