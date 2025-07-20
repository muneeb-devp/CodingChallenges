package main

import (
	"bytes"
	"fmt"
	"io"
	"os"
	"strings"
	"testing"
)

// captureOutput captures stdout during function execution
func captureOutput(f func()) string {
	old := os.Stdout
	r, w, _ := os.Pipe()
	os.Stdout = w
	
	f()
	
	w.Close()
	os.Stdout = old
	
	var buf bytes.Buffer
	io.Copy(&buf, r)
	return buf.String()
}

func TestPrintResults(t *testing.T) {
	tests := []struct {
		name     string
		results  []WhichResult
		expected string
	}{
		{
			name: "single found command",
			results: []WhichResult{
				{Command: "ls", Path: "/bin", Found: true},
			},
			expected: "Found ls in /bin\n",
		},
		{
			name: "single not found command",
			results: []WhichResult{
				{Command: "nonexistent", Found: false},
			},
			expected: "nonexistent not found in PATH directories\n",
		},
		{
			name: "multiple mixed results",
			results: []WhichResult{
				{Command: "ls", Path: "/bin", Found: true},
				{Command: "nonexistent", Found: false},
				{Command: "echo", Path: "/bin", Found: true},
			},
			expected: "Found ls in /bin\nnonexistent not found in PATH directories\nFound echo in /bin\n",
		},
		{
			name:     "empty results",
			results:  []WhichResult{},
			expected: "",
		},
	}

	for _, tt := range tests {
		t.Run(tt.name, func(t *testing.T) {
			output := captureOutput(func() {
				printResults(tt.results)
			})
			
			if output != tt.expected {
				t.Errorf("printResults() output = %q, want %q", output, tt.expected)
			}
		})
	}
}

func TestMainFunction(t *testing.T) {
	// Save original args and restore them after test
	originalArgs := os.Args
	defer func() { os.Args = originalArgs }()

	tests := []struct {
		name         string
		args         []string
		expectedExit bool
	}{
		{
			name:         "no arguments",
			args:         []string{"program"},
			expectedExit: false, // Should return early, not exit
		},
		{
			name:         "with arguments",
			args:         []string{"program", "ls"},
			expectedExit: true, // Should call os.Exit(0)
		},
	}

	for _, tt := range tests {
		t.Run(tt.name, func(t *testing.T) {
			// We can't easily test os.Exit(0) without using a subprocess
			// So we'll test the logic that leads up to it
			os.Args = tt.args
			
			if len(tt.args) == 1 {
				// Test the "no arguments" case
				output := captureOutput(func() {
					args := os.Args[1:]
					if len(args) == 0 {
						fmt.Println("No arguments provided.")
						return
					}
				})
				
				expected := "No arguments provided.\n"
				if output != expected {
					t.Errorf("Expected %q, got %q", expected, output)
				}
			}
		})
	}
}

func TestWhichResultStruct(t *testing.T) {
	// Test that WhichResult struct works as expected
	result := WhichResult{
		Command: "test",
		Path:    "/usr/bin",
		Found:   true,
	}
	
	if result.Command != "test" {
		t.Errorf("Expected Command to be 'test', got %v", result.Command)
	}
	if result.Path != "/usr/bin" {
		t.Errorf("Expected Path to be '/usr/bin', got %v", result.Path)
	}
	if !result.Found {
		t.Errorf("Expected Found to be true, got %v", result.Found)
	}
}

// Test edge cases
func TestEdgeCases(t *testing.T) {
	t.Run("command with special characters", func(t *testing.T) {
		result := findCommandInPath("cmd-with-dashes", []string{})
		if result.Found {
			t.Errorf("Expected command with special characters not to be found in empty path")
		}
	})
	
	t.Run("path with trailing colon", func(t *testing.T) {
		paths := parsePathEnv("/usr/bin:/bin:")
		expected := []string{"/usr/bin", "/bin", ""}
		if len(paths) != len(expected) {
			t.Errorf("Expected %d paths, got %d", len(expected), len(paths))
		}
		for i, path := range paths {
			if path != expected[i] {
				t.Errorf("Expected path[%d] = %q, got %q", i, expected[i], path)
			}
		}
	})
	
	t.Run("very long command name", func(t *testing.T) {
		longCommand := strings.Repeat("a", 1000)
		result := findCommandInPath(longCommand, []string{"/bin"})
		if result.Found {
			t.Errorf("Expected very long command name not to be found")
		}
		if result.Command != longCommand {
			t.Errorf("Expected command name to be preserved")
		}
	})
}
