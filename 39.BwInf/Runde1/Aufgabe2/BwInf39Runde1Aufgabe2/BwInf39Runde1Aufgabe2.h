#pragma once
#include <string>
#include <vector>
#include <iostream>
#include <sstream>
#include <fstream>
#include <chrono>  // for high_resolution_clock

#include "piece.h"

using namespace std;

//Variables
auto start_time = std::chrono::high_resolution_clock::now();
auto finish_time = std::chrono::high_resolution_clock::now();
const int number_of_tests = 4;
bool found_solution = false;
vector<bool> Pieces_in_use(9);
vector<vector<piece>> Pieces(9);
vector<piece*> Puzzel(9);
vector<piece*> Solution(9);

//Methods
void print_user_greetings();
void read_input(int number);
void solve(int index);
bool check_new_piece(int index);
void save_solution();
void print_solution();
void cleanup();
int main();