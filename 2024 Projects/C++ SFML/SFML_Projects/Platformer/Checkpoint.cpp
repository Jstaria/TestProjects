#include "Checkpoint.h"

bool Checkpoint::CheckCollision()
{
	return false;
}

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
