#include "TileData.h"

TileData::TileData(sf::Texture* texture, sf::Vector2f position, int scaler, int tileID, sf::Vector2f arrayPos) :
	texture(texture), position(position), tileID(tileID), arrayPos(arrayPos)
{
	sprite.setTexture(*texture);
	sprite.setPosition(position);
	sprite.setScale(scaler, scaler);

	isActive = true;
}

TileData::TileData() {
	isActive = false;
}

void TileData::Draw(sf::RenderTexture& target)
{
	target.draw(sprite);
}

int TileData::getID() {
	return tileID;
}

bool TileData::IsActive()
{
	return isActive;
}
