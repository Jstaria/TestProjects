#include "GameManager.h"

GameManager::GameManager(Player* player) : player(player)
{
}

void GameManager::Draw(sf::RenderWindow& window)
{
	currentLevel->Draw(window);
	player->Draw(window);
}

void GameManager::Update()
{
	player->Update();
}

void GameManager::SetLevel(std::string levelPath)
{
	if (currentLevel != nullptr) {
		delete currentLevel;
	}

	currentLevel = new Level(levelPath);
	player->setCurrentLevel(currentLevel);
}
