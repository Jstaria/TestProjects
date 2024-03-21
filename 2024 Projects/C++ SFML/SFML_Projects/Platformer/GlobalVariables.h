#pragma once

#include <iostream>
#include <map>
#include <SFML/Graphics.hpp>

class GlobalVariables {
private:
	static GlobalVariables* s_instance;

	static float textureScaler;
	static std::map<int, sf::Texture> textures;
	static sf::Vector2f playerPosition;

public:
	GlobalVariables();

	static GlobalVariables* Instance();

	static float getTextureScaler();
	static void setTextureScaler(float scaler);

	static std::map<int, sf::Texture> getTextures();
	static void setTextures(std::map<int, sf::Texture> textures);
	static sf::Vector2f getPlayerPosition();
	static void setPlayerPosition(sf::Vector2f position);
};

