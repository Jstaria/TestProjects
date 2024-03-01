#pragma once
#include "Entity.h"
#include <SFML/Window/Context.hpp>
#include <iostream>
#include <map>
#include <functional>
#include "HelperFunctions.h"

class Player :
    public Entity
{
public:
    Player(sf::Sprite sprite, sf::Vector2f position);

    void Update();

private:

    int speedMultiplier;
    sf::Vector2f direction;

    void Move(sf::Vector2f speed);
};

