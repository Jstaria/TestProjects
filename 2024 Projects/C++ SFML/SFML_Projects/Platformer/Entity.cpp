#include "Entity.h"

Entity::Entity(std::map<std::string, sf::Sprite>* sprites, sf::Vector2f position, int maxFrames)
    : sprites(sprites), position(position), maxFrames(maxFrames) {}

void Entity::Draw(sf::RenderWindow& window)
{
    // Draw the sprite on the window
}

void Entity::Update()
{
    // Update logic for the entity (if needed)
}