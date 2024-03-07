#pragma once
#include "Entity.h"
#include <SFML/Window/Context.hpp>
#include <iostream>
#include <map>
#include <functional>
#include "HelperFunctions.h"
#include "BoundingBox.h"
#include "Level.h"

enum PlayerState {
    Idle,
    Walk,
    Jump
};

class Player :
    public Entity
{
public:
    Player(std::map<std::string,sf::Sprite>* sprites, sf::Vector2f position, int maxFrames);

    void Draw(sf::RenderWindow& window);
    void Update();

    void setCurrentLevel(Level* level);

private:

    PlayerState currentState;

    int count;
    int speedMultiplier;

    sf::Vector2f velocity;
    sf::Vector2f maxVelocity;
    float jumpVelocity;
    float currentGravity;
    float defaultGravity;
    float acceleration;
    float deceleration;
    bool isGrounded;
    bool canJump;
    bool wasPressingKey;

    int lastFacedDirectionX;
    Level* currentLevel;

    sf::Clock clock;
    sf::Time timeOfGrounded;
    sf::Time timeOfJumpPress;
    sf::Time coyoteTime;

    std::map<std::string, BoundingBox> boundingBoxes;

    void GetCurrentSprite();
    void IncrementFrameNum(int modulus);
    sf::Sprite CreateSprite(const sf::Texture* texture);
    
    void Move(sf::Vector2f speed);
    void UpdateJump(bool anyCollision);
    void UpdateKeyboardControls(bool* isMoving, bool canMoveRight, bool canMoveLeft);

    void MoveTo(sf::Vector2f pos);

    void SetGravityScale(float newGravity);
    void ResetGravity();

    sf::FloatRect GetFutureRect(bool useX, bool useY);
    void CreateBB();
};

