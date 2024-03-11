#pragma once
#include "HelperFunctions.h"
#include "GlobalVariables.h"
#include <SFML/Graphics.hpp>


class ViewManager {
private:
	static ViewManager* s_instance;
	sf::Vector2f position;
	sf::Vector2f targetPosition;
	sf::View* view;

public:
	ViewManager();

	static ViewManager* Instance();

	void SetWindowView(sf::View* view);
	sf::View GetWindowView();
	void SetCameraPosition(sf::Vector2f targetPosition);
	void SetViewZoom(float delta);

	void UpdateView();
};