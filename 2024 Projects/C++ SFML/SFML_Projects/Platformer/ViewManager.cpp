#include "ViewManager.h"

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

void ViewManager::shakeCamera(float maxAngle, float maxDistance, float strength)
{
}
