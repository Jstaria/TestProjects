#include "Player.h"
#include <iostream>

const float Scaler = 4;

Player::Player(std::map<std::string, sf::Sprite>* sprites, sf::Vector2f position, int maxFrames) 
	: Entity(sprites, position, maxFrames) {
	
	for (const auto& pair : *Player::sprites) {
		sf::Sprite& sprite = (*Player::sprites)[pair.first];

		sprite.setScale(Scaler, Scaler);
		sprite.setPosition(position);
	}

	std::string key = "walk";
	this->frameWidth = (*this->sprites)[key].getLocalBounds().width / maxFrames;
	this->frameHeight = (*this->sprites)[key].getLocalBounds().height;

	this->speedMultiplier = 10;
	this->direction = sf::Vector2f(0, 0);

	lastFacedDirectionX = 1;
	currentSprite = (*this->sprites)[key];
}

void Player::Update() {
	sf::Vector2f direction(0, 0);
	float lerpSpeed = .25f;
	float movementLerp = .1f;

	currentSprite = (*sprites)[std::string("idle")];

	//if (sf::Keyboard::isKeyPressed(sf::Keyboard::W)) {
	//	direction.y -= 1;
	//	lerpSpeed = movementLerp;
	//	currentSprite = (*sprites)[std::string("walk")];
	//}

	//if (sf::Keyboard::isKeyPressed(sf::Keyboard::S)) {
	//	direction.y += 1;
	//	lerpSpeed = movementLerp;
	//	currentSprite = (*sprites)[std::string("walk")];
	//}

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

	//std::cout << direction.x << "," << direction.y << std::endl;
	Move(Player::direction);

	drawnSprite = Player::GetCurrentSprite(currentSprite, lastFacedDirectionX);

	if (direction.x != 0) {
		lastFacedDirectionX = direction.x;
	}
}

void Player::Draw(sf::RenderWindow& window) {
	window.draw(drawnSprite);
}

sf::Sprite Player::GetCurrentSprite(sf::Sprite& currentSprite, int xDirection) {

	const sf::Texture* texture = currentSprite.getTexture();

	sf::Sprite newSprite(*texture, sf::IntRect(frameWidth * frameNum, 0, frameWidth, frameHeight));
	newSprite.setScale(Scaler * sign(xDirection), Scaler);
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
}