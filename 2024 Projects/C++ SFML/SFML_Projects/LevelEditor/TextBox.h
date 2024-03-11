#pragma once
#include "BoundingBox.h"
#include <iostream>

class TextBox
{
private:
	BoundingBox box;
	sf::Vector2f relativePosition;
	std::string currentText;
	sf::Text text;

public:
	TextBox(sf::Font font, sf::Vector2f position);
	TextBox();
	void Update();
	void Draw(sf::RenderWindow& window);
	void SetPositionRelativeToView(sf::RenderWindow& window, sf::Vector2f relativePos);
};


