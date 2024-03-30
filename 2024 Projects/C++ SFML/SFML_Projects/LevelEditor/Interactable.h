#pragma once
#include <SFML/Graphics.hpp>
#include "GlobalVariables.h"
#include "BoundingBox.h"

class Interactable
{
private:
	int id;
	sf::Sprite sprite;
	BoundingBox bb;

public:
	Interactable(int ID, sf::Vector2f position);

	void Draw(sf::RenderWindow& window);

	bool getCollision(sf::Vector2f pos);

	int getID();

	sf::Vector2f getPosition();
};

