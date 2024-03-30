#include "Interactable.h"

Interactable::Interactable(int ID, sf::Vector2f position)
{
	id = ID;
	sprite = sf::Sprite(*GlobalVariables::getTextures("interactable")[ID]);
	sprite.setOrigin(
		sprite.getGlobalBounds().getSize().x / 2,
		sprite.getGlobalBounds().getSize().y);
	sprite.setPosition(position);
	sprite.setScale(GlobalVariables::getTextureScaler(), GlobalVariables::getTextureScaler());

	bb = BoundingBox(
		sf::Vector2f(sprite.getGlobalBounds().left, sprite.getGlobalBounds().top),
		sf::Vector2f(
			sprite.getGlobalBounds().left + sprite.getGlobalBounds().width,
			sprite.getGlobalBounds().top + sprite.getGlobalBounds().height),
		sf::Color::Blue, sf::Vector2i(0, 0), sf::Vector2i(0, 0));
}

int Interactable::getID()
{
	return id;
}

void Interactable::Draw(sf::RenderWindow& window)
{
	window.draw(sprite);

	//sf::CircleShape circle(5);
	//circle.setOrigin(2.5, 2.5);
	//circle.setFillColor(sf::Color::Red);
	//circle.setPosition(sprite.getPosition());
	//window.draw(circle);
}

bool Interactable::getCollision(sf::Vector2f pos)
{
	return bb.CheckCollision(pos);
}

sf::Vector2f Interactable::getPosition()
{
	return sprite.getPosition();
}
