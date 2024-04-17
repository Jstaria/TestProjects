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

std::vector<sf::Vector2f> BoundingBox::RayCast(sf::Vector2f p1, sf::Vector2f d1)
{
	std::vector<float> t1s;
	std::vector<sf::Vector2f> rayPoints;

	for (size_t i = 0; i < edgePoints.size(); i++)
	{
		if (Normalize(d1, 1) == Normalize(edgeDirections[i], 1)) continue;

		sf::Vector2f p2 = edgePoints[i];
		sf::Vector2f d2 = edgeDirections[i];

		float t2 = (d1.x * (p2.y - p1.y) + d1.y * (p1.x - p2.x)) / (d2.x * d1.y - d2.y * d1.x);
		float t1 = (p2.x + d2.x * t2 - p1.x) / d1.x;

		if (t1 <= 0 || !(t2 > 0 && t2 < 1)) continue;

		t1s.push_back(t1);
		rayPoints.push_back(p1 + d1 * t1);
	}


	return rayPoints;
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

void BoundingBox::CreateEdges()
{
	edgePoints.push_back(sf::Vector2f(position.top + position.height, position.left)); // left
	edgePoints.push_back(sf::Vector2f(position.top, position.left)); // top
	edgePoints.push_back(sf::Vector2f(position.top, position.left + position.width)); // right
	edgePoints.push_back(sf::Vector2f(position.top + position.height, position.left + position.width)); // bottom

	edgeDirections.push_back(sf::Vector2f(position.top, position.left));
	edgeDirections.push_back(sf::Vector2f(position.top, position.left + position.width));
	edgeDirections.push_back(sf::Vector2f(position.top + position.height, position.left + position.width));
	edgeDirections.push_back(sf::Vector2f(position.top + position.height, position.left));
}
