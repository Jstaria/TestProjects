#include "GlobalVariables.h"

GlobalVariables* GlobalVariables::s_instance = 0;
int GlobalVariables::textureScaler;
std::map<int, sf::Texture> GlobalVariables::textures;

GlobalVariables::GlobalVariables()
{
}

GlobalVariables* GlobalVariables::Instance()
{
	return s_instance;
}

int GlobalVariables::getTextureScaler()
{
	return textureScaler;
}

void GlobalVariables::setTextureScaler(int scaler)
{
	textureScaler = scaler;
}

std::map<int, sf::Texture> GlobalVariables::getTextures()
{
	return textures;
}

void GlobalVariables::setTextures(std::map<int, sf::Texture> textures)
{
	GlobalVariables::textures = textures;
}
