#include "Entity.h"

Entity::Entity(sf::Sprite sprite, sf::Vector2f position)
    : sprite(sprite), position(position) {}

void Entity::Draw(sf::RenderWindow& window)
{
    // Draw the sprite on the window
    window.draw(sprite);
}

void Entity::Update()
{
    // Update logic for the entity (if needed)
}