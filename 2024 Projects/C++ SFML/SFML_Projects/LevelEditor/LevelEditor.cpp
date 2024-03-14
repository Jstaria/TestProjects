#include "LevelEditor.h"

LevelEditor::LevelEditor()
{
	selectedTileID = 0;
	arrayWidth = 200;
	arrayHeight = 200;

	CreateArray();

	cellSize =
		GlobalVariables::Instance()->getTextures()[0].getSize().x *
		GlobalVariables::Instance()->getTextureScaler();

	leftPressed = false;

	currentEditMode = EditMode::Tile;
	currentTileMode = TileMode::Place;

	textures = GlobalVariables::getTextures();
}

void LevelEditor::CreateArray()
{
	tileArray = std::vector<std::vector<TileData>>(arrayHeight, std::vector<TileData>(arrayWidth));
}

void LevelEditor::Update(sf::RenderWindow& window)
{
	sf::Vector2f mousePosition = window.mapPixelToCoords(sf::Mouse::getPosition(window));

	switch (currentEditMode) {
	case EditMode::Tile: {
		TileMode(window, mousePosition);
	}
					   break;

	case EditMode::BoundingBox: {
		BoundingBoxMode(window, mousePosition);
	}
							  break;

	case EditMode::CameraPosition: {
		CameraPositionMode(window, mousePosition);
	}
								 break;

	}
}

void LevelEditor::TileMode(sf::RenderWindow& window, sf::Vector2f mousePosition)
{
	switch (currentTileMode) {
	case TileMode::Place: {
		if (sf::Mouse::isButtonPressed(sf::Mouse::Left)) {

			int gridX = mousePosition.x / cellSize;
			int gridY = mousePosition.y / cellSize;

			// Round the grid coordinates to the nearest whole number
			gridX = std::round(gridX);
			gridY = std::round(gridY);

			startPos = sf::Vector2i(gridX, gridY);

			std::cout << "Position Set: " << gridX << "," << gridY << std::endl;

			float scaler = GlobalVariables::getTextureScaler() * GlobalVariables::getTextures()[0].getSize().x;

			if (IsInArray(startPos)) {
				sf::Texture* texture = &textures[selectedTileID];

				TileData tile(
					texture, 
					sf::Vector2f(gridX * scaler, gridY * scaler), 
					GlobalVariables::getTextureScaler(), 
					selectedTileID, 
					sf::Vector2f(startPos));

				tileArray[gridX][gridY] = tile;
			}
		}
	}
					   break;

	case TileMode::Select: {
		
	}
							  break;

	case TileMode::Delete: {
		if (sf::Mouse::isButtonPressed(sf::Mouse::Left)) {

			int gridX = mousePosition.x / cellSize;
			int gridY = mousePosition.y / cellSize;

			// Round the grid coordinates to the nearest whole number
			gridX = std::round(gridX);
			gridY = std::round(gridY);

			startPos = sf::Vector2i(gridX, gridY);

			std::cout << "Position Deleted: " << gridX << "," << gridY << std::endl;
		}
	}
								 break;

	}

}

void LevelEditor::BoundingBoxMode(sf::RenderWindow& window, sf::Vector2f mousePosition)
{
	if (sf::Mouse::isButtonPressed(sf::Mouse::Left) && !leftPressed) {

		int gridX = mousePosition.x / cellSize;
		int gridY = mousePosition.y / cellSize;

		// Round the grid coordinates to the nearest whole number
		gridX = std::round(gridX);
		gridY = std::round(gridY);

		startPos = sf::Vector2i(gridX, gridY);

		std::cout << "First Pos: " << gridX << "," << gridY << std::endl;

		leftPressed = true;
	}

	if (sf::Mouse::isButtonPressed(sf::Mouse::Right) && leftPressed) {
		int gridX = mousePosition.x / cellSize;
		int gridY = mousePosition.y / cellSize;

		// Round the grid coordinates to the nearest whole number
		gridX = std::round(gridX);
		gridY = std::round(gridY);

		std::cout << "Second Pos: " << gridX << "," << gridY << std::endl;

		endPos = sf::Vector2i(gridX, gridY);

		leftPressed = false;
	}
}

void LevelEditor::CameraPositionMode(sf::RenderWindow& window, sf::Vector2f mousePosition)
{
}

void LevelEditor::SetTile(TileData tile, sf::Vector2f position)
{

}

void LevelEditor::DeleteTile(sf::Vector2f position)
{

}

bool LevelEditor::IsInArray(sf::Vector2i position)
{
	return
		position.x < tileArray.size() && position.x >= 0 &&
		position.y < tileArray[0].size() && position.y >= 0;
}


void LevelEditor::Draw(sf::RenderWindow& window)
{
	// Draw vertical lines
	for (int x = 0; x <= cellSize * arrayWidth; x += cellSize)
	{
		sf::Vertex line[] =
		{
			sf::Vertex(sf::Vector2f(x, 0)),
			sf::Vertex(sf::Vector2f(x, cellSize * arrayHeight))
		};
		window.draw(line, 2, sf::Lines);
	}

	// Draw horizontal lines
	for (int y = 0; y <= cellSize * arrayHeight; y += cellSize)
	{
		sf::Vertex line[] =
		{
			sf::Vertex(sf::Vector2f(0, y)),
			sf::Vertex(sf::Vector2f(cellSize * arrayWidth, y))
		};
		window.draw(line, 2, sf::Lines);
	}

	for (size_t i = 0; i < arrayWidth; i++)
	{
		for (size_t j = 0; j < arrayWidth; j++)
		{
			tileArray[i][j].Draw(window);
		}
	}
}
