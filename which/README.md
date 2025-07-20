# Which Command Implementation in Go

A Go implementation of the Unix `which` command that locates executables in the system PATH. This tool helps you find where commands are installed on your system.

## Overview

The `which` command searches through the directories listed in your PATH environment variable to find executable files. When you type a command in your terminal, the shell uses a similar process to locate the program to execute.

This implementation:

- ✅ Searches all directories in the PATH environment variable
- ✅ Supports searching for multiple commands at once
- ✅ Returns the full path to the directory containing each executable
- ✅ Handles commands that don't exist gracefully
- ✅ Cross-platform compatible (Unix/Linux/macOS)

## Features

- **Multiple Command Search**: Search for several commands in a single invocation
- **PATH Parsing**: Correctly parses and handles PATH environment variable
- **Error Handling**: Clear feedback when commands are not found
- **Performance**: Efficient search with early termination when command is found
- **Clean Output**: Simple, parseable output format

## Installation

### Method 1: Build from Source

```bash
# Clone or download the source code
cd /path/to/which
go build -o which main.go
```

### Method 2: Run Directly

```bash
go run main.go [commands...]
```

### Method 3: Install Globally

```bash
# Build and install to GOPATH/bin
go install .

# Or build and copy to a directory in your PATH
go build -o which main.go
sudo mv which /usr/local/bin/
```

## Usage

### Basic Usage

```bash
# Find a single command
./which ls
# Output: /bin

# Find multiple commands
./which ls echo cat
# Output:
# /bin
# /bin
# /bin
```

### Command Line Arguments

```bash
# Using the compiled binary
./which [command1] [command2] [command3] ...

# Using go run
go run main.go [command1] [command2] [command3] ...
```

### Examples

#### Finding System Commands

```bash
./which ls echo cat grep
```

**Output:**

```
/bin
/bin
/bin
/usr/bin
```

#### Checking Development Tools

```bash
./which python3 node npm go git
```

**Output:**

```
/usr/bin
node not found in PATH directories
npm not found in PATH directories
/usr/local/bin
/opt/homebrew/bin
```

#### Verifying Installation

```bash
./which docker kubectl terraform
```

**Output:**

```
/usr/local/bin
/usr/local/bin
terraform not found in PATH directories
```

#### No Arguments

```bash
./which
```

**Output:**

```
No arguments provided.
```

## How It Works

### PATH Environment Variable

The program reads your system's PATH environment variable, which contains a colon-separated list of directories where executable files are stored:

```bash
echo $PATH
# Example output: /usr/local/bin:/usr/bin:/bin:/usr/sbin:/sbin
```

### Search Algorithm

1. **Parse PATH**: Split the PATH variable by colons (`:`) to get individual directories
2. **For Each Command**: Iterate through each command argument
3. **Directory Search**: Check each PATH directory for the command file
4. **File Existence**: Use `os.Stat()` to verify the file exists
5. **Early Return**: Stop searching when the first match is found
6. **Result Output**: Print the directory path or "not found" message

### Code Structure

The program is organized into several key functions:

- **`main()`**: Entry point, handles command-line arguments
- **`searchCommands()`**: Orchestrates the search for multiple commands
- **`findCommandInPath()`**: Searches for a single command in PATH directories
- **`parsePathEnv()`**: Parses the PATH environment variable
- **`printResults()`**: Outputs the search results

## Output Format

### Found Commands

When a command is found, the program outputs the directory path where it was located:

```
/bin
/usr/local/bin
/opt/homebrew/bin
```

### Not Found Commands

When a command is not found, a clear error message is displayed:

```
nonexistent not found in PATH directories
```

### Mixed Results

The program handles mixed results gracefully:

```bash
./which ls nonexistent echo
```

**Output:**

```
/bin
nonexistent not found in PATH directories
/bin
```

## Comparison with System `which`

### Similarities

- Searches PATH environment variable
- Returns directory containing executable
- Handles multiple commands
- Clear not-found messages

### Differences

- **Output Format**: This implementation shows only the directory path, while system `which` shows the full path including filename
- **Exit Codes**: System `which` uses exit codes to indicate success/failure
- **Options**: System `which` has additional flags like `-a` (show all matches)

### Example Comparison

```bash
# System which command
which ls
# Output: /bin/ls

# This implementation
./which ls
# Output: /bin
```

## Building and Development

### Prerequisites

- Go 1.19+ installed
- Unix-like system (Linux, macOS) or Windows with proper PATH handling

### Build Commands

```bash
# Basic build
go build main.go

# Build with custom name
go build -o which main.go

# Build for different platforms
GOOS=linux GOARCH=amd64 go build -o which-linux main.go
GOOS=darwin GOARCH=amd64 go build -o which-darwin main.go
GOOS=windows GOARCH=amd64 go build -o which.exe main.go
```

### Running Tests

```bash
# Run all tests
go test

# Run tests with coverage
go test -cover

# Run tests verbosely
go test -v

# Run benchmarks
go test -bench=.
```

### Project Structure

```
which/
├── main.go              # Main program implementation
├── main_test.go         # Core functionality tests
├── output_test.go       # Output and edge case tests
├── example_test.go      # Usage examples and documentation
├── TEST_README.md       # Testing documentation
├── README.md           # This file
└── go.mod              # Go module definition
```

## Error Handling

The program handles various error conditions gracefully:

### No Arguments

```bash
./which
# Output: No arguments provided.
```

### Empty PATH

If the PATH environment variable is empty or unset, all commands will be reported as not found.

### Permission Issues

The program uses `os.Stat()` which respects file system permissions. Files that exist but are not accessible may not be found.

### Invalid Directory Paths

Non-existent directories in PATH are silently skipped.

## Performance

The implementation is optimized for performance:

- **Early Termination**: Stops searching when the first match is found
- **Efficient Path Parsing**: Minimal string operations
- **Memory Efficient**: Uses slices with appropriate capacity
- **No External Dependencies**: Uses only Go standard library

### Benchmarks

```bash
go test -bench=.
```

Example results:

```
BenchmarkParsePathEnv-10         11967940    86.38 ns/op
BenchmarkFindCommandInPath-10       87511    13682 ns/op
```

## Limitations

- **First Match Only**: Returns only the first directory where command is found
- **No Executable Bit Check**: Doesn't verify if files are actually executable
- **Unix PATH Format**: Uses colon (`:`) as separator (Windows uses semicolon `;`)
- **No Alias Resolution**: Doesn't handle shell aliases or functions

## Contributing

Contributions are welcome! Areas for improvement:

1. **Windows Support**: Handle Windows PATH format with semicolon separators
2. **Executable Verification**: Check file permissions/executable bit
3. **All Matches**: Option to show all locations where command exists
4. **Symlink Resolution**: Follow symbolic links to actual executables
5. **Performance**: Further optimization for large PATH variables

## License

This project is part of a coding challenges repository. See the main repository LICENSE file for details.

## See Also

- **Testing**: See `TEST_README.md` for comprehensive testing documentation
- **Unix Manual**: `man which` for system command documentation
- **PATH Variable**: Understanding shell PATH environment variable
