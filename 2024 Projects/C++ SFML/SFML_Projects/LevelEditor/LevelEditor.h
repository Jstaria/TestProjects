#pragma once
#include "Level.h"
#include "GlobalVariables.h"
#include "ViewManager.h"
#include <iostream>

enum EditMode {
	Tile,
	BoundingBox,
	CameraPosition
};

enum TileMode {
	Place,
	Select,
	Delete
};

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

	EditMode currentEditMode;
	TileMode currentTileMode;

	int selectedTileID;

	std::map<int, sf::Texture> textures;

public:
	LevelEditor();

	void CreateArray();

	void Update(sf::RenderWindow& window);
	void TileMode(sf::RenderWindow& window, sf::Vector2f mousePosition);
	void BoundingBoxMode(sf::RenderWindow& window, sf::Vector2f mousePosition);
	void CameraPositionMode(sf::RenderWindow& window, sf::Vector2f mousePosition);

	void SetTile(TileData tile, sf::Vector2f position);
	void DeleteTile(sf::Vector2f position);

	bool IsInArray(sf::Vector2i position);

	void Draw(sf::RenderWindow& window);
};

