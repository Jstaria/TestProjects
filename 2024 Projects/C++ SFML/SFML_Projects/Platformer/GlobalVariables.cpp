#include "GlobalVariables.h"

GlobalVariables* GlobalVariables::s_instance = 0;
float GlobalVariables::textureScaler;
sf::Vector2f GlobalVariables::playerPosition;
BoundingBox* GlobalVariables::playerBB;
Input* GlobalVariables::input = nullptr;
std::map<std::string, std::map<int, sf::Texture*>> GlobalVariables::textures;
std::map<std::string, std::map<std::string, sf::Sprite>*> GlobalVariables::sprites;
std::map<std::string, sf::Shader*> GlobalVariables::shaders;
FastNoiseLite GlobalVariables::noise;
sf::Clock GlobalVariables::clock;
Camera* GlobalVariables::camera;

GlobalVariables::GlobalVariables()
{
	noise = FastNoiseLite();
	noise.SetNoiseType(FastNoiseLite::NoiseType_Perlin);
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

sf::Shader* GlobalVariables::getShader(std::string shaderName)
{
	return shaders[shaderName];
}

void GlobalVariables::setShader(std::string shaderName, sf::Shader* shader)
{
	shaders[shaderName] = shader;
}

FastNoiseLite GlobalVariables::getNoise() {
	return noise;
}

sf::Clock GlobalVariables::getClock() {
	return clock;
}

void GlobalVariables::setCamera(Camera* camera) {
	GlobalVariables::camera = camera;
}

Camera* GlobalVariables::getCamera() {
	return camera;
}