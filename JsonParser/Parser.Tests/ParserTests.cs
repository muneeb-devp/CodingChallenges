namespace Parser.Tests;

[TestFixture]
public class ParserTests
{
    [Test]
    public void Parse_StringLiteral_ReturnsString()
    {
        var lexer = new Lexer("\"hello\"");
        var parser = new Parser(lexer);
        var result = parser.Parse();
        
        Assert.That(result, Is.EqualTo("hello"));
    }

    [Test]
    public void Parse_NumberLiteral_ReturnsNumber()
    {
        var lexer = new Lexer("123.45");
        var parser = new Parser(lexer);
        var result = parser.Parse();
        
        Assert.That(result, Is.EqualTo(123.45));
    }

    [Test]
    public void Parse_BooleanLiteral_ReturnsBoolean()
    {
        var lexer = new Lexer("true");
        var parser = new Parser(lexer);
        var result = parser.Parse();
        
        Assert.That(result, Is.EqualTo(true));
    }

    [Test]
    public void Parse_NullLiteral_ReturnsNull()
    {
        var lexer = new Lexer("null");
        var parser = new Parser(lexer);
        var result = parser.Parse();
        
        Assert.That(result, Is.EqualTo(null));
    }

    [Test]
    public void Parse_EmptyArray_ReturnsEmptyList()
    {
        var lexer = new Lexer("[]");
        var parser = new Parser(lexer);
        var result = parser.Parse();
        
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.InstanceOf<List<object>>());
            Assert.That(((List<object>)result), Is.Empty);
        });
    }

    [Test]
    public void Parse_ArrayWithValues_ReturnsList()
    {
        var lexer = new Lexer("[\"hello\", 123, true, null]");
        var parser = new Parser(lexer);
        var result = parser.Parse();
        var list = (List<object>)result;

        Assert.That(list, Has.Count.EqualTo(4));
        Assert.Multiple(() =>
        {
            Assert.That(list[0], Is.EqualTo("hello"));
            Assert.That(list[1], Is.EqualTo(123.0));
            Assert.That(list[2], Is.EqualTo(true));
            Assert.That(list[3], Is.EqualTo(null));
        });
    }

    [Test]
    public void Parse_EmptyObject_ReturnsEmptyDictionary()
    {
        var lexer = new Lexer("{}");
        var parser = new Parser(lexer);
        var result = parser.Parse();
        Assert.That(result, Is.InstanceOf<Dictionary<string, object>>());
        Assert.That((Dictionary<string, object>)result, Is.Empty);
    }

    [Test]
    public void Parse_ObjectWithValues_ReturnsDictionary()
    {
        var lexer = new Lexer("{\"key1\": \"value1\", \"key2\": 123, \"key3\": true, \"key4\": null}");
        var parser = new Parser(lexer);
        var result = parser.Parse();
        var dict = (Dictionary<string, object>)result;

        Assert.That(dict, Has.Count.EqualTo(4));
        Assert.Multiple(() =>
        {
            Assert.That(dict["key1"], Is.EqualTo("value1"));
            Assert.That(dict["key2"], Is.EqualTo(123.0));
            Assert.That(dict["key3"], Is.EqualTo(true));
            Assert.That(dict["key4"], Is.EqualTo(null));
        });
    }

    [Test]
    public void Parser_RunAllTests()
    {

        string projectRoot = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;

        foreach (var testDir in Directory.EnumerateDirectories(Path.Join(projectRoot, "tests")))
        {
            foreach (var file in Directory.EnumerateFiles(testDir))
            {
                if (file.Contains("invalid"))
                {
                    var input = File.ReadAllText(file);
                    
                    if (string.IsNullOrEmpty(input)) continue;
                    
                    var lexer = new Lexer(input);
                    var parser = new Parser(lexer);
                    Assert.That(() => parser.Parse(), Throws.Exception);
                }
                else if (file.Contains("valid"))
                {
                    var input = File.ReadAllText(file);
                    var lexer = new Lexer(input);
                    var parser = new Parser(lexer);
                    var result = parser.Parse();
                    Assert.That(result, Is.Not.Null);
                }
            }
        }


        Console.WriteLine(projectRoot);
    }
    
}