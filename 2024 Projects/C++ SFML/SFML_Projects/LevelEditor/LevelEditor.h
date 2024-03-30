#pragma once
#include "Level.h"
#include "GlobalVariables.h"
#include "ViewManager.h"
#include <iostream>
#include "SelectionItem.h"
#include "Interactable.h"

enum EditMode {
	Tile,
	BoundingBoxPos,
	CameraPosition,
	Interactables,
	Save
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
	std::vector<Interactable> interactables;
	std::vector<SelectionItem> levelItems;
	std::vector<SelectionItem> interactableItems;
	std::vector<BoundingBox> bbArray;
	std::vector<BoundingBox> cameraArray;

	sf::Vector2f playerStartPos;

	int brushSize;

	int arrayWidth 	;
	int arrayHeight ;

	int cellSize;

	sf::Vector2i startPos;
	sf::Vector2i endPos;

	bool leftPressed;
	bool rightWasPressed;
	bool wasKeyPressed;
	bool wasKeyPressed1;
	bool wasKeyPressed2;
	bool wasFPressed;

	EditMode currentEditMode;
	TileMode currentTileMode;

	int selectedTileID;
	int selectedInterID;

	sf::Clock clock;
	sf::Time placeCoolDown;
	sf::Time timeOfSwitch;

	std::map<int, sf::Texture*> textures;
	std::vector<sf::Sprite> previewSprites;

	sf::Vector2i arraySize;

	void TileMode(sf::RenderWindow& window, sf::Vector2f mousePosition);
	void BoundingBoxMode(sf::RenderWindow& window, sf::Vector2f mousePosition);
	void CameraPositionMode(sf::RenderWindow& window, sf::Vector2f mousePosition);
	void InteractableMode(sf::RenderWindow& window, sf::Vector2f mousePosition);

	void SetTile(sf::Vector2i position, float scaler);
	void DeleteTile(sf::Vector2i position);
	void SwapMode();

	void CreateSelectMenu();
	void CreateArray();

	bool IsInArray(sf::Vector2i position);

	void SetBoundingBox(std::vector<BoundingBox>& array, sf::Vector2f mousePosition, sf::Color color);
	void DeleteFromArray(std::vector<BoundingBox>& array);

	void SaveLevel(sf::Vector2f mousePosition);
	void PreviewSelection(EditMode mode, int selectionID, float scaler, sf::Vector2f position, sf::Color color);

	void LoadTileData(std::string filePath);
	void CreateBB(std::string filePath);
	void CreateCameraBB(std::string filePath);
	void CreateInteractables(std::string filePath);

public:
	LevelEditor();

	void Update(sf::RenderWindow& window);

	void Draw(sf::RenderWindow& window);
	
	void LoadLevel();
};

