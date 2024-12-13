namespace Parser;

public enum TokenKind
{
    OpenCurlyBrace,             //  {
    CloseCurlyBrace,            //  }
    
    Comma,                     // ,
    Colon,                     // :
    
    OpenSquareBracket,         // [
    CloseSquareBracket,        // ]
    
    StringLiteral,             // "string"
    NumberLiteral,             // 123
    BooleanLiteral,           // true or false
    Null,                     // null
    
    EOF,                      // End of file
}