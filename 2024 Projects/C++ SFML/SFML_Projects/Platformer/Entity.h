#pragma once
#include <SFML/Graphics.hpp>

class Entity
{
protected:
	sf::Sprite sprite;
	sf::Vector2f position;

public:
	Entity(sf::Sprite sprite, sf::Vector2f position);

	void Draw(sf::RenderWindow& window);

	void Update();
};

