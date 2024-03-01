#include "Player.h"

Player::Player(sf::Sprite sprite, sf::Vector2f position) : Entity(sprite, position) {
	this->sprite.setScale(4, 4);
	this->speed = 10;
}

//void Player::SetKeyMap() {
//
//	keyMap = {
//		{ sf::Keyboard::A, [&](sf::Vector2f& speed) { Move(speed); }},
//		{ sf::Keyboard::D, [&](sf::Vector2f& speed) { Move(speed); }},
//		{ sf::Keyboard::W, [&](sf::Vector2f& speed) { Move(speed); }},
//		{ sf::Keyboard::D, [&](sf::Vector2f& speed) { Move(speed); }},
//	};
//}

void Player::Update() {
	sf::Vector2f direction(0,0);

	if (sf::Keyboard::isKeyPressed(sf::Keyboard::W)) {
		direction.y -= 1;
	}

	if (sf::Keyboard::isKeyPressed(sf::Keyboard::S)) {
		direction.y += 1;
	}

	if (sf::Keyboard::isKeyPressed(sf::Keyboard::A)) {
		direction.x -= 1;
	}

	if (sf::Keyboard::isKeyPressed(sf::Keyboard::D)) {
		direction.x += 1;
	}

	direction = Normalize(direction, speed);

	Move(direction);
}

void Player::Move(sf::Vector2f speed) {
	position += speed;
	sprite.setPosition(position);
}

sf::Vector2f Player::Normalize(sf::Vector2f& vector, int multiplier) {
	float length = std::sqrt(vector.x * vector.x + vector.y * vector.y);

	if (length != 0.f) {
		return sf::Vector2f(vector.x / length * multiplier, vector.y / length * multiplier);
	}
	else {
		return vector;
	}
}


