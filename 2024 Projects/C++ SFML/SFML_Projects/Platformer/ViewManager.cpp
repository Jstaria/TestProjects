#include "ViewManager.h"
#include "GlobalVariables.h"

ViewManager* ViewManager::s_instance = 0;

ViewManager::ViewManager()
{
	lerpSpeed = .15;
}

ViewManager* ViewManager::Instance()
{
	if (s_instance == nullptr) {
		s_instance = new ViewManager();
	}
	return s_instance;
}

void ViewManager::SetWindowView(sf::View* view)
{
	ViewManager::view = view;
}

sf::View ViewManager::GetWindowView()
{
	return *view;
}

void ViewManager::SetCameraPosition(sf::Vector2f targetPosition)
{
	this->targetPosition = targetPosition;
}

void ViewManager::SetViewZoom(float delta)
{
	view->zoom(delta);
}

void ViewManager::MoveView(sf::Vector2f direction)
{
	view->move(direction);
}

void ViewManager::UpdateView()
{
	position.x = lerp(position.x, targetPosition.x, .15);
	position.y = lerp(position.y, targetPosition.y, .05);

	view->setCenter(position);

	int angle = magnitude *
		GlobalVariables::getNoise().GetNoise(0.f,
			(float)GlobalVariables::getClock().getElapsedTime().asMilliseconds());

	angle %= 360;

	view->setRotation(angle);

	angle = displace *
		GlobalVariables::getNoise().GetNoise(0.f,
			(float)GlobalVariables::getClock().getElapsedTime().asMilliseconds());

	angle %= 360;

	float offsetX = sin(angle) * displace;
	float offsetY = cos(angle) * displace;

	view->move(offsetX, offsetY);

	if (displace < 0.01 && displace > 0) displace = 0;
	if (displace > -0.01 && displace < 0) displace = 0;
	else displace *= shakeStrength;

	if (magnitude > 0) magnitude -= shakeStrength;
	if (magnitude < 0) magnitude += shakeStrength;

	//view->zoom(.999001);
}

void ViewManager::ResetPosition()
{
	view->setCenter(sf::Vector2f(1920 / 2, 1080 / 2));
	view->zoom(1920 / view->getSize().x);
}

void ViewManager::setLerpSpeed(float speed)
{
	lerpSpeed = speed;
}

void ViewManager::shakeCamera(float maxAngle, float maxDistance, float strength, float frequency)
{
	//GlobalVariables::getNoise().SetFrequency(frequency);
	shakeStrength = strength;
	magnitude = maxAngle;
	displace = maxDistance;
}
