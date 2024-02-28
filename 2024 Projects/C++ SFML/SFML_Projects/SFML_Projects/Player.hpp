#ifndef PLAYER_HPP
#define PLAYER_HPP

#include <../../SFML_Projects/Entity.hpp>

class Player : public Entity {

public:
	Player(Texture& texture, Vector2f position);

	virtual void Draw(RenderTexture& target);
};

#endif