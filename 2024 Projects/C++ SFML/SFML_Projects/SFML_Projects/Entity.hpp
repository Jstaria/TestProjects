#pragma once
#include <iostream>
#include <SFML/Graphics.hpp>

using namespace sf;
using namespace std;

class Entity {

private:

	Sprite& sprite;
	Vector2f position;

public:

	Entity(Sprite& sprite, Vector2f position);

	void Draw(RenderTexture& target);
};