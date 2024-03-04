#pragma once

#include <SFML/Graphics.hpp>

class BoundingBox
{
private:
	sf::FloatRect position;
	sf::RectangleShape boundingBox;
	sf::Vector2f offset;

public:
	BoundingBox(sf::Vector2f pos1, sf::Vector2f pos2, sf::Color color);
	BoundingBox(sf::Vector2f pos1, sf::Vector2f pos2, sf::Color color, sf::Vector2f offset);
	BoundingBox();

	sf::FloatRect GetRect();

	sf::FloatRect SetRect(sf::FloatRect newPosition);

	void Draw(sf::RenderWindow& window);
	void Move(sf::Vector2f direction);
	void MoveTo(sf::Vector2f pos);

	bool CheckCollision(BoundingBox bb);
	bool CheckCollision(sf::FloatRect rect);
	bool isEqual(BoundingBox& bb);
};

