#pragma once

#include <SFML/Graphics.hpp>

class BoundingBox
{
private:
	sf::FloatRect position;

public:
	BoundingBox(sf::Vector2f pos1, sf::Vector2f pos2);

	sf::FloatRect GetRect();

	sf::FloatRect SetRect(sf::FloatRect newPosition);
};

