#include "GlobalVariables.h"

GlobalVariables* GlobalVariables::s_instance = 0;
float GlobalVariables::textureScaler;
std::map< std::string, std::map<int, sf::Texture*>> GlobalVariables::textures;

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

std::map<int, sf::Texture*> GlobalVariables::getTextures(std::string name)
{
	return textures[name];
}

void GlobalVariables::setTextures(std::map<int, sf::Texture*>& textures, std::string name)
{
	GlobalVariables::textures[name] = textures;
}
