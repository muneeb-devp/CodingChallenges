using Parser;
using System.Linq;

try
{
  var jsonStr = Environment.GetCommandLineArgs().Skip(1).FirstOrDefault();
  
  if (string.IsNullOrEmpty(jsonStr))
  {
    Console.Error.WriteLine("No JSON string provided");
    Environment.Exit(1);
  }
  
  var lexer = new Lexer(jsonStr);
  var parser = new Parser.Parser(lexer);
  var result = parser.Parse();
  
  Console.WriteLine("Valid JSON string");
  Environment.Exit(0);
}
catch (FormatException ex)
{
  Console.Error.WriteLine($"Invalid JSON string: \n{ex.Message}");
  Environment.Exit(1);
}
catch (Exception ex)
{
  Console.Error.WriteLine($"An error occurred: \n{ex.Message}");
  Environment.Exit(1);
}

