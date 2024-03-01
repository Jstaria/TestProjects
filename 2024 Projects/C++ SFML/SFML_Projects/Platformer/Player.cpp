#include "Player.h"
#include <iostream>

Player::Player(sf::Sprite sprite, sf::Vector2f position) : Entity(sprite, position) {
	this->sprite.setScale(4, 4);
	this->sprite.setPosition(position);
	this->speedMultiplier = 10;
	this->direction = sf::Vector2f(0, 0);
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
	sf::Vector2f direction(0, 0);

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

	direction = Normalize(direction, speedMultiplier);

	Player::direction = lerp(Player::direction, direction, .05f);

	//std::cout << direction.x << "," << direction.y << std::endl;
	Move(Player::direction);
}

void Player::Move(sf::Vector2f speed) {
	position += speed;
	//std::cout << position.x << "," << position.y << std::endl;
	sprite.setPosition(position);
}