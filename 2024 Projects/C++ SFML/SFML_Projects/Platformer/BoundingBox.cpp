#include "BoundingBox.h"


#include <iostream>

BoundingBox::BoundingBox(sf::Vector2f pos1, sf::Vector2f pos2, sf::Color color) :
	position(pos1, pos2 - pos1)
{
	color.a = 150;

	boundingBox = sf::RectangleShape(sf::Vector2f(position.width, position.height));
	boundingBox.setFillColor(sf::Color::Transparent);
	boundingBox.setOutlineColor(color);
	boundingBox.setOutlineThickness(-1.25f);

	sf::Vector2f origin(position.width / 2, position.height / 2);

	//boundingBox.setOrigin(origin);
	boundingBox.setPosition(pos1);

	CreateEdges();
}

BoundingBox::BoundingBox(sf::Vector2f pos1, sf::Vector2f pos2, sf::Color color, sf::Vector2f offset) :
	position(pos1, pos2 - pos1), offset(offset)
{
	color.a = 150;

	boundingBox = sf::RectangleShape(sf::Vector2f(position.width, position.height));
	boundingBox.setFillColor(sf::Color::Transparent);
	boundingBox.setOutlineColor(color);
	boundingBox.setOutlineThickness(-1.25f);

	sf::Vector2f origin(position.width / 2, position.height / 2);

	//boundingBox.setOrigin(origin);
	boundingBox.setPosition(pos1);
}

BoundingBox::BoundingBox()
{
}

sf::FloatRect BoundingBox::getRect()
{
	return position;
}

sf::Vector2f BoundingBox::getOffset()
{
	return offset;
}

sf::FloatRect BoundingBox::setRect(sf::FloatRect newPosition)
{
	position = newPosition;

	boundingBox.setPosition(position.getPosition());
	return position;
}

void BoundingBox::Draw(sf::RenderWindow& window)
{
	window.draw(boundingBox);
	//int radius = 10;
	//sf::CircleShape origin(radius);
	//origin.setOrigin(radius, radius);
	//origin.setPosition(position.getPosition());
	//origin.setFillColor(sf::Color::Red);
	//window.draw(origin);
}

void BoundingBox::Move(sf::Vector2f direction)
{
	sf::FloatRect rect(position.getPosition() + direction, position.getSize());

	boundingBox.setPosition(rect.getPosition());

	position = rect;
}

void BoundingBox::MoveTo(sf::Vector2f pos)
{
	sf::FloatRect rect(pos + offset, position.getSize());

	boundingBox.setPosition(rect.getPosition());

	position = rect;
}

std::vector<sf::Vector2f> BoundingBox::RayCast(sf::Vector2f rp, sf::Vector2f rd)
{
	std::vector<float> t1s;
	std::vector<sf::Vector2f> rayPoints;

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

		if (!(t1 > 0 && t1 < 1) || !(t2 > 0 && t2 < 1)) continue;

		t1s.push_back(t1);
		rayPoints.push_back(rp + (rd * t1));
	}


	return rayPoints;
}

std::vector<sf::Vector2f> BoundingBox::RayCastCorners(sf::Vector2f rp)
{
	//std::vector<float> t1s;
	std::vector<sf::Vector2f> rayPoints;

	//for (size_t i = 0; i < edgePoints.size(); i++)
	//{
	//	float minValue = 1000000000000;
	//	sf::Vector2f closestPoint = sf::Vector2f(0,0);

	//	for (size_t j = -1; j < 2; j++)
	//	{
	//		sf::Vector2f rd = rp - edgePoints[i] + sf::Vector2f(cos(j), sin(j));

	//		if (Normalize(rd, 1) == Normalize(edgeDirections[i], 1)) continue;

	//		float r_px = rp.x;
	//		float r_py = rp.y;
	//		float r_dx = rd.x;
	//		float r_dy = rd.y;

	//		float s_px = edgePoints[i].x;
	//		float s_py = edgePoints[i].y;
	//		float s_dx = edgeDirections[i].x;
	//		float s_dy = edgeDirections[i].y;

	//		float t2 = (r_dx * (s_py - r_py) + r_dy * (r_px - s_px)) / (s_dx * r_dy - s_dy * r_dx);
	//		float t1 = (s_px + s_dx * t2 - r_px) / r_dx;

	//		if (!(t1 > 0 && t1 < 1) || !(t2 > 0 && t2 < 1)) continue;
	//		if (t1 > minValue) continue;
	//		
	//		minValue = t1;
	//		closestPoint = rp + (rd * t1);
	//	}
	//}


	return rayPoints;

	// doing this wrong, I need all of the edges for the light class, and once I have them all I can then ray cast because then it wont be segmented anymore
}

bool BoundingBox::CheckCollision(BoundingBox bb)
{
	sf::FloatRect rect = bb.getRect();

	/*bool isColliding = rect.top > position.top + position.height && rect.top + rect.height < position.top &&
		rect.left < position.left + position.width && rect.left + rect.width > position.left;*/

		//isColliding = bb.GetRect().contains(boundingBox.getPosition());

	return position.intersects(rect);
}

bool BoundingBox::CheckCollision(sf::FloatRect rect)
{
	return position.intersects(rect);
}

bool BoundingBox::CheckCollision(sf::Vector2f position)
{
	return this->position.contains(position);
}

bool BoundingBox::isEqual(BoundingBox& bb)
{
	return
		position == bb.getRect();

}

std::vector<sf::Vector2f> BoundingBox::getEdgePoints()
{
	return edgePoints;
}

std::vector<sf::Vector2f> BoundingBox::getEdgeDirections()
{
	return edgeDirections;
}

void BoundingBox::CreateEdges()
{
	edgePoints.push_back(sf::Vector2f(position.left, position.top)); // top left
	edgePoints.push_back(sf::Vector2f(position.left + position.width, position.top)); // top right
	edgePoints.push_back(sf::Vector2f(position.left + position.width, position.top + position.height)); // bottom right
	edgePoints.push_back(sf::Vector2f(position.left, position.top + position.height)); // bottom left

	edgeDirections.push_back(sf::Vector2f(position.width, 0)); // top
	edgeDirections.push_back(sf::Vector2f(0, position.height)); // right
	edgeDirections.push_back(sf::Vector2f(-position.width, 0)); // bottom
	edgeDirections.push_back(sf::Vector2f(0, -position.height)); // left
}
