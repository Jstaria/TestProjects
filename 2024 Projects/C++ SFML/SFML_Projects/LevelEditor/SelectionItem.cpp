#include "SelectionItem.h"

void SelectionItem::UpdateBB(sf::Vector2f position, float size)
{
	sf::FloatRect rectPos(
		position - sf::Vector2f(10, 10), 
		sf::Vector2f(size + 20, size + 20));

	bb.setRect(rectPos);
}

SelectionItem::SelectionItem(sf::Texture* texture, sf::Vector2f positionOffset, int id)
{
	this->positionOffset = positionOffset;
	this->tileID = id;

	sprite.setTexture(*texture);
	rect.setSize(sf::Vector2f(texture->getSize()));
	sf::Color color = sf::Color::Black;
	color.a = 50;
	rect.setFillColor(color);

	isInCollision = false;
}

bool SelectionItem::CheckCollision(sf::Vector2f mousePosition)
{
	return (isInCollision = bb.CheckCollision(mousePosition));
}

void SelectionItem::Draw(sf::RenderWindow& window)
{
	float scaler = (ViewManager::Instance()->GetWindowView().getSize().x / 1920);
	int scale = 10;

	if (isInCollision) {
		scale = 11;
	}

	sprite.setScale(
		scaler * scale,
		scaler * scale);
	
	sf::Vector2f position(ViewManager::Instance()->GetWindowView().getCenter() -
		(ViewManager::Instance()->GetWindowView().getSize() * .5f) +
		positionOffset * scaler);

	sprite.setPosition(position);
	rect.setPosition(position - (sf::Vector2f(10,10) * scaler));
	
	rect.setScale(
		scaler * scale,
		scaler * scale);

	UpdateBB(position, scaler * rect.getSize().x * 10);

	window.draw(rect);
	window.draw(sprite);
}

int SelectionItem::GetID()
{
	return tileID;
}


