#include "Checkpoint.h"

Checkpoint::Checkpoint(std::map<std::string, sf::Sprite>* sprites, sf::Vector2f position, int maxFrames) :
	Entity(sprites, position, maxFrames)
{
}

void Checkpoint::Draw(sf::RenderTarget& window)
{
	window.draw(currentSprite);
}

void Checkpoint::Update()
{
	// Get interaction
}

void Checkpoint::GetInteraction() const
{
	// Check collision, if true, bring up interaction text
}

void Checkpoint::CheckCollision() const
{
	// GlobalVariables player bounding box, check to see if it intersects with the save box
}
