using System.Text;

namespace Parser;

public class Lexer
{
    private int _position;
    private char _currentChar;
    
    private readonly string _input;
     
    public Lexer(string input) 
    {
        _input = input ?? throw new ArgumentNullException(nameof(input));
        _position = 0;
        _currentChar = '\0';
        
        Next();
    }

    public Token Lex()
    {
        SkipWhitespace();

        var token = _currentChar switch
        {
            '{' => new Token(TokenKind.OpenCurlyBrace, _currentChar),
            '}' => new Token(TokenKind.CloseCurlyBrace, _currentChar),
            ',' => new Token(TokenKind.Comma, _currentChar),
            ':' => new Token(TokenKind.Colon, _currentChar),
            '[' => new Token(TokenKind.OpenSquareBracket, _currentChar),
            ']' => new Token(TokenKind.CloseSquareBracket, _currentChar),
            '"' => LexString(),
            't' or 'f' => LexBoolean(),
            'n' => LexNull(),
            
            _ => char.IsDigit(_currentChar) ? LexNumber() : new Token(TokenKind.EOF, '\0')
        };
        
        Next();
        
        return token;
    }

    private Token LexNumber()
    {
        var sb = new StringBuilder();
        var hasDecimalPoint = false;

        while (char.IsDigit(_currentChar) || _currentChar == '.')
        {
            if (_currentChar == '.')
            {
                if (hasDecimalPoint) throw new FormatException("Invalid number format");
                hasDecimalPoint = true;
            }
            sb.Append(_currentChar);
            
            Next();
        }

        Reverse();
        
        return new Token(TokenKind.NumberLiteral, double.Parse(sb.ToString()));
    }

    private Token LexString()
    {
        var sb = new StringBuilder();
        Next();
        
        while (_currentChar != '"' && _currentChar != '\0')
        {
            sb.Append(_currentChar);
            Next();
        }

        if (_currentChar == '\0')  throw new Exception("Unterminated string");
        
        return new Token(TokenKind.StringLiteral, sb.ToString());
    }
    
    private Token LexNull()
    {
        if (_input.Substring(_position - 1, 4) == "null")
        {
            _position += 3;
            return new Token(TokenKind.Null, null);
        }

        return new Token(TokenKind.EOF, '\0');
    }

    private Token LexBoolean()
    {
        if (_input.Substring(_position - 1, 4) == "true")
        {
            _position += 3;
            return new Token(TokenKind.BooleanLiteral, true);
        }
        if (_input.Substring(_position - 1, 5) == "false")
        {
            _position += 4;
            return new Token(TokenKind.BooleanLiteral, false);
        }
        
        return new Token(TokenKind.EOF, '\0');
    }
    
    private void Next()
    {
        _currentChar = _position < _input.Length ? _input[_position] : '\0';
        
        _position++;
    }

    private void Reverse()
    {
        _position = Math.Max(0, _position - 1);
        _currentChar = _position < _input.Length ? _input[_position] : '\0';
    }
    
    private void SkipWhitespace()
    {
        while (char.IsWhiteSpace(_currentChar)) Next();
    }
}