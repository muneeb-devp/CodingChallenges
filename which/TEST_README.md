# Which Command - Unit Tests

This directory contains comprehensive unit tests for the `which` command implementation in Go.

## Test Coverage

The test suite includes:

### Core Function Tests (`main_test.go`)

- **`TestParsePathEnv`**: Tests PATH environment variable parsing with various formats
- **`TestFindCommandInPath`**: Tests command discovery in PATH directories using temporary test files
- **`TestSearchCommands`**: Tests searching for multiple commands simultaneously
- **`TestSearchCommandsWithEmptyPath`**: Edge case testing with empty PATH
- **`TestSearchCommandsIntegration`**: Real-world integration test using system commands

### Output and Edge Case Tests (`output_test.go`)

- **`TestPrintResults`**: Tests output formatting for found/not found commands
- **`TestMainFunction`**: Tests main function logic (argument handling)
- **`TestWhichResultStruct`**: Tests the WhichResult data structure
- **`TestEdgeCases`**: Tests edge cases like special characters, trailing colons, long names

### Example and Documentation (`example_test.go`)

- **`TestExampleUsage`**: Demonstrates typical usage patterns
- Includes documentation for running tests

### Benchmark Tests

- **`BenchmarkParsePathEnv`**: Performance testing for PATH parsing
- **`BenchmarkFindCommandInPath`**: Performance testing for command searching

## Running Tests

### Basic test execution:

```bash
go test                    # Run all tests
go test -v                 # Verbose output
go test -cover             # With coverage report
go test -v -cover          # Verbose with coverage
```

### Specific test execution:

```bash
go test -v -run TestParsePathEnv     # Run specific test
go test -v -run TestFind*            # Run tests matching pattern
```

### Benchmark testing:

```bash
go test -bench=.           # Run all benchmarks
go test -bench=Parse       # Run specific benchmarks
```

### Coverage analysis:

```bash
go test -coverprofile=coverage.out
go tool cover -html=coverage.out    # Generate HTML report
```

### Multiple test runs:

```bash
go test -count=5           # Run tests 5 times
go test -race              # Test for race conditions
```

## Test Structure

The tests follow Go testing best practices:

1. **Table-driven tests**: Using test cases in slices for comprehensive coverage
2. **Temporary directories**: Creating isolated test environments
3. **Subtests**: Using `t.Run()` for organized test execution
4. **Output capture**: Testing functions that write to stdout
5. **Integration tests**: Testing with real system commands
6. **Benchmarks**: Performance measurement
7. **Edge case testing**: Boundary conditions and error cases

## Test Coverage

Current coverage: **66.7%** of statements

The tests cover:

- ✅ All core business logic functions
- ✅ Error handling and edge cases
- ✅ Output formatting
- ✅ PATH parsing with various formats
- ✅ Command discovery logic
- ✅ Integration with real system

Not covered (by design):

- `os.Exit(0)` call in main (requires subprocess testing)
- Some error paths that are hard to trigger in tests

## Test Files

- `main_test.go`: Core functionality tests
- `output_test.go`: Output and edge case tests
- `example_test.go`: Usage examples and documentation

## Dependencies

Tests use only Go standard library:

- `testing`: Test framework
- `os`: File system operations
- `path/filepath`: Path handling
- `reflect`: Deep equality testing
- `bytes`, `io`: Output capture
