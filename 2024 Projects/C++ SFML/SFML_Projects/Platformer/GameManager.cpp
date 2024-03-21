#include "GameManager.h"

GameManager::GameManager(Player* player) : player(player)
{
	camera = new Camera();
}

void GameManager::Draw(sf::RenderWindow& window)
{
	camera->Draw(window);
	currentLevel->Draw(window);
	player->Draw(window);
}

void GameManager::Update()
{
	player->Update();
	camera->Update();
	ViewManager::Instance()->UpdateView();
}

void GameManager::SetLevel(std::string levelPath)
{
	if (currentLevel != nullptr) {
		delete currentLevel;
	}

	while (camera->GetBoundingEdges().size() > 0) {
		camera->RemoveLastEdge();
	}

	currentLevel = new Level(levelPath, camera);
	player->setCurrentLevel(currentLevel);
}
