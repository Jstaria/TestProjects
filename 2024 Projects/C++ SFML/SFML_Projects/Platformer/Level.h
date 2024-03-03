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
	int textureScaler;

	int arrayWidth;
	int arrayHeight;

	/*std::vector<std::vector<TileData>>*/
	std::vector<std::vector<TileData>> tileArray;
	std::list<BoundingBox> bbArray;
	std::map<int, sf::Texture> textures;

public:
	Level(std::string levelPath, int textureScaler, std::map<int,sf::Texture> textures);

	~Level();

	void LoadTileData(std::string filePath);

	void CreateBB(std::string filePath);

	void Draw(sf::RenderTexture& target);
};

