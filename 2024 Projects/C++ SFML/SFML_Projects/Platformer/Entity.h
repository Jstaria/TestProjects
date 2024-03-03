#pragma once
#include <SFML/Graphics.hpp>

class Entity
{
protected:
	std::map<std::string, sf::Sprite>* sprites;
	sf::Vector2f position;
	sf::Sprite currentSprite;
	sf::Sprite drawnSprite;
	int frameNum;
	int maxFrames;
	int frameWidth;
	int frameHeight;

public:
	Entity(std::map<std::string, sf::Sprite>* sprites, sf::Vector2f position, int maxFrames);

	void Draw(sf::RenderWindow& window);

	void Update();
};

