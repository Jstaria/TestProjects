#pragma once
#include "Level.h"
#include "GlobalVariables.h"
#include "ViewManager.h"

class LevelEditor
{
private:
	std::vector<std::vector<TileData>> tileArray;
	int arrayWidth 	;
	int arrayHeight ;

	int cellSize;

	sf::Vector2i startPos;
	sf::Vector2i endPos;

	bool leftPressed;

public:
	LevelEditor();

	void Update(sf::RenderWindow& window);
	void GetMouseInteraction();

	void Draw(sf::RenderWindow& window);
};

