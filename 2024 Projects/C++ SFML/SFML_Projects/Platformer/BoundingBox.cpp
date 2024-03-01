#include "BoundingBox.h"

BoundingBox::BoundingBox(sf::Vector2f pos1, sf::Vector2f pos2) :
	position(pos1, pos2 - pos1)
{
	
}

sf::FloatRect BoundingBox::GetRect()
{
	return position;
}

sf::FloatRect BoundingBox::SetRect(sf::FloatRect newPosition)
{
	position = newPosition;
	return position;
}


