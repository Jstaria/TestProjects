#ifndef PLAYER_HPP
#define PLAYER_HPP

#include <../../SFML_Projects/Entity.hpp>
#include <../../SFML_Projects/InputController.cpp>

class Player : public Entity {
protected:
	InputController inputCon;

public:
	Player(Texture& texture, Vector2f position);

	virtual void Draw(RenderTexture& target);
};

#endif