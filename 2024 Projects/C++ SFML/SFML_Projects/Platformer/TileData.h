#pragma once

#include <SFML/Graphics.hpp>
#include <iostream>

class TileData
{
private:
	sf::Sprite sprite;
	sf::Texture* texture;
	sf::Vector2f position;

	bool isActive;
	int tileID;

	sf::Vector2f arrayPos;

public:
	TileData(sf::Texture* texture, sf::Vector2f position, int scaler, int tileID, sf::Vector2f arrayPos);
	TileData();

	void Draw(sf::RenderTexture& target);

	int getID();

	bool IsActive();
};
