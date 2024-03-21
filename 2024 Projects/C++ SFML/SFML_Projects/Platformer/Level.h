#pragma once

#include <iostream>
#include <SFML/Graphics.hpp>
#include <map>
#include <vector>
#include "BoundingBox.h"
#include "FileIO.h"
#include "TileData.h"
#include <list>
#include "Camera.h"

class Level
{
private:
	std::string levelPath;
	float textureScaler;

	int arrayWidth;
	int arrayHeight;

	sf::Vector2f playerStartPos;
	Camera* camera;

	/*std::vector<std::vector<TileData>>*/
	std::vector<std::vector<TileData>> tileArray;
	std::list<BoundingBox>* bbArray;
	std::map<int, sf::Texture*> textures;

public:
	Level(std::string levelPath);
	Level(std::string imagePath, bool);
	Level(std::string levelPath, Camera* camera);

	~Level();

	void LoadTileData(std::string filePath);
	void LoadTileDataPNG(std::string imagePath);

	void CreateBB(std::string filePath);
	void CreateCameraBB(std::string filePath);

	void Draw(sf::RenderWindow& window);

	std::list<BoundingBox>* getBBArray();
	sf::Vector2f getPlayerPos();
};

