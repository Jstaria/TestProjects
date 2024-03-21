#pragma once
#include <SFML/Graphics.hpp>
#include "GlobalVariables.h"
#include "ViewManager.h"
#include "BoundingBox.h"
#include <vector>
#include <map>
#include "HelperFunctions.h"

class Camera
{
private:
	sf::Vector2f currentPosition;
	std::vector<BoundingBox> boundingEdges;
	BoundingBox current;

	void CheckCollisions();
	void StayInBounds();

public:
	Camera();

	void AddBoundingEdge(BoundingBox boundingEdge);
	void RemoveLastEdge();

	void Update();
	void Draw(sf::RenderWindow& window);
	std::vector< BoundingBox> GetBoundingEdges();
};

