#pragma once

#include <string>
#include <map>
#include <sstream>


using namespace std;

class given_word
{
	//Variables
private:
	int length;
	string word;
	map<int, int> umlaut_identifiers;
public:
	bool assigned;


	//Methods
private:

public:
	given_word(string word);

	int get_length();
	string get_word();
	wstring print_word();
	wstring get_umlaut(int identifier);
	int get_umlaut_code(int index);

	friend bool operator<(given_word word_1, given_word word_2)
	{
		return word_1.length > word_2.length;
	}
};
