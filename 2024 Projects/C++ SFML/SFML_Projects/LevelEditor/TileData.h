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
	TileData(sf::Texture* texture, sf::Vector2f position, float scaler, int tileID, sf::Vector2f arrayPos);
	TileData();
	~TileData();
	void Draw(sf::RenderWindow& window);

	int getID();

	bool IsActive();
};
