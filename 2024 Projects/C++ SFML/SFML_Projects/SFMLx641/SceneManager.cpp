#include <SFML/Graphics.hpp>
#include "GameState.h"

enum GameState {
	Playing,
	Paused,
	EndState,
	Menu
};

class SceneManager {
private:
	static SceneManager* s_instance;

	static GameState currentState;

public:
	SceneManager() {
		s_instance = this;
		currentState = Menu;
	}

	static SceneManager* Instance() {
		if (s_instance == NULL) {
			s_instance = new SceneManager();
		}

		return s_instance;
	}

	void ChangeState(GameState state) {
		currentState = state;
	}
};

SceneManager* SceneManager::s_instance = 0;