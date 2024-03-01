#pragma once

#include <iostream>
#include <SFML/Graphics.hpp>
#include <map>
#include <vector>
#include "BoundingBox.h"
#include "FileIO.h"
#include "TileData.h"

class Level
{
private:
	std::string levelDirectoryPath;
	sf::Vector2f textureScaler;

	std::vector<std::vector<TileData>>* tileArray;
	std::vector<BoundingBox>* bbArray;

public:
	Level(std::string levelDirectoryPath, sf::Vector2f textureScaler);

	~Level() {
		delete tileArray;
		delete bbArray;
	}
};

