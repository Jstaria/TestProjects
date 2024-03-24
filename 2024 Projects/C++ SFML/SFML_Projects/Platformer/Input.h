#pragma once

#include <SFML/Graphics.hpp>
#include "GlobalVariables.h"
#include "FileIO.h"

enum InputState {
	Keyboard,
	Controller,
};

class Input
{
private:
	InputState currentInputState;

	std::map<std::string, sf::Keyboard::Key> keyboardInputs;
	std::map<std::string, bool> kbInputsPressed;

	std::map<std::string, int> controllerButtonInputs;
	std::map < std::string, sf::Vector2f> controllerAxisInputs;
	std::map<std::string, bool> conInputsPressed;

	void LoadInput(std::string filePath);

public:

	Input(std::string filePath);
	Input();

	bool GetInputHold(std::string input);
	bool GetInputPress(std::string input);

	void Update();
};

