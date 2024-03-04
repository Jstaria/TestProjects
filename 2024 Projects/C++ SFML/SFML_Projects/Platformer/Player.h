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

    sf::Vector2f velocity;
    sf::Vector2f maxVelocity;
    float jumpVelocity;
    float gravity;
    float acceleration;
    float deceleration;
    bool isGrounded;


    int lastFacedDirectionX;
    Level* currentLevel;

    sf::Clock clock;
    sf::Time timeOfGrounded;
    sf::Time coyoteTime;

    std::map<std::string, BoundingBox> boundingBoxes;

    sf::Sprite GetCurrentSprite(sf::Sprite& currentSprite, int xDirection);
    
    void Move(sf::Vector2f speed);
    void MoveTo(sf::Vector2f pos);

    sf::FloatRect GetFutureRect();
    void CreateBB();
};

