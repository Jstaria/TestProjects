#pragma once
#include <SFML/Graphics.hpp>
#include "ViewManager.h"
#include "BoundingBox.h"
#include <vector>
#include <map>

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

	void ShakeCamera(float maxAngle, float maxDistance, float strength, float frequency);
};

