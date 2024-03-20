#pragma once
#include <SFML/Graphics.hpp>
#include "GlobalVariables.h"
#include "ViewManager.h"
#include "BoundingBox.h"
#include <vector>
#include <map>

class Camera
{
private:
	sf::Vector2f currentPosition;
	std::vector<BoundingBox> boundingEdges;

	void CheckCollisions();

public:
	Camera();

	void AddBoundingEdge(BoundingBox boundingEdge);
	void RemoveLastEdge();

	void Update();

	std::vector< BoundingBox> GetBoundingEdges();
};

