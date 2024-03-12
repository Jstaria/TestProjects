#include "LevelEditor.h"

LevelEditor::LevelEditor()
{
	arrayWidth = 200;
	arrayHeight = 200;

	cellSize = 1;
}

void LevelEditor::Update(sf::RenderWindow& window)
{
	//cellSize =
	//	GlobalVariables::Instance()->getTextures()[0].getSize().x *
	//	GlobalVariables::Instance()->getTextureScaler() *
	//	1920 / ViewManager::Instance()->GetWindowView().getSize().x;
	//
	//sf::Vector2f viewCenter = ViewManager::Instance()->GetWindowView().getCenter();
	//sf::Vector2f viewSize = ViewManager::Instance()->GetWindowView().getSize();
	sf::Vector2i mousePosition = sf::Mouse::getPosition(window); //+sf::Vector2i(viewCenter - viewSize * .5f);

	if (sf::Mouse::isButtonPressed(sf::Mouse::Left)) {
		
		int gridX = mousePosition.x / cellSize;
		int gridY = mousePosition.y / cellSize;

		// Round the grid coordinates to the nearest whole number
		gridX = std::round(gridX);
		gridY = std::round(gridY);
		
		startPos = sf::Vector2i(gridX, gridY);

		std::cout << gridX << "," << gridY << std::endl;

		leftPressed = true;
	}

	if (sf::Mouse::isButtonPressed(sf::Mouse::Right) && leftPressed) {
		int gridX = mousePosition.x / cellSize;
		int gridY = mousePosition.y / cellSize;

		// Round the grid coordinates to the nearest whole number
		gridX = std::round(gridX);
		gridY = std::round(gridY);

		std::cout << gridX << "," << gridY << std::endl;

		endPos = sf::Vector2i(gridX, gridY);
	}
}

void LevelEditor::GetMouseInteraction()
{
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
}
