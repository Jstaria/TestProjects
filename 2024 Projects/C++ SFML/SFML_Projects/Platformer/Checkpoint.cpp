#include "Checkpoint.h"

Checkpoint::Checkpoint(std::map<std::string, sf::Sprite>* sprites, sf::Vector2f position, int maxFrames) :
	Entity(sprites, position, maxFrames)
{
	spriteMap = *sprites;
	float scaler = GlobalVariables::getTextureScaler();

	for (auto& pair : this->spriteMap)
	{
		sf::Vector2f bounds = pair.second.getLocalBounds().getSize();
		pair.second.setOrigin(bounds.x / 2, bounds.y);
		pair.second.setPosition(position);

		pair.second.setScale(scaler, scaler);

		spriteMap[pair.first] = pair.second;
	}

	sf::Vector2f size = spriteMap["unlit"].getLocalBounds().getSize() * scaler;
	sf::Vector2f newPosition = position - sf::Vector2f(
		size.x / 2,
		size.y);

	boundingBox = BoundingBox(
		newPosition,
		newPosition + size,
		sf::Color::Green
	);

	currentSprite = spriteMap["unlit"];
}

void Checkpoint::Draw(sf::RenderWindow& window)
{
	window.draw(currentSprite);

	boundingBox.Draw(window);
}

void Checkpoint::Update()
{
	GetInteraction();

	currentSprite = isActive ? spriteMap["lit"] : spriteMap["unlit"];
}

void Checkpoint::GetInteraction()
{
	if (CheckCollision() && GlobalVariables::getInput()->GetInputPress("Interact")) {
		isActive = !isActive;
	}
}

bool Checkpoint::CheckCollision()
{
	BoundingBox bb = GlobalVariables::Instance()->getPlayerBB();

	return bb.CheckCollision(boundingBox);
}
