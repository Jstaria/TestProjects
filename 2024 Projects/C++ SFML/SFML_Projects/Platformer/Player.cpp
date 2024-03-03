#include "Player.h"
#include <iostream>
#include "GlobalVariables.h"

Player::Player(std::map<std::string, sf::Sprite>* sprites, sf::Vector2f position, int maxFrames) 
	: Entity(sprites, position, maxFrames) {
	
	for (const auto& pair : *Player::sprites) {
		sf::Sprite& sprite = (*Player::sprites)[pair.first];

		sprite.setScale(GlobalVariables::getTextureScaler(), GlobalVariables::getTextureScaler());
		sprite.setPosition(position);
	}

	std::string key = "walk";
	this->frameWidth = (*this->sprites)[key].getLocalBounds().width / maxFrames;
	this->frameHeight = (*this->sprites)[key].getLocalBounds().height;

	this->speedMultiplier = 10;
	this->direction = sf::Vector2f(0, 0);

	lastFacedDirectionX = 1;
	currentSprite = (*this->sprites)[key];
	drawnSprite = GetCurrentSprite(currentSprite, lastFacedDirectionX);

	CreateBB();
}

void Player::Update() {
	sf::Vector2f direction(0, 2);
	float lerpSpeed = .25f;
	float movementLerp = .1f;

	currentSprite = (*sprites)[std::string("idle")];

	for (auto& bb : currentLevel->getBBArray()) {
		if (boundingBoxes["GroundBox"].CheckCollision(bb)) {
			direction.y = 0;
			lerpSpeed = 1;
			//std::cout << "In collision" << std::endl;
		}
	}

	if (sf::Keyboard::isKeyPressed(sf::Keyboard::Space)) {
		direction.y -= 2;
	}

	if (sf::Keyboard::isKeyPressed(sf::Keyboard::A)) {
		direction.x -= 1;
		lerpSpeed = movementLerp;
		currentSprite = (*sprites)[std::string("walk")];
	}

	if (sf::Keyboard::isKeyPressed(sf::Keyboard::D)) {
		direction.x += 1;
		lerpSpeed = movementLerp;
		currentSprite = (*sprites)[std::string("walk")];
	}

	direction = Normalize(direction, speedMultiplier);

	Player::direction = lerp(Player::direction, direction, lerpSpeed);

	Move(Player::direction);

	drawnSprite = Player::GetCurrentSprite(currentSprite, lastFacedDirectionX);

	if (direction.x != 0) {
		lastFacedDirectionX = direction.x;
	}
}

void Player::setCurrentLevel(Level* level)
{
	currentLevel = level;
	MoveTo(level->getPlayerPos());
}

void Player::Draw(sf::RenderWindow& window) {
	window.draw(drawnSprite);

	for (auto& pair : boundingBoxes) {
		pair.second.Draw(window);
	}
}

sf::Sprite Player::GetCurrentSprite(sf::Sprite& currentSprite, int xDirection) {

	const sf::Texture* texture = currentSprite.getTexture();

	sf::Sprite newSprite(*texture, sf::IntRect(frameWidth * frameNum, 0, frameWidth, frameHeight));
	newSprite.setScale(GlobalVariables::getTextureScaler() * sign(xDirection), GlobalVariables::getTextureScaler());
	newSprite.setOrigin(newSprite.getLocalBounds().width / 2, newSprite.getLocalBounds().height / 2);
	newSprite.setPosition(position);

	if (count % (6) == 0) {
		frameNum = (frameNum + 1) % maxFrames;
		count = 0;
	}
	
	count++;
	return newSprite;
}

void Player::Move(sf::Vector2f speed) {
	position += speed;
	//std::cout << position.x << "," << position.y << std::endl;
	currentSprite.setPosition(position);

	for (auto& pair : boundingBoxes) {
		BoundingBox bb = pair.second;

		bb.Move(speed);

		boundingBoxes[pair.first] = bb;
	}
}

void Player::MoveTo(sf::Vector2f pos)
{
	position = pos;
	//std::cout << position.x << "," << position.y << std::endl;
	currentSprite.setPosition(position);

	for (auto& pair : boundingBoxes) {
		BoundingBox bb = pair.second;

		//sf::Vector2f offset(bb.GetRect().width / 2, bb.GetRect().height / 2);

		bb.MoveTo(position);

		boundingBoxes[pair.first] = bb;
	}
}

void Player::CreateBB()
{
	int scaler = GlobalVariables::getTextureScaler();
	BoundingBox box(
		position - sf::Vector2f(drawnSprite.getLocalBounds().width / 4 * scaler, drawnSprite.getLocalBounds().height / 2 * scaler),
		position + sf::Vector2f(drawnSprite.getLocalBounds().width / 4 * scaler, drawnSprite.getLocalBounds().height / 2 * scaler),
		sf::Color::Yellow);

	boundingBoxes.emplace("Hitbox", box);

	BoundingBox ground(
		position - sf::Vector2f(drawnSprite.getLocalBounds().width / 8 * scaler, 0),
		position + sf::Vector2f(drawnSprite.getLocalBounds().width / 8 * scaler, 1),
		sf::Color::Magenta,
		sf::Vector2f(0, drawnSprite.getLocalBounds().height / 2 * scaler));

	boundingBoxes.emplace("GroundBox", ground);

}
