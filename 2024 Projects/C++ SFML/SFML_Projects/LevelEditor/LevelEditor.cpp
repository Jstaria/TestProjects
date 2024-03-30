#include "LevelEditor.h"
#include "Windows.h"

LevelEditor::LevelEditor()
{
	HANDLE hConsole = GetStdHandle(STD_OUTPUT_HANDLE);
	SetConsoleTextAttribute(hConsole, 14);

	std::cout << "(WASD) and (MMB) to move camera\n(+/-) to zoom camera\nPress (SPACE) while in tile mode to bring up selectable tiles\n(LMB) to place/delete tiles, (RMB) to switch from delete and place\nPress (UP/DOWN) to change brush size\nPress (F) to switch placing mode\n In Bounding Box mode(Where you set tile collision boxes), from left to right, LMB to set first position and RMB to set second position\nMake sure First is to the left and above the second position for this and the next two modes\nIn CameraBB mode, set the area a camera can move\nIn Save mode, you can select corners of your level to export by pressing (L)\nCrashes if you do not follow instructions on how to select corners!!!" << std::endl;


	SetConsoleTextAttribute(hConsole, 15);
	selectedTileID = 0;
	arrayWidth = 400;
	arrayHeight = 200;

	brushSize = 3;

	std::cout << "Set dimensions" << std::endl;

	CreateArray();

	std::cout << "Created Array" << std::endl;

	textures = GlobalVariables::getTextures("level");

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

	std::cout << "Created select menu" << std::endl;

	placeCoolDown = sf::seconds(.25f);

	LoadLevel();
}

void LevelEditor::CreateArray()
{
	tileArray = std::vector<std::vector<TileData*>>(arrayWidth, std::vector<TileData*>(arrayHeight));

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
	case EditMode::Interactables: {
		InteractableMode(window, mousePosition);
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
		currentEditMode = (EditMode)((currentEditMode + 1) % 5);
		std::cout << currentEditMode << std::endl;
	}

	wasFPressed = isFPressed;
}

void LevelEditor::TileMode(sf::RenderWindow& window, sf::Vector2f mousePosition)
{
	bool isKeyUpPressed = false;
	bool isKeyDownPressed = false;

	if (sf::Keyboard::isKeyPressed(sf::Keyboard::Up)) {
		isKeyUpPressed = true;
	}
	if (sf::Keyboard::isKeyPressed(sf::Keyboard::Down)) {
		isKeyDownPressed = true;
	}

	if (isKeyUpPressed && !wasKeyPressed2) {
		brushSize++;
	}
	if (isKeyDownPressed && !wasKeyPressed1) {
		brushSize = brushSize - 1 > 0 ? brushSize - 1 : 1;
	}

	wasKeyPressed2 = isKeyUpPressed;
	wasKeyPressed1 = isKeyDownPressed;

	int gridX = mousePosition.x / cellSize;
	int gridY = mousePosition.y / cellSize;

	// Round the grid coordinates to the nearest whole number
	gridX = std::round(gridX);
	gridY = std::round(gridY);

	startPos = sf::Vector2i(gridX, gridY);
	float scaler = GlobalVariables::getTextureScaler() * textures[0]->getSize().x;

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

			//std::cout << "Position Set: " << gridX << "," << gridY << std::endl;

			for (int i = -brushSize + 1; i < brushSize; i++)
			{
				for (int j = -brushSize + 1; j < brushSize; j++)
				{
					if (IsInArray(startPos + sf::Vector2i(i, j)) /*&& tileArray[gridX][gridY]->getID() != selectedTileID*/) {
						SetTile(startPos + sf::Vector2i(i, j), scaler);
					}
				}
			}


		}

		PreviewSelection(currentEditMode, selectedTileID, scaler, sf::Vector2f(startPos), sf::Color::Color(255, 255, 255, 50));

		rightWasPressed = rightPressed;
	}
						break;

	case TileMode::Select: {
		for (size_t i = 0; i < levelItems.size(); i++)
		{
			if (levelItems[i].CheckCollision(mousePosition) && sf::Mouse::isButtonPressed(sf::Mouse::Left)) {
				selectedTileID = levelItems[i].GetID();

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

			//std::cout << "Position Deleted: " << gridX << "," << gridY << std::endl;

			float scaler = GlobalVariables::getTextureScaler() * textures[0]->getSize().x;

			if (IsInArray(startPos) /*&& tileArray[gridX][gridY]->getID() != selectedTileID*/) {

			}

			for (int i = -brushSize + 1; i < brushSize; i++)
			{
				for (int j = -brushSize + 1; j < brushSize; j++)
				{
					if (IsInArray(startPos + sf::Vector2i(i, j)) /*&& tileArray[gridX][gridY]->getID() != selectedTileID*/) {
						DeleteTile(startPos + sf::Vector2i(i, j));
					}
				}
			}
		}

		PreviewSelection(currentEditMode, selectedTileID, scaler, sf::Vector2f(startPos), sf::Color::Color(0, 0, 0, 100));

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

void LevelEditor::InteractableMode(sf::RenderWindow& window, sf::Vector2f mousePosition)
{
	int gridX = mousePosition.x / cellSize;
	int gridY = mousePosition.y / cellSize;

	// Round the grid coordinates to the nearest whole number
	gridX = std::round(gridX);
	gridY = std::round(gridY);

	//std::cout << "Position Set: " << gridX << "," << gridY << std::endl;

	float scaler = GlobalVariables::getTextureScaler() * textures[0]->getSize().x;
	sf::Vector2f startPos = sf::Vector2f(gridX * scaler, gridY * scaler + cellSize);

	switch (currentTileMode) {

	case TileMode::Place: {


		bool rightPressed = false;

		if (sf::Mouse::isButtonPressed(sf::Mouse::Right)) {
			rightPressed = true;
		}

		if (rightPressed && !rightWasPressed) {
			SwapMode();
		}

		bool isPressed = false;

		if (sf::Mouse::isButtonPressed(sf::Mouse::Left) && clock.getElapsedTime() - timeOfSwitch > placeCoolDown) {

			isPressed = true;
		}

		if (isPressed && !wasKeyPressed2) {

			if (IsInArray(sf::Vector2i(gridX, gridY))) {
				interactables.push_back(Interactable(selectedInterID, startPos));
			}
		}

		rightWasPressed = rightPressed;
		wasKeyPressed2 = isPressed;

		PreviewSelection(currentEditMode, selectedInterID, scaler, startPos, sf::Color::Color(255, 255, 255, 100));
	}
						break;

	case TileMode::Select: {
		for (size_t i = 0; i < interactableItems.size(); i++)
		{
			if (interactableItems[i].CheckCollision(mousePosition) && sf::Mouse::isButtonPressed(sf::Mouse::Left)) {
				selectedInterID = interactableItems[i].GetID();

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

			for (size_t i = 0; i < interactables.size(); i++)
			{
				if (interactables[i].getCollision(mousePosition)) {
					interactables.erase(interactables.begin() + i);
					i--;
				}
			}
		}

		rightWasPressed = rightPressed;

		PreviewSelection(currentEditMode, selectedInterID, scaler, mousePosition, sf::Color::Color(0, 0, 0, 100));
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
	levelItems = std::vector<SelectionItem>();

	int x = 0;
	int y = 0;

	for (auto& pair : GlobalVariables::getTextures("level"))
	{
		sf::Vector2f position(y * 250 + 100, x * 250 + 100);

		levelItems.push_back(SelectionItem(pair.second, position, pair.first));

		if (x % 8 == 0) {
			x = 0;
			y++;
		}
	}

	interactableItems = std::vector<SelectionItem>();

	x = 0;
	y = 0;

	for (auto& pair : GlobalVariables::getTextures("interactable"))
	{
		sf::Vector2f position(y * 300 + 100, x * 250 + 100);

		interactableItems.push_back(SelectionItem(pair.second, position, pair.first));

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
	bool isPressed = false;
	if (sf::Mouse::isButtonPressed(sf::Mouse::Left)) {
		isPressed = true;
	}

	if (isPressed && !wasKeyPressed1) {

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

	wasKeyPressed1 = isPressed;
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
	bool isPressed = false;
	if (sf::Mouse::isButtonPressed(sf::Mouse::Left)) {
		isPressed = true;
	}

	if (isPressed && !wasKeyPressed1) {

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

		arraySize = endPos - startPos + sf::Vector2i(1, 1);

		std::cout << "Array Size: " << arraySize.x << "," << arraySize.y << std::endl;
	}

	wasKeyPressed1 = isPressed;

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

		data.clear();

		for (size_t i = 0; i < interactables.size(); i++)
		{
			// Must check to see if the interactable is in the desired area
			switch (interactables[i].getID()) {
			case 0: {
				data.push_back(
					std::to_string(0) + "," +
					std::to_string(interactables[i].getPosition().x) + "," +
					std::to_string(interactables[i].getPosition().y) + ",");
			}
			}
		}

		FileIO::WriteToFile("Levels/EditorTestIO.txt", data);
	}

	wasKeyPressed = isKeyPressed;
}

void LevelEditor::PreviewSelection(EditMode mode, int selectionID, float scaler, sf::Vector2f position, sf::Color color)
{
	previewSprites.clear();

	switch (mode) {
	case EditMode::Tile: {
		switch (currentTileMode) {
		case TileMode::Place: {
			for (int i = -brushSize + 1; i < brushSize; i++)
			{
				for (int j = -brushSize + 1; j < brushSize; j++)
				{
					if (IsInArray(sf::Vector2i(position) + sf::Vector2i(i, j))) {
						sf::Sprite sprite(*GlobalVariables::getTextures("level")[selectionID]);
						sprite.setPosition(position.x * scaler + i * scaler, position.y * scaler + j * scaler);
						sprite.setColor(color);
						sprite.setScale(GlobalVariables::getTextureScaler(), GlobalVariables::getTextureScaler());

						previewSprites.push_back(sprite);
					}
				}
			}
		}
							break;
		case TileMode::Delete: {
			if (IsInArray(sf::Vector2i(position))) {
				sf::Sprite sprite(*GlobalVariables::getTextures("level")[selectionID]);
				sprite.setPosition(position.x * scaler - (brushSize - 1) * scaler, position.y * scaler - (brushSize - 1) * scaler);
				sprite.setColor(color);
				sprite.setScale(GlobalVariables::getTextureScaler() * (brushSize - .5f) * 2, GlobalVariables::getTextureScaler() * (brushSize - .5f) * 2);

				previewSprites.push_back(sprite);
			}
		}
							 break;
		}
	}
					   break;
	case EditMode::Interactables: {
		switch (currentTileMode) {
		case TileMode::Place: {

			sf::Sprite sprite(*GlobalVariables::getTextures("interactable")[selectionID]);
			sprite.setPosition(position.x, position.y);
			sprite.setOrigin(sprite.getLocalBounds().getSize().x / 2, sprite.getLocalBounds().getSize().y);
			sprite.setColor(color);
			sprite.setScale(GlobalVariables::getTextureScaler(), GlobalVariables::getTextureScaler());

			previewSprites.push_back(sprite);

		}
							break;
		case TileMode::Delete: {

			sf::Sprite sprite(*GlobalVariables::getTextures("level")[0]);
			sprite.setPosition(position.x, position.y);
			sprite.setOrigin(sprite.getLocalBounds().getSize().y / 2, sprite.getLocalBounds().getSize().y / 2);
			sprite.setColor(color);

			previewSprites.push_back(sprite);
		}
							 break;
		}
	}
								break;
	}
}

void LevelEditor::LoadTileData(std::string filePath)
{
	std::vector<std::string> data = FileIO::ReadFromFile(filePath + ".txt");

	//std::cout << "Loaded File" << std::endl;

	//std::vector<std::string> dimensions = FileIO::Split(',', data[0]);

	sf::Vector2f scaler(textures[1]->getSize().x * GlobalVariables::getTextureScaler(), textures[1]->getSize().y * GlobalVariables::getTextureScaler());

	std::vector<std::string> playerPos = FileIO::Split(',', data[1]);

	playerStartPos = sf::Vector2f(std::stoi(playerPos[0]) * scaler.x, std::stoi(playerPos[1]) * scaler.y);

	//std::cout << "Loaded Level Dimensions" << std::endl;

	std::vector<std::vector<TileData*>> tileArray(arrayWidth, std::vector<TileData*>(arrayHeight));

	//std::cout << "Set Array Width" << std::endl;

	//std::cout << "Set Array height" << std::endl;

	for (size_t i = 0; i < arrayWidth; i++)
	{
		for (size_t j = 0; j < arrayHeight; j++)
		{
			tileArray[i][j] = new TileData();
		}
	}

	for (size_t i = 2; i < data.size(); i++)
	{
		std::string line = data[i];

		std::vector<std::string> lineData = FileIO::Split(',', line);

		for (size_t j = 0; j < lineData.size(); j++)
		{
			//std::cout << "Loaded {" << i << "," << j << "}" << std::endl;

			if (lineData[j] == "-1") continue;

			sf::Texture* texture;

			texture = textures[std::stoi(lineData[j])];

			sf::Vector2f position((i - 2) * texture->getSize().x * GlobalVariables::getTextureScaler(), j * texture->getSize().y * GlobalVariables::getTextureScaler());

			TileData* tile = new TileData(texture, position, GlobalVariables::getTextureScaler(), 0, sf::Vector2f(i, j));

			delete tileArray[i - 2][j];
			tileArray[i - 2][j] = tile;
			//std::cout << "Created Tile" << std::endl;

		}
	}

	this->tileArray = tileArray;

	//std::cout << "Loaded Tile Data" << std::endl;
}

void LevelEditor::CreateBB(std::string filePath)
{
	std::vector<std::string> data = FileIO::ReadFromFile(filePath + "BB.txt");

	//std::cout << "Read File" << std::endl;

	bbArray = std::vector<BoundingBox>();


	for (size_t i = 0; i < data.size(); i++)
	{
		std::string line = data[i];

		std::vector<std::string> lineData = FileIO::Split(',', line);

		sf::Vector2f scaler(textures[1]->getSize().x * GlobalVariables::getTextureScaler(), textures[1]->getSize().y * GlobalVariables::getTextureScaler());

		std::vector<std::string> pos1Data = FileIO::Split(':', lineData[0]);
		sf::Vector2f pos1(std::stoi(pos1Data[0]) * scaler.x, std::stoi(pos1Data[1]) * scaler.y);
		sf::Vector2i gridPos1(std::stoi(pos1Data[0]), std::stoi(pos1Data[1]));

		//std::cout << "Position 1 Created" << std::endl;

		std::vector<std::string> pos2Data = FileIO::Split(':', lineData[1]);
		sf::Vector2f pos2(std::stoi(pos2Data[0]) * scaler.x + scaler.x, std::stoi(pos2Data[1]) * scaler.y + scaler.x);
		sf::Vector2i gridPos2(std::stoi(pos2Data[0]), std::stoi(pos2Data[1]));

		//std::cout << "Position 2 Created" << std::endl;

		BoundingBox bb(pos1, pos2, sf::Color::Yellow, gridPos1, gridPos2);

		bbArray.push_back(bb);

		//std::cout << "Bounding Box Created" << std::endl;
	}
}

void LevelEditor::CreateCameraBB(std::string filePath)
{
	std::vector<std::string> data = FileIO::ReadFromFile(filePath + "CBB.txt");

	//std::cout << "Read File" << std::endl;


	for (size_t i = 0; i < data.size(); i++)
	{
		std::string line = data[i];

		std::vector<std::string> lineData = FileIO::Split(',', line);

		sf::Vector2f scaler(textures[1]->getSize().x * GlobalVariables::getTextureScaler(), textures[1]->getSize().y * GlobalVariables::getTextureScaler());

		std::vector<std::string> pos1Data = FileIO::Split(':', lineData[0]);
		sf::Vector2f pos1(std::stoi(pos1Data[0]) * scaler.x, std::stoi(pos1Data[1]) * scaler.y);
		sf::Vector2i gridPos1(std::stoi(pos1Data[0]), std::stoi(pos1Data[1]));

		//std::cout << "Position 1 Created" << std::endl;

		std::vector<std::string> pos2Data = FileIO::Split(':', lineData[1]);
		sf::Vector2f pos2(std::stoi(pos2Data[0]) * scaler.x + scaler.x, std::stoi(pos2Data[1]) * scaler.y + scaler.x);
		sf::Vector2i gridPos2(std::stoi(pos2Data[0]), std::stoi(pos2Data[1]));

		//std::cout << "Position 2 Created" << std::endl;

		BoundingBox bb(pos1, pos2, sf::Color::Magenta, gridPos1, gridPos2);


		cameraArray.push_back(bb);

		//std::cout << "Bounding Box Created" << std::endl;
	}
}

void LevelEditor::CreateInteractables(std::string filePath)
{
	std::vector<std::string> data = FileIO::ReadFromFile(filePath + "IO.txt");

	//std::cout << "Read File" << std::endl;


	for (size_t i = 0; i < data.size(); i++)
	{
		std::string line = data[i];

		std::vector<std::string> lineData = FileIO::Split(',', line);

		sf::Vector2f scaler(textures[1]->getSize().x * GlobalVariables::getTextureScaler(), textures[1]->getSize().y * GlobalVariables::getTextureScaler());

		int ID = stoi(lineData[0]);
		//std::vector<std::string> pos1Data = FileIO::Split(':', lineData[0]);
		sf::Vector2f pos1(std::stof(lineData[1]), std::stof(lineData[2]));

		Interactable* interactable = nullptr;

		switch (ID) {
		case 0: {
			interactable = new Interactable(ID, pos1);
		}
		}

		interactables.push_back(*interactable);
	}
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

	// Get only the area that the camera can see to draw
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	int scaler = GlobalVariables::getTextureScaler() * GlobalVariables::getTextures("level")[1]->getSize().x;
	int buffer = 10;
	int halfWidth = (ViewManager::Instance()->GetWindowView().getSize().x / 2);
	int halfHeight = (ViewManager::Instance()->GetWindowView().getSize().y / 2);

	sf::Vector2f cameraCenter = ViewManager::Instance()->GetWindowView().getCenter();

	// Start Position
	int gridX = (cameraCenter.x - halfWidth) / scaler;
	int gridY = (cameraCenter.y - halfHeight) / scaler;

	gridX = std::round(gridX) - buffer;
	gridY = std::round(gridY) - buffer;

	sf::Vector2i startPos = sf::Vector2i(clamp(gridX, 0, arrayWidth), clamp(gridY, 0, arrayHeight));
	//std::cout << "Start Position Set: " << startPos.x << "," << startPos.y << std::endl;
	// End Position
	gridX = (cameraCenter.x + halfWidth) / scaler;
	gridY = (cameraCenter.y + halfHeight) / scaler;

	gridX = std::round(gridX) + buffer;
	gridY = std::round(gridY) + buffer;

	sf::Vector2i endPos = sf::Vector2i(clamp(gridX, 0, arrayWidth), clamp(gridY, 0, arrayHeight));

	//std::cout << "End Position Set: " << endPos.x << "," << endPos.y << std::endl;
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	for (size_t i = startPos.x; i < endPos.x; i++)
	{
		for (size_t j = startPos.y; j < endPos.y; j++)
		{
			if (IsInArray(sf::Vector2i(i, j)) && !tileArray[i][j]->IsActive()) continue;

			tileArray[i][j]->Draw(window);

			//std::cout << "Drew Tile: {" << i << "," << j << "}" << std::endl;
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

	for (size_t i = 0; i < interactables.size(); i++)
	{
		interactables[i].Draw(window);
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

			for (size_t i = 0; i < levelItems.size(); i++)
			{
				levelItems[i].Draw(window, 10);
			}
		}
							 break;
		case TileMode::Delete:
		case TileMode::Place: {
			for (size_t i = 0; i < previewSprites.size(); i++)
			{
				window.draw(previewSprites[i]);
			}

		}

							break;
		}
	}
					   break;
	case EditMode::Interactables: {
		switch (currentTileMode) {
		case TileMode::Select: {

			sf::RectangleShape rect(ViewManager::Instance()->GetWindowView().getSize());
			rect.setOrigin(rect.getSize().x / 2, rect.getSize().y / 2);
			rect.setPosition(ViewManager::Instance()->GetWindowView().getCenter());

			sf::Color color = sf::Color::White;
			color.a = 50;

			rect.setFillColor(color);

			window.draw(rect);

			for (size_t i = 0; i < interactableItems.size(); i++)
			{
				interactableItems[i].Draw(window, 4);
			}
		}
							 break;
		case TileMode::Place:
		case TileMode::Delete: {
			for (size_t i = 0; i < previewSprites.size(); i++)
			{
				window.draw(previewSprites[i]);
			}
		}
							 break;
		}
	}
								break;

	}

	previewSprites.clear();
}

void LevelEditor::LoadLevel()
{
	std::vector<std::string> data = FileIO::ReadFromFile("Levels/LoadLevel.txt");

	if (data.size() == 0) return;

	std::string path = data[0];

	LoadTileData(path);
	CreateBB(path);
	CreateCameraBB(path);
	CreateInteractables(path);
}
