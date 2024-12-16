```markdown
# JSON Parser

A simple JSON parser written in C# that can parse JSON strings into corresponding C# objects. This project includes a lexer and parser to handle various JSON data types such as strings, numbers, booleans, null, arrays, and objects.

## Features

- Parse JSON strings into C# objects
- Supports string, number, boolean, null, array, and object literals
- Includes unit tests for various JSON structures

## Getting Started

### Prerequisites

- .NET SDK 6.0 or later

### Installation

1. Clone the repository:
    ```sh
    git clone https://github.com/yourusername/json-parser.git
    cd json-parser
    ```

2. Build the project:
    ```sh
    dotnet build
    ```

### Usage

To use the JSON parser, run the following command with a JSON string as an argument:
```sh
dotnet run --project Parser/Program.cs -- "{ \"key\": \"value\" }"
```

### Running Tests

To run the unit tests, use the following command:
```sh
dotnet test
```

## Project Structure

- `Parser/`: Contains the main lexer and parser implementation.
- `Parser.Tests/`: Contains unit tests for the parser.
- `Parser/Program.cs`: Entry point for running the parser from the command line.

## Example

Here is an example of how to use the JSON parser in your C# code:

```csharp
using Parser;

var jsonStr = "{\"name\": \"John\", \"age\": 30, \"isStudent\": false}";
var lexer = new Lexer(jsonStr);
var parser = new Parser(lexer);
var result = parser.Parse();

Console.WriteLine(result);
```

## Contributing

Contributions are welcome! Please open an issue or submit a pull request for any improvements or bug fixes.

## License

This project is licensed under the MIT License. See the `LICENSE` file for details.
```
