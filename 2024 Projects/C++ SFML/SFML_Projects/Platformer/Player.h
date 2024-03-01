#pragma once
#include "Entity.h"
#include <SFML/Window/Context.hpp>
#include <iostream>
#include <map>
#include <functional>

class Player :
    public Entity
{
public:
    Player(sf::Sprite sprite, sf::Vector2f position);

    void Update();

private:

    int speed;

    void SetKeyMap();
    void Move(sf::Vector2f speed);
    sf::Vector2f Normalize(sf::Vector2f& vector, int multiplier);
};

