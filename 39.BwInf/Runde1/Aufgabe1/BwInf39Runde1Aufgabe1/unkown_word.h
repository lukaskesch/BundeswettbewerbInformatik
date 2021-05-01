#pragma once

#include <string>
#include <vector>
#include <map>
#include <iostream>

#include "given_word.h"

using namespace std;

class unkown_word
{

	//Variables
private:
	int length = 0;
	int index_given_char = -1;
	char given_character = ' ';
	wstring postfix = L"";
	map<int, int> umlaut_identifiers;

public:
	vector<given_word*> possible_words;
	given_word* solution = nullptr;
	bool solved = false;
	int index_in_output;

	static vector<pair<int, int>> letter_intervals;
	static vector<pair<int, int>> symbol_intervals;

	//Methods
private:
	static bool is_in_intervals(vector<pair<int, int>> intervals, int number)
	{
		for (int i = 0; i < intervals.size(); i++)
		{
			bool out_of_bounce = number < intervals[i].first || number > intervals[i].second;
			if (!out_of_bounce)
			{
				return true;
			}
		}
		return false;
	}

public:
	unkown_word(string input, int index);

	wstring print();
	int get_length();
	int get_index_given_char();
	char get_given_char();
	int get_umlaut_code(int index);


	friend bool operator<(unkown_word word_1, unkown_word word_2)
	{
		return word_1.length > word_2.length;
	}

	static void preparation() {
		letter_intervals.push_back(pair<int, int>(0, 0));
		letter_intervals.push_back(pair<int, int>(65, 90));
		letter_intervals.push_back(pair<int, int>(97, 122));

		symbol_intervals.push_back(pair<int, int>(33, 64));
		symbol_intervals.push_back(pair<int, int>(91, 96));
		symbol_intervals.push_back(pair<int, int>(123, 126));
	}

};