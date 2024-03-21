#include "BoundingBox.h"

#include <iostream>

BoundingBox::BoundingBox(sf::Vector2f pos1, sf::Vector2f pos2, sf::Color color, sf::Vector2i gridPos1, sf::Vector2i gridPos2) :
	position(pos1, pos2 - pos1), pos1(gridPos1), pos2(gridPos2)
{
	color.a = 150;

	boundingBox = sf::RectangleShape(sf::Vector2f(position.width, position.height));
	boundingBox.setFillColor(sf::Color::Transparent);
	boundingBox.setOutlineColor(color);
	boundingBox.setOutlineThickness(-5.f);

	sf::Vector2f origin(position.width / 2, position.height / 2);

	//boundingBox.setOrigin(origin);
	boundingBox.setPosition(pos1 );
}

BoundingBox::BoundingBox(sf::Vector2f pos1, sf::Vector2f pos2, sf::Color color, sf::Vector2f offset) :
	position(pos1, pos2 - pos1), offset(offset)
{
	color.a = 150;

	boundingBox = sf::RectangleShape(sf::Vector2f(position.width, position.height));
	boundingBox.setFillColor(sf::Color::Transparent);
	boundingBox.setOutlineColor(color);
	boundingBox.setOutlineThickness(-2.f);

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

bool BoundingBox::CheckCollision(BoundingBox bb)
{
	sf::FloatRect rect = bb.getRect();

	/*bool isColliding = rect.top > position.top + position.height && rect.top + rect.height < position.top &&
		rect.left < position.left + position.width && rect.left + rect.width > position.left;*/

	//isColliding = bb.GetRect().contains(boundingBox.getPosition());

	return position.intersects(rect);
}

sf::Vector2i BoundingBox::getPos1()
{
	return pos1;
}

sf::Vector2i BoundingBox::getPos2()
{
	return pos2;
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


