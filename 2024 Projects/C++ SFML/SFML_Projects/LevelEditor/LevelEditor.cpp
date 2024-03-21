#include "LevelEditor.h"

LevelEditor::LevelEditor()
{
	selectedTileID = 0;
	arrayWidth = 200;
	arrayHeight = 200;

	std::cout << "Set dimensions" << std::endl;

	CreateArray();

	std::cout << "Created Array" << std::endl;

	textures = GlobalVariables::getTextures();

	std::cout << "Got textures" << std::endl;

	cellSize =
		textures[0]->getSize().x *
		GlobalVariables::Instance()->getTextureScaler();

	std::cout << "Created cell size" << std::endl;

	leftPressed = false;

	currentEditMode = EditMode::Tile;
	currentTileMode = TileMode::Select;

	std::cout << "Set current modes" << std::endl;

	CreateSelectMenu();

	placeCoolDown = sf::seconds(.25f);
}

void LevelEditor::CreateArray()
{
	tileArray = std::vector<std::vector<TileData*>>(arrayHeight, std::vector<TileData*>(arrayWidth));

	for (size_t i = 0; i < arrayWidth; i++)
	{
		for (size_t j = 0; j < arrayHeight; j++)
		{
			tileArray[i][j] = new TileData();
		}
	}
}

void LevelEditor::Update(sf::RenderWindow& window)
{
	sf::Vector2f mousePosition = window.mapPixelToCoords(sf::Mouse::getPosition(window));

	switch (currentEditMode) {
	case EditMode::Tile: {
		TileMode(window, mousePosition);
	}
					   break;

	case EditMode::BoundingBoxPos: {
		BoundingBoxMode(window, mousePosition);
	}
								 break;

	case EditMode::CameraPosition: {
		CameraPositionMode(window, mousePosition);
	}
								 break;
	case EditMode::Save: {
		SaveLevel(mousePosition);
	}
					   break;

	}

	bool isFPressed = false;

	if (sf::Keyboard::isKeyPressed(sf::Keyboard::F)) {
		isFPressed = true;
	}

	if (isFPressed && !wasFPressed) {
		currentEditMode = (EditMode)((currentEditMode + 1) % 4);
		std::cout << currentEditMode << std::endl;
	}

	wasFPressed = isFPressed;
}

void LevelEditor::TileMode(sf::RenderWindow& window, sf::Vector2f mousePosition)
{
	switch (currentTileMode) {
	case TileMode::Place: {

		bool rightPressed = false;

		if (sf::Mouse::isButtonPressed(sf::Mouse::Right)) {
			rightPressed = true;
		}

		if (rightPressed && !rightWasPressed) {
			SwapMode();
		}

		if (sf::Mouse::isButtonPressed(sf::Mouse::Left) && clock.getElapsedTime() - timeOfSwitch > placeCoolDown) {

			int gridX = mousePosition.x / cellSize;
			int gridY = mousePosition.y / cellSize;

			// Round the grid coordinates to the nearest whole number
			gridX = std::round(gridX);
			gridY = std::round(gridY);

			startPos = sf::Vector2i(gridX, gridY);

			//std::cout << "Position Set: " << gridX << "," << gridY << std::endl;

			float scaler = GlobalVariables::getTextureScaler() * textures[0]->getSize().x;

			if (IsInArray(startPos) /*&& tileArray[gridX][gridY]->getID() != selectedTileID*/) {
				SetTile(startPos, scaler);
			}
		}

		rightWasPressed = rightPressed;
	}
						break;

	case TileMode::Select: {
		for (size_t i = 0; i < items.size(); i++)
		{
			if (items[i].CheckCollision(mousePosition) && sf::Mouse::isButtonPressed(sf::Mouse::Left)) {
				selectedTileID = items[i].GetID();

				timeOfSwitch = clock.getElapsedTime();

				currentTileMode = TileMode::Place;
			}
		}
	}
						 break;

	case TileMode::Delete: {

		bool rightPressed = false;

		if (sf::Mouse::isButtonPressed(sf::Mouse::Right)) {
			rightPressed = true;
		}

		if (rightPressed && !rightWasPressed) {
			SwapMode();
		}

		if (sf::Mouse::isButtonPressed(sf::Mouse::Left)) {

			int gridX = mousePosition.x / cellSize;
			int gridY = mousePosition.y / cellSize;

			// Round the grid coordinates to the nearest whole number
			gridX = std::round(gridX);
			gridY = std::round(gridY);

			startPos = sf::Vector2i(gridX, gridY);

			//std::cout << "Position Deleted: " << gridX << "," << gridY << std::endl;

			float scaler = GlobalVariables::getTextureScaler() * textures[0]->getSize().x;

			if (IsInArray(startPos) /*&& tileArray[gridX][gridY]->getID() != selectedTileID*/) {
				DeleteTile(startPos);
			}
		}

		rightWasPressed = rightPressed;
	}
						 break;

	}


	bool isKeyPressed = false;

	if (sf::Keyboard::isKeyPressed(sf::Keyboard::Space)) {
		isKeyPressed = true;
	}

	if (!wasKeyPressed && isKeyPressed) {
		if (currentTileMode == TileMode::Select) {
			currentTileMode = TileMode::Place;
		}
		else if (currentTileMode != TileMode::Select) {
			currentTileMode = TileMode::Select;
		}
	}

	wasKeyPressed = isKeyPressed;
}

void LevelEditor::BoundingBoxMode(sf::RenderWindow& window, sf::Vector2f mousePosition)
{
	SetBoundingBox(bbArray, mousePosition, sf::Color::Yellow);

	DeleteFromArray(bbArray);
}

void LevelEditor::CameraPositionMode(sf::RenderWindow& window, sf::Vector2f mousePosition)
{
	SetBoundingBox(cameraArray, mousePosition, sf::Color::Magenta);

	DeleteFromArray(cameraArray);


}

void LevelEditor::SetTile(sf::Vector2i position, float scaler)
{
	tileArray[position.x][position.y] = new TileData(
		textures[selectedTileID],
		sf::Vector2f(position.x * scaler, position.y * scaler),
		GlobalVariables::getTextureScaler(),
		selectedTileID,
		sf::Vector2f(startPos));
}

void LevelEditor::DeleteTile(sf::Vector2i position)
{
	delete tileArray[position.x][position.y];

	TileData* tile = new TileData();

	tileArray[position.x][position.y] = tile;
}

void LevelEditor::SwapMode()
{
	if (currentTileMode == TileMode::Place) {
		currentTileMode = TileMode::Delete;
	}
	else if
		(currentTileMode == TileMode::Delete) {
		currentTileMode = TileMode::Place;
	}

	std::cout << "Mode Swapped: " << currentTileMode << std::endl;
}

void LevelEditor::CreateSelectMenu()
{
	items = std::vector<SelectionItem>();

	int x = 0;
	int y = 0;

	for (auto& pair : textures)
	{
		sf::Vector2f position(y * 250 + 100, x * 250 + 100);

		items.push_back(SelectionItem(pair.second, position, pair.first));

		if (x % 8 == 0) {
			x = 0;
			y++;
		}
	}
}

bool LevelEditor::IsInArray(sf::Vector2i position)
{
	return
		position.x < tileArray.size() && position.x >= 0 &&
		position.y < tileArray[0].size() && position.y >= 0;
}

void LevelEditor::SetBoundingBox(std::vector<BoundingBox>& array, sf::Vector2f mousePosition, sf::Color color)
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

		float scale = textures[0]->getSize().x * GlobalVariables::getTextureScaler();
		sf::Vector2f position1(startPos.x * scale, startPos.y * scale);
		sf::Vector2f position2(endPos.x * scale + scale, endPos.y * scale + scale);

		array.push_back(BoundingBox(position1, position2, color, startPos, endPos));
	}
}

void LevelEditor::DeleteFromArray(std::vector<BoundingBox>& array)
{
	bool isKeyPressed = false;

	if (sf::Keyboard::isKeyPressed(sf::Keyboard::Backspace)) {
		isKeyPressed = true;
	}

	if (wasKeyPressed && !isKeyPressed && array.size() > 0) {
		array.pop_back();

	}

	wasKeyPressed = isKeyPressed;
}

void LevelEditor::SaveLevel(sf::Vector2f mousePosition) {

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
		
		arraySize = endPos - startPos + sf::Vector2i(1,1);

		std::cout << "Array Size: " << arraySize.x << "," << arraySize.y << std::endl;
	}

	bool isKeyPressed = false;


	if (sf::Keyboard::isKeyPressed(sf::Keyboard::L)) {
		isKeyPressed = true;
	}

	if (!wasKeyPressed && isKeyPressed) {
		std::cout << "Array Start: " << startPos.x << "," << startPos.y << std::endl;
		std::cout << "Array Size: " << arraySize.x << "," << arraySize.y << std::endl;

		std::vector<std::string> data;

		std::string size = std::to_string(arraySize.y) + "," + std::to_string(arraySize.x) + ",";
		data.push_back(size);

		size = std::to_string(3) + "," + std::to_string(-1) + ",";
		data.push_back(size);

		for (size_t i = 0; i < arraySize.x; i++)
		{
			std::string line;

			for (size_t j = 0; j < arraySize.y; j++)
			{
				line += std::to_string(tileArray[i + startPos.x][j + startPos.y]->getID()) + ",";
			}

			data.push_back(line);
		}

		FileIO::WriteToFile("Levels/EditorTest.txt", data);

		data.clear();

		for (size_t i = 0; i < bbArray.size(); i++)
		{
			data.push_back(
				std::to_string(bbArray[i].getPos1().x) + ":" +
				std::to_string(bbArray[i].getPos1().y) + ":" + "," +
				std::to_string(bbArray[i].getPos2().x) + ":" +
				std::to_string(bbArray[i].getPos2().y) + ":" + ",");
		}

		FileIO::WriteToFile("Levels/EditorTestBB.txt", data);

		data.clear();

		for (size_t i = 0; i < cameraArray.size(); i++)
		{
			data.push_back(
				std::to_string(cameraArray[i].getPos1().x) + ":" +
				std::to_string(cameraArray[i].getPos1().y) + ":" + "," +
				std::to_string(cameraArray[i].getPos2().x) + ":" +
				std::to_string(cameraArray[i].getPos2().y) + ":" + ",");
		}

		FileIO::WriteToFile("Levels/EditorTestCBB.txt", data);
	}

	wasKeyPressed = isKeyPressed;
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
			tileArray[i][j]->Draw(window);
		}
	}

	for (size_t i = 0; i < bbArray.size(); i++)
	{
		bbArray[i].Draw(window);
	}

	for (size_t i = 0; i < cameraArray.size(); i++)
	{
		cameraArray[i].Draw(window);
	}

	switch (currentEditMode) {
	case EditMode::Tile: {
		switch (currentTileMode) {
		case TileMode::Select: {

			sf::RectangleShape rect(ViewManager::Instance()->GetWindowView().getSize());
			rect.setOrigin(rect.getSize().x / 2, rect.getSize().y / 2);
			rect.setPosition(ViewManager::Instance()->GetWindowView().getCenter());

			sf::Color color = sf::Color::White;
			color.a = 50;

			rect.setFillColor(color);

			window.draw(rect);

			for (size_t i = 0; i < items.size(); i++)
			{
				items[i].Draw(window);
			}
		}
							 break;
		}
	}
					   break;
	case EditMode::BoundingBoxPos: {

	}
	}
}
