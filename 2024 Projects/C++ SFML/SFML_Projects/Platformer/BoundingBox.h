#pragma once

#include <SFML/Graphics.hpp>
#include <vector>
#include <map>
#include "HelperFunctions.h"

class BoundingBox
{
private:
	sf::FloatRect position;
	sf::RectangleShape boundingBox;
	sf::Vector2f offset;

	std::vector<sf::Vector2f> edgePoints;
	std::vector<sf::Vector2f> edgeDirections;

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

	std::vector<sf::Vector2f> RayCast(sf::Vector2f point, sf::Vector2f direction);
	std::vector<sf::Vector2f> RayCastCorners(sf::Vector2f point);
	bool CheckCollision(BoundingBox bb);
	bool CheckCollision(sf::FloatRect rect);
	bool CheckCollision(sf::Vector2f position);
	bool isEqual(BoundingBox& bb);

	std::vector<sf::Vector2f> getEdgePoints();
	std::vector<sf::Vector2f> getEdgeDirections();
};

