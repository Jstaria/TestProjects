#include <../../SFML_Projects/Entity.hpp>

class Player : Entity {

private:
	Sprite& sprite;
	Vector2f position;

public:
	Player(Sprite& sprite, Vector2f position);

	void Draw(RenderTarget& target);
};