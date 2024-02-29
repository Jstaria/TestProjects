#include <SFML/Graphics.hpp>
#include "GameState.h"
#include "SceneManager.h"

SceneManager* SceneManager::s_instance = 0;

GameState SceneManager::currentState = Menu;

SceneManager::SceneManager() {
	s_instance = this;
	currentState = GameState::Menu;
}

SceneManager* SceneManager::Instance() {
	if (s_instance == nullptr) {
		s_instance = new SceneManager();
	}

	return s_instance;
}

void SceneManager::ChangeState(GameState state) {
	currentState = state;
}