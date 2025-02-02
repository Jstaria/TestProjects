#include "GameManager.h"
#include "Light.h"

Light light;

GameManager::GameManager(Player* player, Input* input) : player(player), input(input)
{
	camera = new Camera();
	GlobalVariables::setCamera(camera);
}

void GameManager::Draw(sf::RenderWindow& window)
{
	camera->Draw(window);
	currentLevel->Draw(window);
	player->Draw(window);
 	light.Draw(window);
}

void GameManager::Update()
{
	player->Update();
	camera->Update();
	input->Update();
	currentLevel->Update();
	ViewManager::Instance()->UpdateView();
	light = Light(GlobalVariables::getPlayerPosition(), 360, 0, 400, 100);
	light.CalculateMesh(*currentLevel->getBBArray());
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
