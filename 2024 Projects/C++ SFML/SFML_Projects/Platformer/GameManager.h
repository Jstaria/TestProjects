#pragma once
#include "Player.h"
#include "SceneManager.h"
#include "Level.h"
#include "Camera.h"

class GameManager
{

private:
	Player* player;
	Level* currentLevel;
	Camera* camera;

public:
	GameManager(Player* player);

	void Draw(sf::RenderWindow& window);

	void Update();

	void SetLevel(std::string levelPath);
};

