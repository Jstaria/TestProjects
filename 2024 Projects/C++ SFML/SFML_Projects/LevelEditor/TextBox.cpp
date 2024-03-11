#include "TextBox.h"

TextBox::TextBox(sf::Font font, sf::Vector2f position)
{
	box = BoundingBox(position, position + sf::Vector2f(100, 50), sf::Color::Black);

	text.setFont(font); // Set the font
	text.setString("0"); // Set the text
	text.setCharacterSize(24); // Set the character size
	text.setFillColor(sf::Color::Red); // Set the fill color
	text.setPosition(position); // Set the position
	relativePosition = position;
}

TextBox::TextBox()
{
}

void TextBox::Update()
{
	
}

void TextBox::Draw(sf::RenderWindow& window)
{
	SetPositionRelativeToView(window, relativePosition);
	//window.draw(text);
	box.Draw(window);
}

void TextBox::SetPositionRelativeToView(sf::RenderWindow& window, sf::Vector2f relativePos) {
	sf::View view = window.getView();
	sf::Vector2f viewCenter = view.getCenter();
	sf::Vector2f viewSize = view.getSize();

	// Calculate position relative to view
	float posX = viewCenter.x - (viewSize.x / 2.0f) + relativePos.x;
	float posY = viewCenter.y - (viewSize.y / 2.0f) + relativePos.y;

	box.MoveTo(sf::Vector2f(posX, posY));
	text.setPosition(posX + 5, posY + 5);
}