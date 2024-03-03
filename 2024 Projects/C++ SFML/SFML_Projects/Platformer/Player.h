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
    Player(std::map<std::string,sf::Sprite>* sprites, sf::Vector2f position, int maxFrames);

    void Draw(sf::RenderWindow& window);
    void Update();

private:

    int count;
    int speedMultiplier;
    sf::Vector2f direction;
    int lastFacedDirectionX;

    sf::Sprite GetCurrentSprite(sf::Sprite& currentSprite, int xDirection);
    
    void Move(sf::Vector2f speed);
};

