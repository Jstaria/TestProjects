#pragma once
#include <SFML/Graphics.hpp>
#include "BoundingBox.h"
#include "GlobalVariables.h"
#include "HelperFunctions.h"

class Light
{
private:
	sf::Vector2f position;
	std::vector<sf::Vector2f> rayPoints;
	sf::Vector2f currentDirection;

	sf::VertexArray mesh;

	float spreadAngle, directionAngle, distance, intensity;

	sf::Vector2f RayCast(sf::Vector2f rp, sf::Vector2f rd, std::vector<sf::Vector2f>& edgePoints, std::vector<sf::Vector2f>& edgeDirections);

public:
	Light();
	Light(sf::Vector2f position, float spreadAngle, float directionAngle, float distance, float intensity);

	void CalculateMesh(std::vector<BoundingBox> bbs);

	void Draw(sf::RenderWindow& window);
};

