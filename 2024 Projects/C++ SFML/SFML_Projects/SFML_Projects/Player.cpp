#include <../../SFML_Projects/Player.hpp>
#include <SFML/Graphics.hpp>

unsigned int frameIndex = 0;
unsigned int rowIndex = 0;
unsigned int Count = 0;

const int rowCount = 4;
const int colCount = 4;

Player::Player(Texture& texture, Vector2f position) : Entity(texture, position) {
	this->texture = texture;

	this->position = position;
}

IntRect FrameRect(int rowIndex, int frameWidth, int frameHeight) {
	return IntRect(frameIndex * frameWidth, rowIndex * frameHeight, frameWidth, frameHeight);
}

void Player::Entity::Update() {
	position += inputCon->GetDirection();
}

void Player::Draw(RenderTexture& target) {

	Sprite sprite(texture, FrameRect(0, 17, 25));

	sprite.setOrigin(sprite.getLocalBounds().width / 2, sprite.getLocalBounds().height / 2);
	sprite.setPosition(position);
	sprite.setScale(Vector2f(10,10));

	target.draw(sprite);

	if (Count % 15 == 0) {
		frameIndex = (frameIndex + 1) % colCount;
		Count = 0;
	}
	
	Count++;
}