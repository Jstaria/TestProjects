#include "ViewControl.h"

ViewControl::ViewControl()
{
}

void ViewControl::SetDelta(float delta)
{
}

void ViewControl::Update()
{
	bool middleIsPressed = false;
	sf::Vector2f difference;

	if (sf::Mouse::isButtonPressed(sf::Mouse::Middle) && !middleWasPressed) {
		mouseStartPos = sf::Mouse::getPosition();
		middleIsPressed = true;
	}
	else if (sf::Mouse::isButtonPressed(sf::Mouse::Middle)) {
		difference = sf::Vector2f(mouseStartPos - sf::Mouse::getPosition());
	}

	if (sf::Keyboard::isKeyPressed(sf::Keyboard::W)) {
		ViewManager::Instance()->MoveView(
			sf::Vector2f(0, -10 * 
				(ViewManager::Instance()->GetWindowView().getSize().x / 1920)));
	}
	if (sf::Keyboard::isKeyPressed(sf::Keyboard::A)) {
		ViewManager::Instance()->MoveView(
			sf::Vector2f(-10 *
				(ViewManager::Instance()->GetWindowView().getSize().x / 1920), 0));
	}
	if (sf::Keyboard::isKeyPressed(sf::Keyboard::S)) {
		ViewManager::Instance()->MoveView(
			sf::Vector2f(0, 10 *
				(ViewManager::Instance()->GetWindowView().getSize().x / 1920)));
	}
	if (sf::Keyboard::isKeyPressed(sf::Keyboard::D)) {
		ViewManager::Instance()->MoveView(
			sf::Vector2f(10 *
				(ViewManager::Instance()->GetWindowView().getSize().x / 1920), 0));
	}

	if (sf::Keyboard::isKeyPressed(sf::Keyboard::Hyphen)) {
		ViewManager::Instance()->SetViewZoom(1.05f);
	}
	else if (sf::Keyboard::isKeyPressed(sf::Keyboard::Equal)) {
		ViewManager::Instance()->SetViewZoom(.95f);
	}

	ViewManager::Instance()->SetCameraPosition(
		difference * (ViewManager::Instance()->GetWindowView().getSize().x / 1920)
		+ ViewManager::Instance()->GetWindowView().getCenter());
	ViewManager::Instance()->UpdateView();

	middleWasPressed = middleIsPressed;
}
