import unittest
from main import (
  get_num_bytes, get_num_lines,
  get_num_words, get_num_characters
)


class TestFileFunctions(unittest.TestCase):
  @classmethod
  def setUpClass(cls):
    cls.test_file = './test.txt'

  def test_get_num_bytes(self):
    self.assertEqual(get_num_bytes(self.test_file), 342_190)

  def test_get_num_lines(self):
    self.assertEqual(get_num_lines(self.test_file), 7145)

  def test_get_num_words(self):
    self.assertEqual(get_num_words(self.test_file), 58164)

  def test_get_num_characters(self):
    self.assertEqual(get_num_characters(self.test_file), 339_292)


if __name__ == '__main__':
  unittest.main()
