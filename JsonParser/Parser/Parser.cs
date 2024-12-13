namespace Parser;

public class Parser(Lexer lexer)
{
    private Token _currentToken = lexer.Lex();
    private Token _nextToken = lexer.Lex();

    public object Parse()
    {
        return _currentToken.Kind switch
        {
            TokenKind.StringLiteral or TokenKind.NumberLiteral or TokenKind.BooleanLiteral => _currentToken.Literal,
            TokenKind.OpenCurlyBrace => MakeDictionary(),
            TokenKind.OpenSquareBracket => MakeArray(),
            TokenKind.Null => null,
            _ => string.Empty
        };
    }

    private List<object> MakeArray()
    {
        var list = new List<object>();
        
        Next();
        
        while (_currentToken.Kind != TokenKind.CloseSquareBracket)
        {
            if (_currentToken.Kind == TokenKind.EOF) throw new FormatException("Unexpected end of file while parsing array.");
            if (_currentToken.Kind == TokenKind.Comma) throw new FormatException("Unexpected comma ',' while parsing array.");
            
            var value = Parse();
            list.Add(value);

            Next();
        
            if (_currentToken.Kind != TokenKind.Comma && _currentToken.Kind != TokenKind.CloseSquareBracket)
                throw new FormatException("Expected a comma ',' after each value in the array.");

            if (_currentToken.Kind == TokenKind.Comma)
            {
                if (_nextToken.Kind == TokenKind.CloseSquareBracket) 
                    throw new FormatException("Unexpected comma ',' before closing square bracket ']'.");
                Next();
            }
        }
        
        if (_currentToken.Kind != TokenKind.CloseSquareBracket) 
            throw new FormatException("Expected closing square bracket ']' for array.");

        return list;
    }

    private Dictionary<string, object> MakeDictionary()
    {
        var dict = new Dictionary<string, object>();
        
        Next();
        
        while (_currentToken.Kind != TokenKind.CloseCurlyBrace)
        {
            if (_currentToken.Kind != TokenKind.StringLiteral)  throw new FormatException("Invalid object format");

            var key = (string)_currentToken.Literal;
            Next();

            if (_currentToken.Kind != TokenKind.Colon) throw new FormatException("Invalid object format");
            
            Next(); 
            var value = Parse();
            dict.Add(key, value);

            Next();
            
            if (_currentToken.Kind == TokenKind.Comma)
            {
                if (_nextToken.Kind == TokenKind.CloseCurlyBrace) 
                    throw new FormatException("Unexpected comma ',' before closing curly brace '}'.");
                Next();
            }
        }

        return dict;
    }

    private void Next()
    {
        _currentToken = _nextToken;
        _nextToken = lexer.Lex();
    }
}