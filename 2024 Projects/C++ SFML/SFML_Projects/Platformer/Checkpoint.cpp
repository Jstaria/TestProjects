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

	std::string key = "unlit";

	currentState = AnimationState::Inactive;

	this->maxFrames = maxFrames;
	this->frameWidth = spriteMap[key].getLocalBounds().width / maxFrames;
	this->frameHeight = spriteMap[key].getLocalBounds().height;
	this->frameNum = 0;

	isActive = false;

	sf::Vector2f size = GetDrawnSprite().getLocalBounds().getSize() * scaler;
	sf::Vector2f newPosition = position - sf::Vector2f(
		size.x / 2,
		size.y);

	boundingBox = BoundingBox(
		newPosition,
		newPosition + size,
		sf::Color::Green
	);

	coolDown = sf::seconds(.1f);
}

void Checkpoint::Draw(sf::RenderWindow& window)
{
	if (inCollision) {
		sf::Sprite sprite = drawnSprite;
		sf::Image image = sprite.getTexture()->copyToImage();
		image.createMaskFromColor(sf::Color::White);
		sf::Texture texture;
		texture.loadFromImage(image);
		sprite.setTexture(texture);

		float size = sprite.getLocalBounds().getSize().y * sprite.getScale().y;

		sprite.setScale(sprite.getScale() + sf::Vector2f(.1f, .1f));

  		size = ((sprite.getLocalBounds().getSize().y * sprite.getScale().y) - size) / 3;

		sprite.setPosition(sprite.getPosition() + sf::Vector2f(0, size));
		window.draw(sprite);
	}

	window.draw(drawnSprite);

	boundingBox.Draw(window);
}

void Checkpoint::Update()
{
	GetInteraction();
	GetDrawnSprite();

}

void Checkpoint::GetInteraction()
{
	if (!(inCollision = CheckCollision())) return;

	if (!GlobalVariables::getInput()->GetInputPress("Interact")) return;
	
	isActive = !isActive;
	frameNum = 0;
	timeOfSwitch = clock.getElapsedTime();
	currentState = isActive ? AnimationState::Activating : AnimationState::Deactivating;
}

bool Checkpoint::CheckCollision()
{
	BoundingBox bb = GlobalVariables::Instance()->getPlayerBB();

	return bb.CheckCollision(boundingBox);
}

sf::Sprite Checkpoint::GetDrawnSprite()
{
	switch (currentState) {
	case AnimationState::Activating: {

		drawnSprite = CreateSprite(spriteMap["light"].getTexture());
		IncrementFrameNum(6);

		if (frameNum == 0 && clock.getElapsedTime() - timeOfSwitch > coolDown) {
			currentState = AnimationState::Active;
		}
	}
								   break;

	case AnimationState::Active: {
		drawnSprite = CreateSprite(spriteMap["lit"].getTexture());
		IncrementFrameNum(6);
	}
							   break;

	case AnimationState::Deactivating: {
		drawnSprite = CreateSprite(spriteMap["extinguish"].getTexture());
		IncrementFrameNum(6);

		if (frameNum == 0 && clock.getElapsedTime() - timeOfSwitch > coolDown) {
			currentState = AnimationState::Inactive;
		}
	}
									 break;

	case AnimationState::Inactive: {
		drawnSprite = CreateSprite(spriteMap["unlit"].getTexture());
		IncrementFrameNum(6);
	}
								 break;
	}

	count++;

	return drawnSprite;
}

void Checkpoint::IncrementFrameNum(int modulus)
{
	if (count % (modulus) == 0) {
		frameNum = (frameNum + 1) % maxFrames;
		count = 0;
	}
}

sf::Sprite Checkpoint::CreateSprite(const sf::Texture* texture)
{

	sf::Sprite newSprite(*texture, sf::IntRect(frameWidth * frameNum, 0, frameWidth, frameHeight));
	newSprite.setScale(GlobalVariables::getTextureScaler(), GlobalVariables::getTextureScaler());
	newSprite.setOrigin(newSprite.getLocalBounds().width / 2, newSprite.getLocalBounds().height);
	newSprite.setPosition(position);

	return newSprite;
}