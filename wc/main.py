import argparse


def get_num_bytes(file: str) -> int:
  with open(file, 'rb') as f:
    return sum(len(line) for line in f)


def get_num_lines(file: str) -> int:
  with open(file, 'r') as f:
    return sum(1 for _ in f)


def get_num_words(file: str) -> int:
  with open(file, 'r') as f:
    return sum(len(line.split()) for line in f)


def get_num_characters(file: str) -> int:
  with open(file, 'rb') as f:
    return len(f.read().decode('utf-8'))


def main() -> None:
  parser = argparse.ArgumentParser(
    description='Clone of unix command line tool wc.'
  )
  parser.add_argument('file', type=str, help='The file to be processed')
  parser.add_argument('-c', action='store_true', help='Count bytes')
  parser.add_argument('-l', action='store_true', help='Count lines')
  parser.add_argument('-w', action='store_true', help='Count words')
  parser.add_argument('-m', action='store_true', help='Count characters')

  args = parser.parse_args()

  if not any([args.c, args.l, args.w, args.m]):
    args.c = args.l = args.w = True

  file = args.file

  if args.c: print(get_num_bytes(args.file), file)
  if args.l: print(get_num_lines(args.file), file)
  if args.w: print(get_num_words(args.file), file)
  if args.m: print(get_num_characters(args.file), file)


if __name__ == '__main__':
  main()
