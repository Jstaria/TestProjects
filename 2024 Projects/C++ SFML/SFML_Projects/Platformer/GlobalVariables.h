#pragma once

#include <iostream>
#include <map>
#include <SFML/Graphics.hpp>
#include "BoundingBox.h"
#include "Input.h"

class GlobalVariables {
private:
	static GlobalVariables* s_instance;

	static float textureScaler;
	static std::map<std::string, std::map<int, sf::Texture*>> textures;
	static sf::Vector2f playerPosition;
	static Input* input;
	static BoundingBox* playerBB;

public:
	GlobalVariables();

	static GlobalVariables* Instance();

	static float getTextureScaler();
	static void setTextureScaler(float scaler);

	static std::map<int, sf::Texture*> getTextures(std::string textureName);
	static void setTextures(std::map<int, sf::Texture*>& textures, std::string textureName);
	static sf::Vector2f getPlayerPosition();
	static BoundingBox getPlayerBB();
	
	static void setPlayerPosition(sf::Vector2f position);
	static void setPlayerBB(BoundingBox* bb);

	static void setInput(Input* input);
	static Input* getInput();
};

