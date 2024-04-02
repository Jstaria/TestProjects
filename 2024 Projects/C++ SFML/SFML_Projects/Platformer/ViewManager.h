#pragma once
#include "HelperFunctions.h"
#include "GlobalVariables.h"
#include <SFML/Graphics.hpp>
#include <FastNoiseLite.h>

class ViewManager {
private:
	static ViewManager* s_instance;
	sf::Vector2f position;
	sf::Vector2f targetPosition;
	float lerpSpeed;
	sf::View* view;

public:
	ViewManager();

	static ViewManager* Instance();

	void SetWindowView(sf::View* view);
	sf::View GetWindowView();
	void SetCameraPosition(sf::Vector2f targetPosition);
	void SetViewZoom(float delta);
	void MoveView(sf::Vector2f direction);
	void UpdateView();
	void ResetPosition();
	void setLerpSpeed(float speed);
	void shakeCamera(float maxAngle, float maxDistance, float strength);
};