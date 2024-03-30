#pragma once

#include "BoundingBox.h"
#include <SFML/Graphics.hpp>
#include <iostream>
#include "ViewManager.h"
#include "GlobalVariables.h"

class SelectionItem
{
private:
	sf::RectangleShape rect;
	sf::Vector2f positionOffset;
	BoundingBox bb;
	sf::Sprite sprite;

	bool isInCollision;
	int tileID;

	void UpdateBB(sf::Vector2f position, float size);

public:
	SelectionItem(sf::Texture* texture, sf::Vector2f position, int id);

	bool CheckCollision(sf::Vector2f mousePosition);

	void Draw(sf::RenderWindow& window, float scale);

	int GetID();
};

