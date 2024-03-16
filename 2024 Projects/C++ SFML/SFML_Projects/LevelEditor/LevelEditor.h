#pragma once
#include "Level.h"
#include "GlobalVariables.h"
#include "ViewManager.h"
#include <iostream>
#include "SelectionItem.h"

enum EditMode {
	Tile,
	BoundingBoxPos,
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
	std::vector<std::vector<TileData*>> tileArray;
	std::vector<SelectionItem> items;
	std::vector<BoundingBox> bbArray;
	int arrayWidth 	;
	int arrayHeight ;

	int cellSize;

	sf::Vector2i startPos;
	sf::Vector2i endPos;

	bool leftPressed;
	bool rightWasPressed;
	bool wasKeyPressed;
	bool wasFPressed;

	EditMode currentEditMode;
	TileMode currentTileMode;

	int selectedTileID;

	sf::Clock clock;
	sf::Time placeCoolDown;
	sf::Time timeOfSwitch;

	std::map<int, sf::Texture*> textures;

	void TileMode(sf::RenderWindow& window, sf::Vector2f mousePosition);
	void BoundingBoxMode(sf::RenderWindow& window, sf::Vector2f mousePosition);
	void CameraPositionMode(sf::RenderWindow& window, sf::Vector2f mousePosition);

	void SetTile(sf::Vector2i position, float scaler);
	void DeleteTile(sf::Vector2i position);
	void SwapMode();

	void CreateSelectMenu();
	void CreateArray();

	bool IsInArray(sf::Vector2i position);

public:
	LevelEditor();

	void Update(sf::RenderWindow& window);

	void Draw(sf::RenderWindow& window);
};

