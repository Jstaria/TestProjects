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
	rayPoints.clear();

	std::vector<sf::Vector2f> edgePoints;
	std::vector<sf::Vector2f> edgeDirections;

	for (size_t i = 0; i < bbs.size(); i++)
	{
		std::vector<sf::Vector2f> tempEdgePoints = bbs[i].getEdgePoints();
		std::vector<sf::Vector2f> tempEdgeDirections = bbs[i].getEdgeDirections();

		for (size_t j = 0; j < tempEdgePoints.size(); j++)
		{
			if (abs(Distance(tempEdgePoints[j], position)) > distance * 10) continue;

			edgePoints.push_back(tempEdgePoints[j]);
			edgeDirections.push_back(tempEdgeDirections[j]);
		}
	}

	for (size_t j = 0; j < edgePoints.size(); j++)
	{
		currentDirection = sf::Vector2f(sf::Vector2i(edgePoints[j]) - sf::Vector2i(position));
		currentDirection = Normalize(currentDirection, distance);

		rayPoints.push_back(RayCast(sf::Vector2f(sf::Vector2i(position)), currentDirection + sf::Vector2f(cos(degreesToRadians(10)), sin(degreesToRadians(10))), edgePoints, edgeDirections));
		rayPoints.push_back(RayCast(sf::Vector2f(sf::Vector2i(position)), currentDirection, edgePoints, edgeDirections));
		rayPoints.push_back(RayCast(sf::Vector2f(sf::Vector2i(position)), currentDirection - sf::Vector2f(cos(degreesToRadians(10)), sin(degreesToRadians(10))), edgePoints, edgeDirections));
	}

	for (size_t i = 0; i < 32; i++)
	{
		currentDirection = sf::Vector2f(cos(degreesToRadians((360 / 32) * i)), sin(degreesToRadians((360 / 32) * i)));
		currentDirection = Normalize(currentDirection, distance);
		rayPoints.push_back(RayCast(sf::Vector2f(sf::Vector2i(position)), currentDirection, edgePoints, edgeDirections));
	}

	SortPointsClockwise(rayPoints, position);

	mesh = sf::VertexArray(sf::Triangles, rayPoints.size() * 3);
	/*mesh[0].position = position;
	mesh[0].color = sf::Color::Color(255, 255, 255, 20);*/
	for (size_t i = 0; i < rayPoints.size(); i++)
	{
		mesh[i * 3].position = position;
		mesh[i * 3 + 1].position = rayPoints[i];
		mesh[i * 3 + 2].position = rayPoints[(i + 1) % rayPoints.size()];
	}
}

sf::Vector2f Light::RayCast(sf::Vector2f rp, sf::Vector2f rd, std::vector<sf::Vector2f>& edgePoints, std::vector<sf::Vector2f>& edgeDirections)
{
	std::vector<sf::Vector2f> rayPoints;

	float minValue = 1000000000000;
	sf::Vector2f closestPoint = rp;

	if (edgePoints.size() == 0) {
		closestPoint = rp + rd;
		return closestPoint;
	}

	for (size_t i = 0; i < edgePoints.size(); i++)
	{
		if (Normalize(rd, 1) == Normalize(edgeDirections[i], 1)) continue;

		float r_px = rp.x;
		float r_py = rp.y;
		float r_dx = rd.x;
		float r_dy = rd.y;

		float s_px = edgePoints[i].x;
		float s_py = edgePoints[i].y;
		float s_dx = edgeDirections[i].x;
		float s_dy = edgeDirections[i].y;

		float t2 = (r_dx * (s_py - r_py) + r_dy * (r_px - s_px)) / (s_dx * r_dy - s_dy * r_dx);
		float t1 = (s_px + s_dx * t2 - r_px) / r_dx;

		if ((t1 > 0 && t1 < 1) && (t2 > 0 && t2 < 1) && t1 < minValue) {
			closestPoint = rp + (rd * t1);
			minValue = t1;
		}

		if (i >= edgePoints.size() - 1 && closestPoint == rp) closestPoint = rp + rd;

		//if (i == edgePoints.size() - 1) closestPoint = rp + rd * t1;
	}

	//rayPoints.push_back(closestPoint);

	//closestPoint = sf::Vector2f(closestPoint.y, closestPoint.x);

	return closestPoint;
}

void Light::Draw(sf::RenderWindow& window)
{
	//sf::CircleShape shape(10);
	//shape.setOrigin(10, 10);
	//shape.setPosition(position);
	//shape.setFillColor(sf::Color::Yellow);

	//window.draw(shape);

	//for (size_t i = 0; i < rayPoints.size(); i++)
	//{
	//	sf::VertexArray line(sf::Lines, 2);
	//	line[0].position = position;
	//	line[0].color = sf::Color::Yellow;
	//	line[1].position = rayPoints[i];

	//	sf::CircleShape shape(5);
	//	shape.setOrigin(5, 5);
	//	shape.setPosition(rayPoints[i]);
	//	shape.setFillColor(sf::Color::Yellow);

	//	window.draw(shape);

	//	window.draw(line);
	//}

	window.draw(mesh, GlobalVariables::getShader("light"));
}

