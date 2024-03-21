#pragma once

#include <SFML/Graphics.hpp>

class BoundingBox
{
private:
	sf::FloatRect position;
	sf::Vector2i pos1;
	sf::Vector2i pos2;
	sf::RectangleShape boundingBox;
	sf::Vector2f offset;

public:
	BoundingBox(sf::Vector2f pos1, sf::Vector2f pos2, sf::Color color, sf::Vector2i gridPos1, sf::Vector2i gridPos2);
	BoundingBox(sf::Vector2f pos1, sf::Vector2f pos2, sf::Color color, sf::Vector2f offset);
	BoundingBox();

	sf::FloatRect getRect();
	sf::Vector2f getOffset();

	sf::FloatRect setRect(sf::FloatRect newPosition);

	void Draw(sf::RenderWindow& window);
	void Move(sf::Vector2f direction);
	void MoveTo(sf::Vector2f pos);

	sf::Vector2i getPos1();
	sf::Vector2i getPos2();

	bool CheckCollision(BoundingBox bb);
	bool CheckCollision(sf::FloatRect rect);
	bool CheckCollision(sf::Vector2f position);
	bool isEqual(BoundingBox& bb);
};

