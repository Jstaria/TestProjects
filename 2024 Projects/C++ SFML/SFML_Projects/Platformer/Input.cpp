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
}

Input::Input(std::string filePath)
{
	currentInputState = InputState::Keyboard;
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
		else {

			sf::Vector2f currentAxis(
				sf::Joystick::getAxisPosition(0, sf::Joystick::X),
				sf::Joystick::getAxisPosition(0, sf::Joystick::Y)
			);
			sf::Vector2f axis = controllerAxisInputs[input];

			if (axis.x == 0) return currentAxis.y > axis.y;
			if (axis.y == 0) return currentAxis.x > axis.x;
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

	}
							   break;
	}

	return inputValue;
}
