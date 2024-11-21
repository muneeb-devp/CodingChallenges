# wc clone

This project is a Python implementation of the Unix command line tool `wc`

## Features

- Count the number of bytes in a file.
- Count the number of lines in a file.
- Count the number of words in a file.
- Count the number of characters in a file.

## Usage

To use this tool, run the `main.py` script with the appropriate arguments.

### Command Line Arguments

- `file`: The file to be processed.
- `-c`: Count bytes.
- `-l`: Count lines.
- `-w`: Count words.
- `-m`: Count characters.

### Examples

Count the number of bytes in a file:

```sh
python main.py -c example.txt
```

Count the number of lines in a file:

```sh
python main.py -l example.txt
```

Count the number of words in a file:

```sh
python main.py -w example.txt
```

Count the number of characters in a file:

```sh
python main.py -m example.txt
```

If no specific count option is provided, the tool will count bytes, lines, and words by default:

```sh
python main.py example.txt
```

## Requirements

- Python 3.x

## Installation

Clone the repository and navigate to the project directory:

```sh
git clone https://github.com/yourusername/wc-clone.git
cd wc-clone
```

Run the script with the desired options as shown in the usage examples.

## License

This project is licensed under the MIT License.
