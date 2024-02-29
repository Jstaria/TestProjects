#include <SFML/Window/Event.hpp>

using namespace std;
using namespace sf;

class InputController {

	Vector2f GetDirection() {

		Vector2f direction(0, 0);

		if (Keyboard::isKeyPressed(Keyboard::W)) {
			direction += Vector2f(0, 1);
		}

		if (Keyboard::isKeyPressed(Keyboard::A)) {
			direction += Vector2f(-1, 0);
		}

		if (Keyboard::isKeyPressed(Keyboard::S)) {
			direction += Vector2f(0, -1);
		}

		if (Keyboard::isKeyPressed(Keyboard::D)) {
			direction += Vector2f(1, 0);
		}

		return direction;
	}

};

