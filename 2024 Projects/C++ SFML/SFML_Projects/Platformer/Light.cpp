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

		sf::View view = ViewManager::Instance()->GetWindowView();

		sf::FloatRect viewRect(view.getCenter() - view.getSize() * .5f, view.getSize());

		if (!bbs[i].getRect().intersects(viewRect)) continue;

		for (size_t j = 0; j < tempEdgePoints.size(); j++)
		{
			edgePoints.push_back(tempEdgePoints[j]);
			edgeDirections.push_back(tempEdgeDirections[j]);
		}
	}

	for (size_t j = 0; j < edgePoints.size(); j++)
	{
		currentDirection = edgePoints[j] - position;
		currentDirection = limitDistance(currentDirection, distance);

		rayPoints.push_back(RayCast(position, currentDirection, edgePoints, edgeDirections));

		float degrees = 20;

		currentDirection = edgePoints[j] - position;
		currentDirection = Normalize(currentDirection, distance);

		rayPoints.push_back(RayCast(position, currentDirection + sf::Vector2f(cos(degreesToRadians(degrees)), sin(degreesToRadians(degrees))), edgePoints, edgeDirections));
		rayPoints.push_back(RayCast(position, currentDirection - sf::Vector2f(cos(degreesToRadians(degrees)), sin(degreesToRadians(degrees))), edgePoints, edgeDirections));
	}


	for (size_t i = 0; i < 33; i++)
	{
		currentDirection = sf::Vector2f(cos(degreesToRadians((360 / 32) * i)), sin(degreesToRadians((360 / 32) * i)));
		currentDirection = Normalize(currentDirection, distance);
		rayPoints.push_back(RayCast(sf::Vector2f(sf::Vector2i(position)), currentDirection, edgePoints, edgeDirections));
	}

	SortPointsClockwise(rayPoints, position);
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

	float minValue = 100000;
	sf::Vector2f closestPoint = rp;

	if (edgePoints.size() == 0) {
		closestPoint = rp + rd;
		return closestPoint;
	}

	for (size_t i = 0; i < edgePoints.size(); i++)
	{
		//if (Normalize(rd, 1) == Normalize(edgeDirections[i], 1)) continue;

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

		if ((t1 > 0 && t1 <= 1) && (t2 > 0 && t2 < 1) && t1 < minValue) {
			closestPoint = sf::Vector2f(r_px + r_dx * t1, r_py + r_dy * t1);
			minValue = t1;
		}

		if (i >= edgePoints.size() - 1 && closestPoint == rp) closestPoint = rp + rd;
	}

	//std::cout << minValue << std::endl;
	return closestPoint;
}

void Light::Draw(sf::RenderWindow& window)
{
	//sf::CircleShape shape(10);
	//shape.setOrigin(10, 10);
	//shape.setPosition(position);
	//shape.setFillColor(sf::Color::Yellow);

	//window.draw(shape);

	GlobalVariables::getShader("light")->setUniform("position", sf::Vector2f(mesh.getBounds().getPosition()));
	GlobalVariables::getShader("light")->setUniform("size", sf::Vector2f(mesh.getBounds().getSize()));

	window.draw(mesh, GlobalVariables::getShader("light"));

	for (size_t i = 0; i < rayPoints.size(); i++)
	{
		sf::VertexArray line(sf::Lines, 2);
		line[0].position = position;
		line[0].color = sf::Color::Red;
		line[1].position = rayPoints[i];

		sf::CircleShape shape(5);
		shape.setOrigin(5, 5);
		shape.setPosition(rayPoints[i]);
		shape.setFillColor(sf::Color::Blue);

		window.draw(shape);

		window.draw(line);
	}


}

