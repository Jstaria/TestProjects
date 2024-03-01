#pragma once

#include <SFML/Graphics.hpp>

class TileData
{
private:
	sf::Sprite sprite;
	sf::Texture texture;
	sf::Vector2f position;

public:
	TileData(sf::Texture texture, sf::Vector2f position);

};

