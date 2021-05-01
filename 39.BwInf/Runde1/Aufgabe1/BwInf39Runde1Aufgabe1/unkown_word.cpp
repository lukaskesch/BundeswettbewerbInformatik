#include "unkown_word.h"

vector<pair<int, int>> unkown_word::letter_intervals;
vector<pair<int, int>> unkown_word::symbol_intervals;


unkown_word::unkown_word(string input, int index)
{
	index_in_output = index;
	for (int i = 0; i < input.length(); i++)
	{
		char current_char = input[i];
		if (current_char == '_')
		{
			continue;
		}

		int asci_code = static_cast<unsigned char>(current_char);

		bool is_letter = is_in_intervals(letter_intervals, asci_code);
		if (is_letter)
		{
			given_character = current_char;
			index_given_char = i;
			continue;
		}

		bool is_symbol = is_in_intervals(symbol_intervals, asci_code);
		if (is_symbol)
		{
			postfix.push_back(current_char);
			input.erase(i, 1);
			continue;
		}

		bool is_umlaut = asci_code > 127;
		if (is_umlaut)
		{
			int special_char_number = static_cast<unsigned char>(input[i + 1]);
			umlaut_identifiers[i] = special_char_number;

			input.erase(i, 2);
			input.insert(i, "$");
		}
	}

	length = input.length();
}

wstring unkown_word::print()
{
	return (*solution).print_word().append(postfix);
}

int unkown_word::get_length()
{
	return length;
}
int unkown_word::get_index_given_char()
{
	return index_given_char;
}
char unkown_word::get_given_char()
{
	return given_character;
}

int unkown_word::get_umlaut_code(int index)
{
	return this->umlaut_identifiers[index];
}

