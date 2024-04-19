#include "Light.h"

Light::Light()
{
}

Light::Light(sf::Vector2f position, float spreadAngle, float directionAngle, float distance, float intensity) :
	spreadAngle(spreadAngle), directionAngle(directionAngle), position(position), distance(distance), intensity(intensity)
{
}

void Light::CalculateMesh(std::vector<BoundingBox> bbs)
{
	std::vector<sf::Vector2f> edgePoints;
	std::vector<sf::Vector2f> edgeDirections;

 	for (size_t i = 0; i < 1; i++)
	{
		if (abs(Distance(bbs[i].getRect().getPosition(), position)) > distance) continue;

		std::vector<sf::Vector2f> tempEdgePoints = bbs[i].getEdgePoints();
		std::vector<sf::Vector2f> tempEdgeDirections = bbs[i].getEdgeDirections();

		for (size_t j = 0; j < tempEdgePoints.size(); j++)
		{
			edgePoints.push_back(tempEdgePoints[j]);
			edgeDirections.push_back(tempEdgeDirections[j]);
		}
	}

	for (size_t i = 0; i < edgePoints.size(); i++)
	{
		sf::Vector2f direction = edgePoints[i] - position;
		//direction = Normalize(direction, distance);

		rayPoints.push_back(RayCast(position, direction, edgePoints, edgeDirections));

	}
}

sf::Vector2f Light::RayCast(sf::Vector2f rp, sf::Vector2f rd, std::vector<sf::Vector2f>& edgePoints, std::vector<sf::Vector2f>& edgeDirections)
{
	std::vector<sf::Vector2f> rayPoints;

	float minValue = 1000000000000;
	sf::Vector2f closestPoint = sf::Vector2f(0, 0);

	for (size_t i = 0; i < edgePoints.size(); i++)
	{
		if (Normalize(rd, 1) == Normalize(edgeDirections[i], 1)) continue;

		float s_px = rp.x;
		float s_py = rp.y;
		float s_dx = rd.x;
		float s_dy = rd.y;

		float r_px = edgePoints[i].x;
		float r_py = edgePoints[i].y;
		float r_dx = edgeDirections[i].x;
		float r_dy = edgeDirections[i].y;

		float t2 = (r_dx * (s_py - r_py) + r_dy * (r_px - s_px)) / (s_dx * r_dy - s_dy * r_dx);
		float t1 = (s_px + s_dx * t2 - r_px) / r_dx;
		
		closestPoint = rp + rd;
	}

	//rayPoints.push_back(closestPoint);

	closestPoint = sf::Vector2f(closestPoint.y, closestPoint.x);

	return closestPoint;
}

void Light::Draw(sf::RenderWindow& window)
{
	sf::CircleShape shape(10);
	shape.setOrigin(10, 10);
	shape.setPosition(position);
	shape.setFillColor(sf::Color::Yellow);

	window.draw(shape);

	for (size_t i = 0; i < rayPoints.size(); i++)
	{
		sf::VertexArray line(sf::Lines, 2);
		line[0].position = position;
		line[0].color = sf::Color::Yellow;
		line[1].position = rayPoints[i];

		window.draw(line);
	}
}
