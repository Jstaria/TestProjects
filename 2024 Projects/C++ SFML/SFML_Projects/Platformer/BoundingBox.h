#pragma once

#include <SFML/Graphics.hpp>

class BoundingBox
{
private:
	sf::FloatRect position;
	sf::RectangleShape boundingBox;
	sf::Vector2f offset;

	std::map<sf::Vector2f, sf::Vector2f> edges;

	void CreateEdges();

public:
	BoundingBox(sf::Vector2f pos1, sf::Vector2f pos2, sf::Color color);
	BoundingBox(sf::Vector2f pos1, sf::Vector2f pos2, sf::Color color, sf::Vector2f offset);
	BoundingBox();

	sf::FloatRect getRect();
	sf::Vector2f getOffset();

	sf::FloatRect setRect(sf::FloatRect newPosition);

	void Draw(sf::RenderWindow& window);
	void Move(sf::Vector2f direction);
	void MoveTo(sf::Vector2f pos);

	bool CheckCollision(BoundingBox bb);
	bool CheckCollision(sf::FloatRect rect);
	bool CheckCollision(sf::Vector2f position);
	bool isEqual(BoundingBox& bb);
};

