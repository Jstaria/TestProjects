#include "Input.h"

void Input::LoadInput(std::string filePath)
{
	std::vector<std::string> data = FileIO::ReadFromFile(filePath + "KB.txt");

	for (size_t i = 0; i < data.size(); i++)
	{
		std::vector<std::string> lineData = FileIO::Split(':', data[i]);

		for (size_t j = 0; j < lineData.size(); j++)
		{
			keyboardInputs[lineData[0]] = (sf::Keyboard::Key)std::stoi(lineData[1]);
		}
	}

	data = FileIO::ReadFromFile(filePath + "GP.txt");

	for (size_t i = 0; i < 4; i++)
	{
		std::vector<std::string> lineData = FileIO::Split(':', data[i]);

		for (size_t j = 0; j < lineData.size(); j++)
		{
			std::vector<std::string> axisData = FileIO::Split(',', lineData[1]);
			sf::Vector2f axis(std::stof(axisData[0]), std::stof(axisData[1]));

			controllerAxisInputs[lineData[0]] = axis;
		}
	}

	for (size_t i = 4; i < data.size(); i++)
	{
		std::vector<std::string> lineData = FileIO::Split(':', data[i]);

		for (size_t j = 0; j < lineData.size(); j++)
		{
			controllerButtonInputs[lineData[0]] = (sf::Keyboard::Key)std::stoi(lineData[1]);
		}
	}
}

Input::Input(std::string filePath)
{
	currentInputState = InputState::Controller;
	LoadInput(filePath);
}

Input::Input()
{
}

bool Input::GetInputHold(std::string input)
{
	switch (currentInputState) {
	case InputState::Keyboard: {
		return sf::Keyboard::isKeyPressed(keyboardInputs[input]);
	}
							 break;
	case InputState::Controller: {

		if (controllerButtonInputs.find(input) != controllerButtonInputs.end()) {
			return sf::Joystick::isButtonPressed(0, controllerButtonInputs[input]);
		}
		else if (controllerAxisInputs.find(input) != controllerAxisInputs.end()){

			sf::Vector2f currentAxis(
				sf::Joystick::getAxisPosition(0, sf::Joystick::X),
				sf::Joystick::getAxisPosition(0, sf::Joystick::Y)
			);
			sf::Vector2f axis = controllerAxisInputs[input];

			if (currentAxis == sf::Vector2f(0, 0)) return false;
			if (axis.x == 0 && axis.y > 0) return currentAxis.y > axis.y;
			if (axis.y == 0 && axis.x > 0) return currentAxis.x > axis.x;
			if (axis.y == 0 && axis.x <= 0) return currentAxis.x <= axis.x;
			if (axis.x == 0 && axis.y <= 0) return currentAxis.y <= axis.y;
		}
	}
							   break;
	}
}

bool Input::GetInputPress(std::string input)
{
	bool isPressed = false;
	bool inputValue = false;

	switch (currentInputState) {
	case InputState::Keyboard: {
		isPressed = sf::Keyboard::isKeyPressed(keyboardInputs[input]);

		inputValue = isPressed && !kbInputsPressed[input];

		kbInputsPressed[input] = isPressed;
	}
							 break;
	case InputState::Controller: {
		isPressed = sf::Joystick::isButtonPressed(0, controllerButtonInputs[input]);

		inputValue = isPressed && !conInputsPressed[input];

		conInputsPressed[input] = isPressed;
	}
							   break;
	}

	return inputValue;
}

void Input::Update()
{
	if (GetInputPress("SwapControl")) {
		currentInputState = (InputState)((currentInputState + 1) % 2);

		std::cout << "did" << std::endl;
	}
}
