#include "Checkpoint.h"

Checkpoint::Checkpoint(std::map<std::string, sf::Sprite>* sprites, sf::Vector2f position, int maxFrames) :
	Entity(sprites, position, maxFrames), spriteMap(*sprites)
{
	for (auto& pair : this->spriteMap)
	{
		sf::Vector2f bounds = pair.second.getLocalBounds().getSize();
		pair.second.setOrigin(bounds.x / 2, bounds.y);
		pair.second.setPosition(position);
	}

	sf::FloatRect rect = spriteMap["Unlit"].getLocalBounds();

	boundingBox = BoundingBox(
		sf::Vector2f(rect.left, rect.top),
		sf::Vector2f(rect.left + rect.width, rect.top + rect.height),
		sf::Color::Green
	);
}

void Checkpoint::Draw(sf::RenderWindow& window)
{
	window.draw(currentSprite);

	boundingBox.Draw(window);
}

void Checkpoint::Update()
{
	GetInteraction();
}

void Checkpoint::GetInteraction() const
{
	std::cout << CheckCollision() << std::endl;
}

bool Checkpoint::CheckCollision() const
{
	BoundingBox bb = GlobalVariables::Instance()->getPlayerBB();

	return bb.CheckCollision(boundingBox);
}
