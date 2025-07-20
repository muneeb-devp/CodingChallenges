package main

import (
	"fmt"
	"os"
	"path/filepath"
	"strings"
)

type WhichResult struct {
	Command string
	Path    string
	Found   bool
}


func findCommandInPath(command string, pathDirs []string) *WhichResult {
	for _, dir := range pathDirs {
		fullPath := filepath.Join(dir, command)
		if _, err := os.Stat(fullPath); err == nil {
			return &WhichResult{
				Command: command,
				Path:    dir,
				Found:   true,
			}
		}
	}
	return &WhichResult{
		Command: command,
		Found:   false,
	}
}

func parsePathEnv(pathEnv string) []string {
	if pathEnv == "" {
		return []string{}
	}
	return strings.Split(pathEnv, ":")
}

// searchCommands searches for multiple commands in PATH directories
func searchCommands(commands []string, pathEnv string) []WhichResult {
	pathDirs := parsePathEnv(pathEnv)
	results := make([]WhichResult, 0, len(commands))
	
	for _, command := range commands {
		result := findCommandInPath(command, pathDirs)
		results = append(results, *result)
	}
	
	return results
}

func printResults(results []WhichResult) {
	for _, result := range results {
		if result.Found {
			fmt.Printf("%s\n", result.Path)
		} else {
			fmt.Printf("%s not found in PATH directories\n", result.Command)
		}
	}
}

func main() {
	args := os.Args[1:]

	if len(args) == 0 {
		fmt.Println("No arguments provided.")
		return
	}
	
	pathEnv := os.Getenv("PATH")
	results := searchCommands(args, pathEnv)
	printResults(results)
	
	os.Exit(0)
}