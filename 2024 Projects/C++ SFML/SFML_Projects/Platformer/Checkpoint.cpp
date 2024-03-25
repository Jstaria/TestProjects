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
}

void Checkpoint::GetInteraction() const
{
}

void Checkpoint::CheckCollision() const
{
}
