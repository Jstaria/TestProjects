#pragma once

#include <SFML/Graphics.hpp>
#include <SFML/Window.hpp>
#include "ViewManager.h"

class ViewControl
{
private:
	sf::Vector2i mouseStartPos;
	bool middleWasPressed;

public:
	ViewControl();
	
	void SetDelta(float delta);

	void Update();
};

