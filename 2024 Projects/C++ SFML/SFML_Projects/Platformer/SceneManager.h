#pragma once

#include "GameState.h"

class SceneManager {
private:
	static SceneManager* s_instance;

	static GameState currentState;

public:
	SceneManager();

	static SceneManager* Instance();

	void ChangeState(GameState state);
};