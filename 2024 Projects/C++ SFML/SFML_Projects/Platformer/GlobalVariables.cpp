#include "GlobalVariables.h"

GlobalVariables* GlobalVariables::s_instance = 0;
float GlobalVariables::textureScaler;
sf::Vector2f GlobalVariables::playerPosition;
std::map<int, sf::Texture*> GlobalVariables::textures;

GlobalVariables::GlobalVariables()
{
}

GlobalVariables* GlobalVariables::Instance()
{
	if (s_instance == nullptr) {
		s_instance = new GlobalVariables();
	}
	return s_instance;
}

float GlobalVariables::getTextureScaler()
{
	return textureScaler;
}

void GlobalVariables::setTextureScaler(float scaler)
{
	textureScaler = scaler;
}

std::map<int, sf::Texture*> GlobalVariables::getTextures()
{
	return textures;
}

void GlobalVariables::setTextures(std::map<int, sf::Texture*>& textures)
{
	GlobalVariables::textures = textures;
}

sf::Vector2f GlobalVariables::getPlayerPosition()
{
	return playerPosition;
}

void GlobalVariables::setPlayerPosition(sf::Vector2f position)
{
	playerPosition = position;
}
