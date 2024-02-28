#include <../../SFML_Projects/Player.hpp>
#include <SFML/Graphics.hpp>

Player::Player(Sprite& sprite, Vector2f position) : Entity(sprite, position), sprite(sprite), position(position) {
}

void Player::Draw(RenderTarget& target) {

}