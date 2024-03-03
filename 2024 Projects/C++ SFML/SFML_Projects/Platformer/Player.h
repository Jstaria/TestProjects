#pragma once
#include "Entity.h"
#include <SFML/Window/Context.hpp>
#include <iostream>
#include <map>
#include <functional>
#include "HelperFunctions.h"
#include "BoundingBox.h"
#include "Level.h"

class Player :
    public Entity
{
public:
    Player(std::map<std::string,sf::Sprite>* sprites, sf::Vector2f position, int maxFrames);

    void Draw(sf::RenderWindow& window);
    void Update();

    void setCurrentLevel(Level* level);

private:

    int count;
    int speedMultiplier;
    sf::Vector2f direction;
    int lastFacedDirectionX;
    Level* currentLevel;

    std::map<std::string, BoundingBox> boundingBoxes;

    sf::Sprite GetCurrentSprite(sf::Sprite& currentSprite, int xDirection);
    
    void Move(sf::Vector2f speed);
    void MoveTo(sf::Vector2f pos);

    void CreateBB();
};

