#include "GlobalVariables.h"

GlobalVariables* GlobalVariables::s_instance = 0;
float GlobalVariables::textureScaler;
sf::Vector2f GlobalVariables::playerPosition;
BoundingBox* GlobalVariables::playerBB;
Input* GlobalVariables::input = nullptr;
std::map<std::string, std::map<int, sf::Texture*>> GlobalVariables::textures;
std::map<std::string, std::map<std::string, sf::Sprite>*> GlobalVariables::sprites;

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

std::map<int, sf::Texture*> GlobalVariables::getTextures(std::string textureName)
{
	return textures[textureName];
}

void GlobalVariables::setTextures(std::map<int, sf::Texture*>& textures, std::string textureName)
{
	GlobalVariables::textures[textureName] = textures;
}

std::map<std::string, sf::Sprite>* GlobalVariables::getSprites(std::string spriteName)
{
	return sprites[spriteName];
}

void GlobalVariables::setSprites(std::map<std::string, sf::Sprite>* sprites, std::string spriteName)
{
	GlobalVariables::sprites[spriteName] = sprites;
}

sf::Vector2f GlobalVariables::getPlayerPosition()
{
	return playerPosition;
}

BoundingBox GlobalVariables::getPlayerBB()
{
	return *playerBB;
}

void GlobalVariables::setPlayerPosition(sf::Vector2f position)
{
	playerPosition = position;
}

void GlobalVariables::setPlayerBB(BoundingBox* bb)
{
	playerBB = bb;
}

void GlobalVariables::setInput(Input* input)
{
	GlobalVariables::input = input;
}

Input* GlobalVariables::getInput()
{
	return input;
}



