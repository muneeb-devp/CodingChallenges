package main

import (
	"os"
	"path/filepath"
	"reflect"
	"testing"
)

func TestParsePathEnv(t *testing.T) {
	tests := []struct {
		name     string
		pathEnv  string
		expected []string
	}{
		{
			name:     "empty PATH",
			pathEnv:  "",
			expected: []string{},
		},
		{
			name:     "single directory",
			pathEnv:  "/usr/bin",
			expected: []string{"/usr/bin"},
		},
		{
			name:     "multiple directories",
			pathEnv:  "/usr/bin:/bin:/usr/local/bin",
			expected: []string{"/usr/bin", "/bin", "/usr/local/bin"},
		},
		{
			name:     "directories with spaces",
			pathEnv:  "/usr/bin:/Applications/My App/bin:/bin",
			expected: []string{"/usr/bin", "/Applications/My App/bin", "/bin"},
		},
	}

	for _, tt := range tests {
		t.Run(tt.name, func(t *testing.T) {
			result := parsePathEnv(tt.pathEnv)
			if !reflect.DeepEqual(result, tt.expected) {
				t.Errorf("parsePathEnv() = %v, want %v", result, tt.expected)
			}
		})
	}
}

func TestFindCommandInPath(t *testing.T) {
	tempDir := t.TempDir()
	
	binDir := filepath.Join(tempDir, "bin")
	usrBinDir := filepath.Join(tempDir, "usr", "bin")
	emptyDir := filepath.Join(tempDir, "empty")
	
	err := os.MkdirAll(binDir, 0755)
	if err != nil {
		t.Fatal(err)
	}
	err = os.MkdirAll(usrBinDir, 0755)
	if err != nil {
		t.Fatal(err)
	}
	err = os.MkdirAll(emptyDir, 0755)
	if err != nil {
		t.Fatal(err)
	}
	
	testCmd1 := filepath.Join(binDir, "testcmd1")
	testCmd2 := filepath.Join(usrBinDir, "testcmd2")
	
	err = os.WriteFile(testCmd1, []byte("#!/bin/bash\necho test1"), 0755)
	if err != nil {
		t.Fatal(err)
	}
	err = os.WriteFile(testCmd2, []byte("#!/bin/bash\necho test2"), 0755)
	if err != nil {
		t.Fatal(err)
	}
	
	pathDirs := []string{binDir, usrBinDir, emptyDir}

	tests := []struct {
		name     string
		command  string
		expected WhichResult
	}{
		{
			name:    "command found in first directory",
			command: "testcmd1",
			expected: WhichResult{
				Command: "testcmd1",
				Path:    binDir,
				Found:   true,
			},
		},
		{
			name:    "command found in second directory",
			command: "testcmd2",
			expected: WhichResult{
				Command: "testcmd2",
				Path:    usrBinDir,
				Found:   true,
			},
		},
		{
			name:    "command not found",
			command: "nonexistent",
			expected: WhichResult{
				Command: "nonexistent",
				Found:   false,
			},
		},
	}

	for _, tt := range tests {
		t.Run(tt.name, func(t *testing.T) {
			result := findCommandInPath(tt.command, pathDirs)
			if result.Command != tt.expected.Command {
				t.Errorf("findCommandInPath().Command = %v, want %v", result.Command, tt.expected.Command)
			}
			if result.Found != tt.expected.Found {
				t.Errorf("findCommandInPath().Found = %v, want %v", result.Found, tt.expected.Found)
			}
			if tt.expected.Found && result.Path != tt.expected.Path {
				t.Errorf("findCommandInPath().Path = %v, want %v", result.Path, tt.expected.Path)
			}
		})
	}
}

func TestSearchCommands(t *testing.T) {
	tempDir := t.TempDir()
	
	binDir := filepath.Join(tempDir, "bin")
	usrBinDir := filepath.Join(tempDir, "usr", "bin")
	
	err := os.MkdirAll(binDir, 0755)
	if err != nil {
		t.Fatal(err)
	}
	err = os.MkdirAll(usrBinDir, 0755)
	if err != nil {
		t.Fatal(err)
	}
	
	testCmd1 := filepath.Join(binDir, "cmd1")
	testCmd2 := filepath.Join(usrBinDir, "cmd2")
	
	err = os.WriteFile(testCmd1, []byte("#!/bin/bash\necho test1"), 0755)
	if err != nil {
		t.Fatal(err)
	}
	err = os.WriteFile(testCmd2, []byte("#!/bin/bash\necho test2"), 0755)
	if err != nil {
		t.Fatal(err)
	}
	
	pathEnv := binDir + ":" + usrBinDir

	tests := []struct {
		name     string
		commands []string
		expected []WhichResult
	}{
		{
			name:     "single command found",
			commands: []string{"cmd1"},
			expected: []WhichResult{
				{Command: "cmd1", Path: binDir, Found: true},
			},
		},
		{
			name:     "multiple commands found",
			commands: []string{"cmd1", "cmd2"},
			expected: []WhichResult{
				{Command: "cmd1", Path: binDir, Found: true},
				{Command: "cmd2", Path: usrBinDir, Found: true},
			},
		},
		{
			name:     "mixed found and not found",
			commands: []string{"cmd1", "nonexistent", "cmd2"},
			expected: []WhichResult{
				{Command: "cmd1", Path: binDir, Found: true},
				{Command: "nonexistent", Found: false},
				{Command: "cmd2", Path: usrBinDir, Found: true},
			},
		},
		{
			name:     "empty command list",
			commands: []string{},
			expected: []WhichResult{},
		},
	}

	for _, tt := range tests {
		t.Run(tt.name, func(t *testing.T) {
			results := searchCommands(tt.commands, pathEnv)
			
			if len(results) != len(tt.expected) {
				t.Errorf("searchCommands() returned %d results, want %d", len(results), len(tt.expected))
				return
			}
			
			for i, result := range results {
				expected := tt.expected[i]
				if result.Command != expected.Command {
					t.Errorf("result[%d].Command = %v, want %v", i, result.Command, expected.Command)
				}
				if result.Found != expected.Found {
					t.Errorf("result[%d].Found = %v, want %v", i, result.Found, expected.Found)
				}
				if expected.Found && result.Path != expected.Path {
					t.Errorf("result[%d].Path = %v, want %v", i, result.Path, expected.Path)
				}
			}
		})
	}
}

func TestSearchCommandsWithEmptyPath(t *testing.T) {
	commands := []string{"anycommand"}
	results := searchCommands(commands, "")
	
	if len(results) != 1 {
		t.Errorf("Expected 1 result, got %d", len(results))
		return
	}
	
	if results[0].Found {
		t.Errorf("Expected command not to be found with empty PATH")
	}
	
	if results[0].Command != "anycommand" {
		t.Errorf("Expected command name 'anycommand', got %v", results[0].Command)
	}
}

func TestSearchCommandsIntegration(t *testing.T) {
	pathEnv := os.Getenv("PATH")
	if pathEnv == "" {
		t.Skip("Skipping integration test: PATH environment variable not set")
	}
	
	commands := []string{"sh"}
	results := searchCommands(commands, pathEnv)
	
	if len(results) != 1 {
		t.Errorf("Expected 1 result, got %d", len(results))
		return
	}
	
	if !results[0].Found {
		t.Errorf("Expected 'sh' command to be found in PATH")
	}
	
	if results[0].Command != "sh" {
		t.Errorf("Expected command name 'sh', got %v", results[0].Command)
	}
	
	if results[0].Path == "" {
		t.Errorf("Expected non-empty path for found command")
	}
}

func BenchmarkParsePathEnv(b *testing.B) {
	pathEnv := "/usr/bin:/bin:/usr/local/bin:/opt/homebrew/bin:/usr/sbin:/sbin"
	for i := 0; i < b.N; i++ {
		parsePathEnv(pathEnv)
	}
}

func BenchmarkFindCommandInPath(b *testing.B) {
	pathEnv := os.Getenv("PATH")
	pathDirs := parsePathEnv(pathEnv)
	
	b.ResetTimer()
	for i := 0; i < b.N; i++ {
		findCommandInPath("sh", pathDirs)
	}
}
