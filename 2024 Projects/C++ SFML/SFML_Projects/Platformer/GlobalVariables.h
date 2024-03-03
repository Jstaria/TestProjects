#pragma once

#include <iostream>
#include <map>
#include <SFML/Graphics.hpp>

class GlobalVariables {
private:
	static GlobalVariables* s_instance;

	static int textureScaler;
	static std::map<int, sf::Texture> textures;

public:
	GlobalVariables();

	static GlobalVariables* Instance();

	static int getTextureScaler();
	static void setTextureScaler(int scaler);

	static std::map<int, sf::Texture> getTextures();
	static void setTextures(std::map<int, sf::Texture> textures);
};

