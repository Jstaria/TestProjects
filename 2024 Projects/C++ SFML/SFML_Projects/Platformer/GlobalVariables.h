#pragma once

#include <iostream>
#include <map>
#include <SFML/Graphics.hpp>
#include "BoundingBox.h"
#include "Input.h"
#include "FastNoiseLite.h"

class GlobalVariables {
private:
	static GlobalVariables* s_instance;

	static float textureScaler;
	static std::map<std::string, std::map<int, sf::Texture*>> textures;
	static std::map<std::string, std::map<std::string, sf::Sprite>*> sprites;
	static std::map<std::string, sf::Shader*> shaders;
	static sf::Vector2f playerPosition;
	static Input* input;
	static BoundingBox* playerBB;
	static FastNoiseLite noise;
	static sf::Clock clock;
	//static Camera* camera;

public:
	GlobalVariables();

	static GlobalVariables* Instance();

	static float getTextureScaler();
	static void setTextureScaler(float scaler);

	static std::map<int, sf::Texture*> getTextures(std::string textureName);
	static void setTextures(std::map<int, sf::Texture*>& textures, std::string textureName);
	static std::map<std::string, sf::Sprite>* getSprites(std::string spriteName);
	static void setSprites(std::map<std::string, sf::Sprite>* sprites, std::string spriteName);
	static sf::Vector2f getPlayerPosition();
	static BoundingBox getPlayerBB();
	
	static void setPlayerPosition(sf::Vector2f position);
	static void setPlayerBB(BoundingBox* bb);

	static sf::Shader* getShader(std::string shaderName);
	static void setShader(std::string shaderName, sf::Shader* shader);

	static void setInput(Input* input);
	static Input* getInput();

	static FastNoiseLite getNoise();
	static sf::Clock getClock();

	//static void setCamera(Camera* camera);
	//static Camera* getCamera();
};

