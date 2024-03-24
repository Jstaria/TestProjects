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
	Input* input;

public:
	GameManager(Player* player, Input* input);

	void Draw(sf::RenderWindow& window);

	void Update();

	void SetLevel(std::string levelPath);
};

