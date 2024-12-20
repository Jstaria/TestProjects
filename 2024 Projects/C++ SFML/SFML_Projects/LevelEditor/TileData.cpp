#include "TileData.h"

TileData::TileData(sf::Texture* texture, sf::Vector2f position, float scaler, int tileID, sf::Vector2f arrayPos) :
	texture(texture), position(position), tileID(tileID), arrayPos(arrayPos)
{
	sprite.setTexture(*texture);
	sprite.setPosition(position);
	sprite.setScale(scaler, scaler);

	isActive = true;
}

TileData::TileData() {
	isActive = false;
	texture = nullptr;
	tileID = -1;
}

TileData::~TileData()
{
	//std::cout << "Deleted" << std::endl;

	sprite.setPosition(sf::Vector2f(-100,-100));

}

void TileData::Draw(sf::RenderWindow& window)
{
	window.draw(sprite);
}

int TileData::getID() {
	return tileID;
}

bool TileData::IsActive()
{
	return isActive;
}
