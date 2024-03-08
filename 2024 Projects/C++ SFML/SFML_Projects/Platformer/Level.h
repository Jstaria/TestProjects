#pragma once

#include <iostream>
#include <SFML/Graphics.hpp>
#include <map>
#include <vector>
#include "BoundingBox.h"
#include "FileIO.h"
#include "TileData.h"
#include <list>

class Level
{
private:
	std::string levelPath;
	float textureScaler;

	int arrayWidth;
	int arrayHeight;

	sf::Vector2f playerStartPos;

	/*std::vector<std::vector<TileData>>*/
	std::vector<std::vector<TileData>> tileArray;
	std::list<BoundingBox>* bbArray;
	std::map<int, sf::Texture> textures;

public:
	Level(std::string levelPath);
	~Level();

	void LoadTileData(std::string filePath);

	void CreateBB(std::string filePath);

	void Draw(sf::RenderWindow& window);

	std::list<BoundingBox>* getBBArray();
	sf::Vector2f getPlayerPos();
};

