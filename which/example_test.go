// Example test file showing how to run the tests
// Run with: go test -v
// Run with coverage: go test -v -cover
// Run benchmarks: go test -bench=.
// Run specific test: go test -v -run TestParsePathEnv

package main

import (
	"testing"
)

// Example of how to run tests from command line:
//
// Basic test run:
//   go test
//
// Verbose output:
//   go test -v
//
// With coverage:
//   go test -cover
//
// With detailed coverage:
//   go test -coverprofile=coverage.out
//   go tool cover -html=coverage.out
//
// Run specific test:
//   go test -v -run TestParsePathEnv
//
// Run benchmarks:
//   go test -bench=.
//
// Run tests multiple times:
//   go test -count=5

func TestExampleUsage(t *testing.T) {
	// Example of how the functions work together
	pathEnv := "/usr/bin:/bin"
	commands := []string{"echo", "nonexistent"}
	
	results := searchCommands(commands, pathEnv)
	
	if len(results) != 2 {
		t.Errorf("Expected 2 results, got %d", len(results))
		return
	}
	
	// First command should be found
	if results[0].Command != "echo" {
		t.Errorf("Expected first command to be 'echo', got %v", results[0].Command)
	}
	
	// Second command should not be found
	if results[1].Found {
		t.Errorf("Expected 'nonexistent' command not to be found")
	}
	
	t.Logf("Results: %+v", results)
}
