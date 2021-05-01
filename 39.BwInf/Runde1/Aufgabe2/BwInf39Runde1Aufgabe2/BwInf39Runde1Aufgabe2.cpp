#include "BwInf39Runde1Aufgabe2.h"


void print_user_greetings()
{
	cout << "Hello there!" << endl;
	cout << "This programm will loop through all the given puzzles and print their solution." << endl;
}

void read_input(int number)
{
	string file_name = "examples/puzzel";
	file_name.append(to_string(number));
	file_name.append(".txt");
	cout << endl << "file: " << file_name << endl;
	ifstream input_file_stream(file_name);

	string dummy;
	getline(input_file_stream, dummy);
	getline(input_file_stream, dummy);

	int left, right, center;
	for (int i = 0; i < 9; i++)
	{
		input_file_stream >> left;
		input_file_stream >> right;
		input_file_stream >> center;

		Pieces[i].push_back(piece(left, right, center, (i * 10 + 0)));
		Pieces[i].push_back(piece(right, center, left, (i * 10 + 1)));
		Pieces[i].push_back(piece(center, left, right, (i * 10 + 2)));
	}
}

void solve(int index)
{
	index++;
	bool valid_new_piece = check_new_piece(index - 1);
	bool reached_end = index == 9;
	if (found_solution)
	{
		return;
	}
	else if (!valid_new_piece)
	{
		return;
	}
	else if (reached_end)
	{
		save_solution();
		found_solution = true;
		return;
	}

	for (int i = 0; i < 9; i++)
	{
		bool piece_in_use = Pieces_in_use[i];
		if (piece_in_use)
		{
			continue;
		}
		for (int j = 0; j < 3; j++)
		{
			Pieces_in_use[i] = true;
			Puzzel[index] = &Pieces[i][j];
			solve(index);
			Pieces_in_use[i] = false;
			Puzzel[index] = nullptr;
		}
	}
}

bool check_new_piece(int index)
{
	bool match;
	switch (index)
	{
	case -1:
		return true;
	case 0:
		return true;
	case 1:
		match = (*Puzzel[0]).right == -(*Puzzel[1]).right;
		if (match)
			return true;
		break;
	case 2:
		match = (*Puzzel[1]).left == -(*Puzzel[2]).left;
		if (match)
			return true;
		break;
	case 3:
		match = (*Puzzel[2]).right == -(*Puzzel[3]).right;
		if (match)
			return true;
		break;
	case 4:
		match = (*Puzzel[3]).left == -(*Puzzel[4]).left;
		if (match)
			return true;
		break;
	case 5:
		match = (*Puzzel[1]).center == -(*Puzzel[5]).center;
		if (match)
			return true;
		break;
	case 6:
		match = (*Puzzel[5]).right == -(*Puzzel[6]).right;
		if (match)
			return true;
		break;
	case 7:
		match = (*Puzzel[6]).left == -(*Puzzel[7]).left && (*Puzzel[3]).center == -(*Puzzel[7]).center;
		if (match)
			return true;
		break;
	case 8:
		match = (*Puzzel[6]).center == -(*Puzzel[8]).center;
		if (match)
			return true;
		break;
	default:
		return false;
	}
	return false;
}

void save_solution()
{
	for (int i = 0; i < 9; i++)
	{
		Solution[i] = Puzzel[i];
	}
}

void print_solution()
{
	chrono::duration<double> elapsed = finish_time - start_time;
	cout << "Elapsed time: " << elapsed.count() << " s" << endl;

	bool no_solution_found = Solution[0] == nullptr;
	if (no_solution_found)
	{
		cout << "No solution has been found";
		return;
	}

	cout << "Index | Piece number | left, right, and center values" << endl;
	for (int i = 0; i < 9; i++)
	{
		cout << to_string(i) << " -> " << to_string((*Solution[i]).id / 10) << ": " << to_string((*Solution[i]).left) << " " << to_string((*Solution[i]).right) << " " << to_string((*Solution[i]).center) << endl;
	}
}

void cleanup()
{
	found_solution = false;

	for (int i = 0; i < 9; i++)
	{
		Pieces_in_use[i] = false;
		Puzzel[i] = nullptr;
		Solution[i] = nullptr;

		Pieces[i].clear();
	}
}

int main()
{
	print_user_greetings();

	for (int i = 0; i < number_of_tests; i++)
	{
		read_input(i);

		start_time = chrono::high_resolution_clock::now();
		solve(-1);
		finish_time = chrono::high_resolution_clock::now();

		print_solution();
		cleanup();
	}

	string dummy;
	cin >> dummy;

	return 0;
}

