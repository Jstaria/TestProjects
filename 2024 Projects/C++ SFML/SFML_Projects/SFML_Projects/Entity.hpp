
#ifndef ENTITY_HPP
#define ENTITY_HPP

#include <iostream>
#include <SFML/Graphics.hpp>

using namespace sf;
using namespace std;

class Entity {

protected:

	Texture& texture;
	Vector2f position;

public:

	Entity(Texture& texture, Vector2f position)
		: texture(texture), position(position) {
	}

	void Draw(RenderTexture& target);/* {
		sprite.setPosition(position);
		target.draw(sprite);
	}*/

	void Update();
};

#endif