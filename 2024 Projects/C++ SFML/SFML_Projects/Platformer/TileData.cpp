#include "TileData.h"

TileData::TileData(sf::Texture* texture, sf::Vector2f position, float scaler, int tileID, sf::Vector2f arrayPos) :
	texture(texture), position(position), tileID(tileID), arrayPos(arrayPos)
{
	sprite.setTexture(*texture);
	sprite.setPosition(position);
	sprite.setScale(scaler, scaler);

	isActive = true;

	rectPos = sf::FloatRect(sprite.getTextureRect());
}

TileData::TileData() {
	isActive = false;
}

void TileData::Draw(sf::RenderWindow& window)
{
	window.draw(sprite);
}

int TileData::getID() {
	return tileID;
}

sf::FloatRect TileData::getPosition()
{
	return rectPos;
}

bool TileData::IsActive()
{
	return isActive;
}
